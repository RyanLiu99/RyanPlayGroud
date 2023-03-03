void Main()
{
	ServiceCollection services = new ServiceCollection();
	
	services.AddMemoryCache();
	services.Decorate<IMemoryCache, InvalidatingMemoryCache>();
	services.AddSingleton<IMemoryCacheInvalidator, InvalidatingMemoryCache>(sp => (InvalidatingMemoryCache) sp.GetRequiredService<IMemoryCache>());
​
	services.AddInvalidation<SomeObject, int>("SomeAggregateRoot", so => so.IntProperty);
	services.AddInvalidation<SomeObject, string>("SomeOtherAggregateRoot", so => so.StrProperty);
	services.AddInvalidation<SomeObject, Guid>("SomeThirdAggregateRoot", so => so.GuidProperty);
​
	IServiceProvider provider = services.BuildServiceProvider();
	
	IMemoryCache cache = provider.GetRequiredService<IMemoryCache>();
​
	Guid[] guids = new Guid[] { Guid.Parse("10005242-0d5c-45af-be23-44ac9887a547"), Guid.Parse("e0c83200-552d-4d50-b759-ff0b1dcc9246"), Guid.Parse("e2fb5b66-1dc7-442d-b63d-ce02f0ecf270") };
​
	cache.Set(468, new SomeObject { IntProperty = 999, StrProperty = "someStr0", GuidProperty = guids[0] });
	cache.Set(469, new SomeObject { IntProperty = 998, StrProperty = "someStr1", GuidProperty = guids[1] });
	cache.Set(470, new SomeObject { IntProperty = 997, StrProperty = "someStr2", GuidProperty = guids[2] });
	
	IMemoryCacheInvalidator invalidator = provider.GetRequiredService<IMemoryCacheInvalidator>();
​
	invalidator.Invalidate("SomeAggregateRoot", 997);
	invalidator.Invalidate("SomeOtherAggregateRoot", "someStr1");
	invalidator.Invalidate("SomeThirdAggregateRoot", guids[0]);
}
​
//
public class SomeObject
{
	public int IntProperty {get; set;}
	public string StrProperty {get; set;}
	public Guid GuidProperty {get; set;}
}
//
​
public interface IMemoryCacheInvalidator
{
	void Invalidate<TKey>(string aggregateName, TKey value);
}
​
public class InvalidatingMemoryCache : IMemoryCache, IMemoryCacheInvalidator
{
	private readonly IMemoryCache _internalCache;
	private readonly IEnumerable<InvalidationKey> _invalidationKeys;
	private readonly ConcurrentDictionary<string, ConcurrentDictionary<object, object>> _index = new ConcurrentDictionary<string, ConcurrentDictionary<object, object>>();
	
	public InvalidatingMemoryCache(IMemoryCache cache, IEnumerable<InvalidationKey> invalidationKeys)
	{
		_internalCache = cache;
		_invalidationKeys = invalidationKeys;
	}
​
	ICacheEntry IMemoryCache.CreateEntry(object key)
	{
		ICacheEntry entry = _internalCache.CreateEntry(key);
​
		entry = new ChangeCallbackCacheEntry(entry, (oldVal, newVal) => 
		{
			deindex(key);			
			index(key, newVal);
		});
		
		entry.RegisterPostEvictionCallback((key, value, reason, state) => 
		{
			deindex(key);
		});
		
		return entry;
	}	
​
	void IDisposable.Dispose()
	{
		_internalCache.Dispose();
	}
​
	void IMemoryCacheInvalidator.Invalidate<TKey>(string aggregateName, TKey value)
	{
		if(_index.TryGetValue(aggregateName, out ConcurrentDictionary<object,object> subindex))
		{
			if(subindex.TryGetValue(value, out object idxKey))
			{
				_internalCache.Remove(idxKey);
			}
		}
	}
​
	void IMemoryCache.Remove(object key)
	{
		_internalCache.Remove(key);
	}
​
	bool IMemoryCache.TryGetValue(object key, out object value)
	{
		return _internalCache.TryGetValue(key, out value);
	}
	
	private void index(object key, object value)
	{		
		IEnumerable<dynamic> invKeys = _invalidationKeys.Where(key => key.GetType().GenericTypeArguments[0].IsAssignableTo(value.GetType()));
		
		foreach(var invKey in invKeys)
		{
			dynamic idxFunc = invKey.FKeySelector.Compile();
			
			dynamic idxVal = idxFunc((dynamic)value);
			
			string name = invKey.AggregateName;
			
			ConcurrentDictionary<object,object> subidx = _index.GetOrAdd(name, _ => new ConcurrentDictionary<object,object>());
			
			subidx[idxVal] = key;
		}
	}
​
	private void deindex(object key)
	{
		var lookup = _index.SelectMany(x => x.Value.Select(y => new {idx = x.Key, subidx = y.Key, key = y.Value})).Where(x => x.key == key);
		
		foreach(var entry in lookup)
		{
			if(_index.TryGetValue(entry.idx, out ConcurrentDictionary<object,object> subidx))
			{
				subidx.TryRemove(entry.subidx, out _);
			}
		}
	}
}
​
public abstract record InvalidationKey
{
	public string AggregateName {get; init;}
}
​
internal record InvalidationKey<TObj, TKey> : InvalidationKey 
{ 
	public Expression<Func<TObj,TKey>> FKeySelector { get; init; }
}
​
public static class InvalidationServiceCollectionExtensions
{
	public static IServiceCollection AddInvalidation<TCachable, TKey>(this IServiceCollection services, string aggregateName, Expression<Func<TCachable,TKey>> fKeySelector)
	{
		services.AddSingleton<InvalidationKey>(new InvalidationKey<TCachable, TKey> { AggregateName = aggregateName, FKeySelector = fKeySelector });
		
		return services;
	}
}
​
public class InvalidationChangeToken : IChangeToken
{
	public bool HasChanged => throw new NotImplementedException();
​
	public bool ActiveChangeCallbacks => throw new NotImplementedException();
​
	public IDisposable RegisterChangeCallback(Action<object> callback, object state)
	{
		throw new NotImplementedException();
	}
}
​
public class ChangeCallbackCacheEntry : ICacheEntry
{
	private readonly ICacheEntry _internalCacheEntry;
	private readonly Action<object,object> _callback;
	
	public ChangeCallbackCacheEntry(ICacheEntry cacheEntry, Action<object,object> valueChangeCallback)
	{
		_internalCacheEntry = cacheEntry;
		_callback = valueChangeCallback;
	}
	
	public object Key => _internalCacheEntry.Key;
​
	public object Value 
	{ 
		get => _internalCacheEntry.Value; 
		set => updateValue(_internalCacheEntry.Value, value);	
	}
	
	private void updateValue(object oldValue, object newValue)
	{
		_internalCacheEntry.Value = newValue;
		_callback.Invoke(oldValue, newValue);
	}
	
	public DateTimeOffset? AbsoluteExpiration { get => _internalCacheEntry.AbsoluteExpiration; set => _internalCacheEntry.AbsoluteExpiration = value; }
	
	public TimeSpan? AbsoluteExpirationRelativeToNow { get => _internalCacheEntry.AbsoluteExpirationRelativeToNow; set => _internalCacheEntry.AbsoluteExpirationRelativeToNow = value; }
	
	public TimeSpan? SlidingExpiration { get => _internalCacheEntry.SlidingExpiration; set => _internalCacheEntry.SlidingExpiration = value; }
​
	public IList<IChangeToken> ExpirationTokens => _internalCacheEntry.ExpirationTokens;
​
	public IList<PostEvictionCallbackRegistration> PostEvictionCallbacks => _internalCacheEntry.PostEvictionCallbacks;
​
	public CacheItemPriority Priority { get => _internalCacheEntry.Priority; set => _internalCacheEntry.Priority = value; }
	
	public long? Size { get => _internalCacheEntry.Size; set => _internalCacheEntry.Size = value; }
​
	public void Dispose()
	{
		_internalCacheEntry.Dispose();
	}
}