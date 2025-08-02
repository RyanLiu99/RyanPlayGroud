@echo off
REM Cleanup local deployment

echo ========================================
echo Cleaning up local deployment
echo ========================================

REM Uninstall Helm release
echo Uninstalling Helm release...
helm uninstall chain-of-proxy-call -n chain-of-proxy-call

REM Delete namespace
echo Deleting namespace...
kubectl delete namespace chain-of-proxy-call

REM Remove image from Minikube
echo Removing image from Minikube...
minikube image rm chain-of-proxy-call:latest

echo ========================================
echo Cleanup completed!
echo ========================================
echo.
echo To completely reset Minikube:
echo   minikube delete
echo   minikube start
echo ========================================
