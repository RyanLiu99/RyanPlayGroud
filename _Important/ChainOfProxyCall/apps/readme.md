This is simple proxy site just call next site in urls (spareated by |) in query string. Like call anotehr itself or another instance of same service running in different port or container.

There are batch files in apps folder to build image, docker-compse 3 instanances ( in 2 ways) . Like
http://localhost:5301/?Url=http://chainofproxycall2:8080|http://host.docker.internal:5303|http://chainofproxycall3:8080/


There is K8s folder, provide another way to deploy multipl instances to K8s.
