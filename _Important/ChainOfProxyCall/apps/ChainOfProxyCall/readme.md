Notice, a container call anoteher container, is calling from inside the docker, cannot use thos host prot unless host network is used.
So test need do this:
    http://localhost:5301/?Url=http://chainofproxycall2:8080|http://chainofproxycall3:8080
Result will be 
     chainofproxycall-1 calling next. |  chainofproxycall-2 calling next. | End of the road from chainofproxycall


    http://localhost:5301/?Url=http://chainofproxycall2:8080|http://host.docker.internal:5303
    chainofproxycall-1 calling next. |  chainofproxycall-2 calling next. | End of the road from chainofproxycall-3

    http://localhost:5301/?Url=http://chainofproxycall2:8080|http://host.docker.internal:5303|http://chainofproxycall3:8080/
    chainofproxycall-1 calling next. |  chainofproxycall-2 calling next. |  chainofproxycall-3 calling next. | End of the road from chainofproxycall-3

Notce if calling within container, either use containerNaem:8080 or host.docker.internal:5303
Output has right name, not containerId, because docker-compose.yaml manually set hostName