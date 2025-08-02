# Setup Minikube for Chain of Proxy Call deployment
# PowerShell version

Write-Host "========================================" -ForegroundColor Green
Write-Host "Setting up Minikube for local development" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green

# Start Minikube with sufficient resources
Write-Host "Starting Minikube..." -ForegroundColor Yellow
minikube start --memory=4096 --cpus=2 --disk-size=20g

# Enable necessary addons
Write-Host "Enabling Minikube addons..." -ForegroundColor Yellow
minikube addons enable ingress
minikube addons enable dashboard
minikube addons enable metrics-server

# Wait for ingress controller to be ready
Write-Host "Waiting for ingress controller to be ready..." -ForegroundColor Yellow
kubectl wait --namespace ingress-nginx --for=condition=ready pod --selector=app.kubernetes.io/component=controller --timeout=90s

# Build and load the application image
Write-Host "Building application image..." -ForegroundColor Yellow
Set-Location "..\apps\ChainOfProxyCall"
& ".\BuildImage.bat"

Write-Host "Loading image into Minikube..." -ForegroundColor Yellow
minikube image load chain-of-proxy-call:latest

Set-Location "..\..\helm"

Write-Host "========================================" -ForegroundColor Green
Write-Host "Minikube setup completed!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""

Write-Host "Minikube IP:" -ForegroundColor Cyan
$minikubeIP = minikube ip
Write-Host $minikubeIP -ForegroundColor White

Write-Host ""
Write-Host "Next steps:" -ForegroundColor Yellow
Write-Host "1. Run 'deploy-local.bat' to deploy the application" -ForegroundColor White
Write-Host "2. Add the following to your hosts file:" -ForegroundColor White
Write-Host "   C:\Windows\System32\drivers\etc\hosts" -ForegroundColor Gray
Write-Host "   $minikubeIP  chainofproxy.local" -ForegroundColor White
Write-Host "3. Visit http://chainofproxy.local" -ForegroundColor White
Write-Host ""

Write-Host "Useful commands:" -ForegroundColor Yellow
Write-Host "  minikube dashboard    - Open Kubernetes dashboard" -ForegroundColor White
Write-Host "  minikube status       - Check Minikube status" -ForegroundColor White
Write-Host "  minikube logs         - View Minikube logs" -ForegroundColor White
Write-Host "  minikube stop         - Stop Minikube" -ForegroundColor White
Write-Host "========================================" -ForegroundColor Green
