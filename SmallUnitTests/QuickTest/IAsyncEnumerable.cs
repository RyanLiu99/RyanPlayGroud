//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;


//public interface IAsyncEnumerable<T>
//{
//    IAsyncEnumerator<T> GetAsyncEnumerator();
//}

//public interface IAsdAsyncEnumerator<T> : IDisposable
//{
//    T Current { get; }
//    Task<bool> MoveNextAsync();
//}

//public class SyncToAsyncEnumerable<TInput, TOutput> : IAsyncEnumerable<TOutput>
//{
//    private IEnumerable<TInput> sourceEnumerable;

//    private Func<TInput, Task<TOutput>> mapperFunction;

//    public SyncToAsyncEnumerable(IEnumerable<TInput> sourceEnumerable, Func<TInput, Task<TOutput>> mapperFunction)
//    {
//        _sourceEnumerable = sourceEnumerable;
//        _mapperFunction = mapperFunction;
//    }

//    public IAsyncEnumerator<TOutput> GetAsyncEnumerator()
//    {
//        return new SyncToAsyncEnumerator<TInput, TOutput>(_sourceEnumerable.GetEnumerator(), _mapperFunction);
//    }
//}

//public class SyncToAsyncEnumerator<TInput, TOutput> : IAsyncEnumerator<TOutput>
//{
//    private IEnumerator<TInput> _sourceEnumerator;
//    private Func<TInput, Task<TOutput>> _mapperFunction;

//    public SyncToAsyncEnumerator(IEnumerator<TInput> sourceEnumerator, Func<TInput, Task<TOutput>> mapperFunction)
//    {
//        _sourceEnumerator = sourceEnumerator;
//        _mapperFunction = mapperFunction;
//    }

//    public TOutput Current { get; private set; }

//    public Dispose()
//    {
//        _sourceEnumerator.Dispose();
//    }

//    public async Task<bool> MoveNextAsync()
//    {
//        if (!_sourceEnumerator.MoveNext())
//        {
//            return false;
//        }

//        Current = await _mapperFunction(_sourceEnumerator.Current);
//        return true;
//    }
//}


//// how to create
//return new SyncToAsyncEnumerable<TInput, TOutput>(regularEnumerable, TInput input => { do something; return Task<TOutput>});

//// how to consume
//var enumerator = asyncEnumerable.GetAsyncEnumerator();
//try
//{
//    while (await enumerator.MoveNextAsync())
//    {
//        var item = enumerator.Current;
//        // do something with item
//    }
//}
//finally
//{
//    enumerator.Dispose();
//}

