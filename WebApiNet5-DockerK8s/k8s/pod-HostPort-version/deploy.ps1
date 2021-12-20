kubectl delete -f .\multi-pod-HostPort.yml
kubectl apply -f .\multi-pod-HostPort.yml


try { [System.Diagnostics.Process]::Start("http://localhost:1852/")  # PS 5 only
} catch { } #Ignore