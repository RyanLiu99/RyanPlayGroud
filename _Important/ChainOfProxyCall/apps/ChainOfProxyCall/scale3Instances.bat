docker-compose up --scale chainofproxycall=3   

REM  probably with above approach, we can't set container name for each instance.

REM  then open http://localhost:52905/?Url=http://bb9b3fda6430:8080|http://host.docker.internal:52903
REM Result will be End of the road from 3283c6a191d7 | bb9b3fda6430 forwarding. | 88d21e3cfa8c forwarding.