@echo off
REM Deploy to local environment (Minikube)

REM Enable ingress addon in Minikube if not already enabled
echo Enabling Minikube ingress addon...
minikube addons enable ingress

REM Build and load the Docker image into Minikube
echo Building and loading image into Minikube...
cd ..\apps\ChainOfProxyCall
call BuildImage.bat
minikube image load chain-of-proxy-call:latest
cd ..\..\helm

REM Deploy using Helm
echo Deploying with Helm...
helm upgrade --install chain-of-proxy-call ./chain-of-proxy-call -f ./chain-of-proxy-call/values-local.yaml --namespace chain-of-proxy-call --create-namespace

REM Add hosts entry information
echo.
echo ========================================
echo Deployment completed!
echo ========================================
echo.
echo To access the application, add this line to your hosts file:
echo.
echo For Windows: C:\Windows\System32\drivers\etc\hosts
echo For Linux/Mac: /etc/hosts
echo.
minikube ip
echo     chainofproxy.local
echo.
echo Then visit: http://chainofproxy.local
echo.
echo Useful commands:
echo   kubectl get pods -n chain-of-proxy-call
echo   kubectl get svc -n chain-of-proxy-call  
echo   kubectl get ingress -n chain-of-proxy-call
echo   minikube service list
echo ========================================
