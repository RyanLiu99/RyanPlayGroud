@echo off
REM Development helper for Chain of Proxy Call

if "%1"=="" goto :help
if "%1"=="help" goto :help
if "%1"=="status" goto :status
if "%1"=="logs" goto :logs
if "%1"=="port-forward" goto :port-forward
if "%1"=="test" goto :test
if "%1"=="rebuild" goto :rebuild
if "%1"=="scale" goto :scale
goto :help

:help
echo ========================================
echo Chain of Proxy Call - Development Helper
echo ========================================
echo.
echo Usage: dev-helper.bat [command]
echo.
echo Commands:
echo   status        - Show deployment status
echo   logs          - Show application logs
echo   port-forward  - Set up port forwarding (bypass ingress)
echo   test          - Test the application endpoints
echo   rebuild       - Rebuild and redeploy the application
echo   scale [n]     - Scale to n replicas
echo   help          - Show this help
echo.
goto :end

:status
echo ========================================
echo Deployment Status
echo ========================================
kubectl get pods -n chain-of-proxy-call
echo.
kubectl get svc -n chain-of-proxy-call
echo.
kubectl get ingress -n chain-of-proxy-call
echo.
helm status chain-of-proxy-call -n chain-of-proxy-call
goto :end

:logs
echo ========================================
echo Application Logs
echo ========================================
kubectl logs -f -l app.kubernetes.io/name=chain-of-proxy-call -n chain-of-proxy-call
goto :end

:port-forward
echo ========================================
echo Setting up port forwarding
echo ========================================
echo Application will be available at: http://localhost:8080
kubectl port-forward svc/chain-of-proxy-call 8080:80 -n chain-of-proxy-call
goto :end

:test
echo ========================================
echo Testing Application
echo ========================================
echo Testing direct access via port-forward...
echo Starting port-forward in background...
start /B kubectl port-forward svc/chain-of-proxy-call 8081:80 -n chain-of-proxy-call
timeout /t 3 >nul
curl http://localhost:8081/ 2>nul
if %errorlevel% equ 0 (
    echo ✓ Application is responding on port-forward
) else (
    echo ✗ Application not responding on port-forward
)
taskkill /f /im kubectl.exe >nul 2>&1
echo.
echo Testing ingress (if chainofproxy.local is configured in hosts)...
curl http://chainofproxy.local/ 2>nul
if %errorlevel% equ 0 (
    echo ✓ Application is responding via ingress
) else (
    echo ✗ Application not responding via ingress (check hosts file)
)
goto :end

:rebuild
echo ========================================
echo Rebuilding and Redeploying
echo ========================================
cd ..\apps\ChainOfProxyCall
call BuildImage.bat
minikube image load chain-of-proxy-call:latest
cd ..\..\helm
helm upgrade chain-of-proxy-call ./chain-of-proxy-call -f ./chain-of-proxy-call/values-local.yaml --namespace chain-of-proxy-call
kubectl rollout restart deployment/chain-of-proxy-call -n chain-of-proxy-call
echo Waiting for rollout to complete...
kubectl rollout status deployment/chain-of-proxy-call -n chain-of-proxy-call
goto :end

:scale
if "%2"=="" (
    echo Usage: dev-helper.bat scale [number]
    goto :end
)
echo ========================================
echo Scaling to %2 replicas
echo ========================================
kubectl scale deployment chain-of-proxy-call --replicas=%2 -n chain-of-proxy-call
kubectl rollout status deployment/chain-of-proxy-call -n chain-of-proxy-call
goto :end

:end
