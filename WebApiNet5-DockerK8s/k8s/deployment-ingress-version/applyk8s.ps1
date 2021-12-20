kubectl apply -f WebApiNet5.yaml
kubectl apply -f WebApiNet5-service.yaml
kubectl apply -f WebApiNet5-ingress.yaml

try { [System.Diagnostics.Process]::Start("http://webapinet5.local.medrio.com/")  # PS 5 only
} catch { } #Ignore