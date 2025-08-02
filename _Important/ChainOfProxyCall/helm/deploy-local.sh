# Deploy to local environment
helm upgrade --install chain-of-proxy-call ./chain-of-proxy-call -f ./chain-of-proxy-call/values-local.yaml --namespace chain-of-proxy-call --create-namespace
