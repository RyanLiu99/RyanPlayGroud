kubectl delete -f .\multi-pod.yml
kubectl delete -f .\nodePort.yml

kubectl apply -f .\multi-pod.yml
kubectl apply -f .\nodePort.yml

try { [System.Diagnostics.Process]::Start("http://localhost:30851/Index")  # PS 5 only
} catch { } #Ignore