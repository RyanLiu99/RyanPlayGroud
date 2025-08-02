REM Deploy to AWS environment
helm upgrade --install chain-of-proxy-call ./chain-of-proxy-call -f ./chain-of-proxy-call/values-aws.yaml --namespace chain-of-proxy-call --create-namespace
