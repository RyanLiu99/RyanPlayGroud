This is simple proxy site just call next site in urls (spareated by |) in query string. Like call anotehr itself or another instance of same service running in different port or container.

There are batch files in apps folder to build image, docker-compse 3 instanances ( in 2 ways) . Like
http://localhost:5301/?Url=http://chainofproxycall2:8080|http://host.docker.internal:5303|http://chainofproxycall3:8080/


There is K8s folder, provide another way to deploy multipl instances to K8s.

After ingress is deployed, and 3 pods in deployment, go to  http://chainofproxy.local/, each time refresh, it will get different result sine serve from different pod.

Go to dashborad to get IP address of intenral pods, then can do this:
http://chainofproxy.local/?url=http://10.244.0.37:8080|http://10.244.0.36:8080  Get result:
 chain-of-proxy-call-55687fbdd5-dqhhk calling next. |  chain-of-proxy-call-55687fbdd5-dqhhk calling next. | End of the road from chain-of-proxy-call-55687fbdd5-nljmw

or 
   chain-of-proxy-call-55687fbdd5-6bqgp calling next. |  chain-of-proxy-call-55687fbdd5-dqhhk calling next. | End of the road from chain-of-proxy-call-55687fbdd5-nljmw


http://chainofproxy.local/?url=http://10.244.0.37:8080|http://chain-of-proxy-call-55687fbdd5-dqhhk:8080 will result 
 chain-of-proxy-call-55687fbdd5-nljmw calling next. |  chain-of-proxy-call-55687fbdd5-dqhhk calling next. | End of the road from chain-of-proxy-call-55687fbdd5-dqhhk

 since inside K8s, Pods can call each other.