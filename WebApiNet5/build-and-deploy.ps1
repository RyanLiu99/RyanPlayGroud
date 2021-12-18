$OldLocaton = Get-Location
Set-Location "$PSSCRIPTROOT\docker"
docker-compose stop
docker ps --format "{{.Image}}#{{.ID}}" -n 1 -q | Where-Object {$_ -Match "webapinet5"} | foreach-object {docker rm -f $_.split("#")[1]}  
# docker container rm -fv WebApiNet5
docker system prune -f
docker-compose up -d --build
try { [System.Diagnostics.Process]::Start("https://localhost:152/Index")  # PS 5 only
} catch { } #Ignore
Set-Location $OldLocaton
