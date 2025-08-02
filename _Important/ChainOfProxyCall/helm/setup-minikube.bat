@echo off
REM Setup Minikube for Chain of Proxy Call deployment

echo ========================================
echo Setting up Minikube for local development
echo ========================================

REM Start Minikube with sufficient resources
echo Starting Minikube...
minikube start --memory=4096 --cpus=2 --disk-size=20g

REM Enable necessary addons
echo Enabling Minikube addons...
minikube addons enable ingress
minikube addons enable dashboard
minikube addons enable metrics-server

REM Wait for ingress controller to be ready
echo Waiting for ingress controller to be ready...
kubectl wait --namespace ingress-nginx --for=condition=ready pod --selector=app.kubernetes.io/component=controller --timeout=90s

REM Build and load the application image
echo Building application image...
cd ..\apps\ChainOfProxyCall
call BuildImage.bat

echo Loading image into Minikube...
minikube image load chain-of-proxy-call:latest

cd ..\..\helm

echo ========================================
echo Minikube setup completed!
echo ========================================
echo.
echo Minikube IP:
minikube ip
echo.
echo Next steps:
echo 1. Run 'deploy-local.bat' to deploy the application
echo 2. Add the following to your hosts file:
echo    [MINIKUBE-IP]  chainofproxy.local
echo 3. Visit http://chainofproxy.local
echo.
echo Useful commands:
echo   minikube dashboard    - Open Kubernetes dashboard
echo   minikube status       - Check Minikube status
echo   minikube logs         - View Minikube logs
echo   minikube stop         - Stop Minikube
echo ========================================
