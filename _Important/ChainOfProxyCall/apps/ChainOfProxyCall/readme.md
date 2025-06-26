
#  Test

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


# Deployment
There are multiple ways
- 2 way to do Docker-Compose in apps, 
- Local K8s using Minikube using ingesss
- AWS EKS

There is apps/ChainOfProxyCall/BuildImage.bat
THere is createK8sYamls.bat.txt, it is used by both Local MiniKube and Aws EKS.  Just change ingress to load balancer.

How to deploy to EKS step by step is here: https://docs.google.com/document/d/1_Pid87ljniuvT8V92al1d0ntAKB4-WuEOrH2KACsnjw/edit?tab=t.0#heading=h.hom79xgvbjj7