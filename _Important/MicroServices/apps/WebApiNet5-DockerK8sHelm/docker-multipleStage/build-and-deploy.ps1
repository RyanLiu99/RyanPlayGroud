function prune{
    # docker container prune 
    docker image prune 
    docker network prune 
}

$OldLocaton = Get-Location
Set-Location "$PSSCRIPTROOT\"
docker-compose stop
docker ps --format "{{.Image}}#{{.ID}}" -n 1 -q | Where-Object {$_ -Match "webapinet5"} | foreach-object {docker rm -f $_.split("#")[1]}  
# docker container rm -fv WebApiNet5
# prune don't do it, it will remove all containers and images, I don't always keep things running
docker-compose up -d --build
try { [System.Diagnostics.Process]::Start("https://localhost:152/Index")  # PS 5 only
} catch { } #Ignore
Set-Location $OldLocaton
# prune # Don't do it, it will remove all containers and images, I don't always keep things running
