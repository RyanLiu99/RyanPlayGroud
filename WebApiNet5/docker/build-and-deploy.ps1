function prune{
    docker container prune -f
    docker image prune -f
    docker network prune -f
}

$OldLocaton = Get-Location
Set-Location "$PSSCRIPTROOT\"
docker-compose stop
docker ps --format "{{.Image}}#{{.ID}}" -n 1 -q | Where-Object {$_ -Match "webapinet5"} | foreach-object {docker rm -f $_.split("#")[1]}  
# docker container rm -fv WebApiNet5
prune
docker-compose up -d --build
try { [System.Diagnostics.Process]::Start("https://localhost:152/Index")  # PS 5 only
} catch { } #Ignore
Set-Location $OldLocaton
prune
