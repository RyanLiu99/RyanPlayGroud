# Set up and run

> dotnet new webapi -o DictionaryinAsyncLocal

> dotnet new webapi -o AsyncLocalInDictionary


 > siege -c 200 -r 600  http://192.168.0.220:5252/    # dictionary in asyncLocal

 > siege -c 200 -r 600  http://192.168.0.220:5296/    # async local in dictonary
 
``` 
	 -c, --concurrent=NUM      CONCURRENT users, default is 10
	 -t, --time=NUMm           TIMED testing where "m" is modifier S, M, or H
								ex: --time=1H, one hour test.
	 -r, --reps=NUM              number of times to run the test.
	 -b, --benchmark           BENCHMARK: no delays between requests.
```
 
  > dotnet-counters monitor  --name DictionaryinAsyncLocal

  > dotnet-counters monitor  --name AsyncLocalInDictionary
  
  
  # Look at result:
  
  Speed (conslusion might change when even high load and GC kicked in)
    DictionaryinAsyncLocal (my version) is slightly slower than AsyncLocalInDictionary (original).  Guess creating a dictionary for each request takes a little time.

  
  ## But for memory and GC:
  
  - When key is static thus just small set:	Two approach performance are similar.
  - When key is dynamic thus lot of keys added overtime: DictionaryinAsyncLocal is way way better 	
  ![when key is dymamic, DictionaryinAsyncLocal is better on GC](./LargeDynamicKeySets2AppraocCompareDicInAsyncLocalIsMuchBetterForGC.png)
  
  
  ## Deep div to spcial case where key is dynamic (120K unique keys in dictionary )

  From ![](./OneGiantDictionary1InstanceButManyKeys.png) , for AsyncLocalIndictionary old approach you can see only one instance of ConcurrentDictionary<string, AsyncLocal<object>>, but this dictioary has 120,003 keys/entries. Take 21,448,872 bytes momeory. Avg  21448872/120003 = 179 Bytes for a KeyVaulePair<string, AsyncLocal<object>>.  *** 21M momory can NOT be released. ***  
 
 ```
 8 bytes for string key refence
 72 bytes for key guid string  or smaller size for other types
 72 bytes for key guid value
 24 bytes for AsyncLocal<T>
Total is 176 bytes 


  For new approach, which dictioanry in AsyncLocal, there are 3054 ConcurrentDictonary<strong, Object> left, (others has been GCed), total size is 97728 bytes.  Avg 97728/3054 = 32 Bytes for a ConcurrentDictionary, which has 4 KeyValuePair<string, object> in it. Each take 8 bytes, just a reference size.  
  
  Even those remaing 3054 instances, they are dead objects already, ready to be GCed.

```
sizeof(object*) which is size of a reference
8

sizeof(Guid)
16

a Guid string
36 characters * 2 bytes/character = 72 bytes

sizeof(DateTime)
8

sizeof(DateTime*)
8

```

