docker-compose up --build --force-recreate -d

REM  then open http://localhost:5301/?Url=http://chainofproxycall2:8080|http://host.docker.internal:5303|http://chainofproxycall3:8080
REM Got: chainofproxycall-1 calling next. |  chainofproxycall-2 calling next. |  chainofproxycall-3 calling next. | End of the road from chainofproxycall-3