kubectl create namespace chain-of-proxy-call
kubectl create deployment chain-of-proxy-call --image=chain-of-proxy-call:latest  -n chain-of-proxy-call --dry-run=client -o yaml > my-deployment.yaml
REM  cd..
REM kubectl rollout restart -n chain-of-proxy-call deployment chain-of-proxy-call
REM kubectl expose deployment chain-of-proxy-call --type=LoadBalancer --port=80 --target-port=8080 -n chain-of-proxy-call --dry-run=client -o yaml > my-service.yaml
REM kubectl delete -n chain-of-proxy-call deployment chain-of-proxy-call


REM  line continuation character in Windows batch files is caret (^),  in PS is `, bash is backslash (\)
kubectl expose deployment chain-of-proxy-call `
  --port=80 `
  --target-port=8080 `
  --type=ClusterIP `
  -n chain-of-proxy-call `
  --dry-run=client -o yaml > chain-of-proxy-call-service.yaml

kubectl apply -f chain-of-proxy-call-service.yaml -n chain-of-proxy-call


kubectl create ingress chain-of-proxy-call-ingress \
  --rule="chainofproxy.local/*=chain-of-proxy-call:80" \
  -n chain-of-proxy-call \
  --dry-run=client -o yaml > chain-of-proxy-call-ingress.yaml

kubectl apply -f chain-of-proxy-call-ingress.yaml -n chain-of-proxy-call


