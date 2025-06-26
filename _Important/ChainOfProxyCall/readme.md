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


 Since service endpont is reliable and pod is not, this is best way to test when ingress is used
 http://chainofproxy.local/?url=http://chain-of-proxy-call|http://chain-of-proxy-call, each time refresh, result changes.
 Note you cannot reeach http://chain-of-proxy-call/ directly, since it is serveir endpoint and it is internal
 You must use ingress url http://chainofproxy.local

Ryan Google doc
 - https://docs.google.com/document/d/1_Pid87ljniuvT8V92al1d0ntAKB4-WuEOrH2KACsnjw/edit?tab=t.0
 - https://docs.google.com/document/d/1TeZatK8QWg-aRHIFvNE__dVZg4G_J7u1vWJAfxttS6s/edit?tab=t.0
 - https://docs.google.com/document/d/1qBlOSFWWIRtiKbYOZwGTXWX5IZcFz8Hdvi8Ku2-F7Aw/edit?tab=t.0