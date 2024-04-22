dotnet new webapi -o DictionaryinAsyncLocal
dotnet new webapi -o AsyncLocalInDictionary



 siege -c 200 -r 600  http://192.168.0.220:5252/    # dictionary in asyncLocal
 siege -c 200 -r 600  http://192.168.0.220:5296/    # async local in dictonary
 
 
 -c, --concurrent=NUM      CONCURRENT users, default is 10
 -t, --time=NUMm           TIMED testing where "m" is modifier S, M, or H
                            ex: --time=1H, one hour test.
 -r, --reps=NUM              number of times to run the test.
 -b, --benchmark           BENCHMARK: no delays between requests.
 
 
  dotnet-counters monitor  --name DictionaryinAsyncLocal
  dotnet-counters monitor  --name AsyncLocalInDictionary
  
  ==========
  Look at result:
  
  Speed (conslustio might change when even high load and GC kicked in)
   DictionaryinAsyncLocal is slight slower then AsyncLocalInDictionary, guess creating asyncLocal is more time consuming than creating dictionary,
  
  But for memory and GC:
  
  - When key is dynamic thus lot of keys added overtime: DictionaryinAsyncLocal is way way better 	
  - When key is static thus just small set:	Two approach performance are similar.