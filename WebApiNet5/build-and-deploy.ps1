$OldLocaton = Get-Location
Set-Location "$PSSCRIPTROOT\docker"
docker-compose stop
docker ps --format "{{.Image}}#{{.ID}}" -n 1 -q | where {$_ -Match "webapinet5"} | foreach-object {docker rm -f $_.split("#")[1]}
docker rm -fv WebApiNet5
docker system prune -f
docker-compose up -d --build
[System.Diagnostics.Process]::Start("https://localhost:12777/Index")
Set-Location $OldLocaton
