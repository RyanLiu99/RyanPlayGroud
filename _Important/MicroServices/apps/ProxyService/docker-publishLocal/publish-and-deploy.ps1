function prune{
    # docker container prune 
    docker image prune 
    docker network prune 
}

$OldLocaton = Get-Location

Set-Location "$PSSCRIPTROOT\..\src\"
$version = '1.0.88' #can be parameter
dotnet build  /p:Version=$version  -c Release # --no-restore, it will restore as needed
dotnet publish -c Release -o ../_Publish # it will restore, build first if needed
New-Item -path '..\_Publish\Certs\' -ItemType Directory -Force
Copy-Item "$PSSCRIPTROOT\DockerfilePublish" -Destination '..\_Publish\'
Copy-Item '..\Certs\localhost-openssl.pfx' -Destination '..\_Publish\Certs\'

Set-Location "$PSSCRIPTROOT\"
docker-compose stop
$image = 'playground/proxyservice'

docker ps --format "{{.Image}}#{{.ID}}" -n 1 -q | Where-Object {$_ -Match $image} | foreach-object {docker rm -f $_.split("#")[1]}  
# docker container rm -fv playgroundProxyService
# prune  # don't do it, it will remove all containers and images, I don't always keep things running
docker-compose up -d --build # or run dockerize-another.directWay.ps1 to create image only
try { [System.Diagnostics.Process]::Start("https://localhost:352/")  # PS 5 only
} catch { } #Ignore
Set-Location $OldLocaton
#  prune   # Don't do it, it will remove all containers and images, I don't always keep things running
