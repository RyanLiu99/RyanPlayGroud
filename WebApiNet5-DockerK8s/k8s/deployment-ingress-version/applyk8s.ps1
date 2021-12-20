kubectl apply -f WebApiNet5.yaml
kubectl apply -f WebApiNet5-service.yaml
kubectl apply -f WebApiNet5-ingress.yaml

try { [System.Diagnostics.Process]::Start("http://webapinet5.local.medrio.com/?q=afda")  # PS 5 only, admin mode
} catch { } #Ignore