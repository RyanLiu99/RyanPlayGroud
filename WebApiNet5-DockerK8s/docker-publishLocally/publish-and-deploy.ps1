function prune{
    docker container prune -f
    docker image prune -f
    docker network prune -f
}

$OldLocaton = Get-Location

Set-Location "$PSSCRIPTROOT\..\src\"
$version = '1.0.88' #can be parameter
dotnet build  /p:Version=$version  -c Release # --no-restore, it will restore as needed
dotnet publish -c Release -o ../Publish # it will restore, build first if needed
Copy-Item '..\docker-publishLocally\DockerfilePublish' -Destination '..\Publish\'
Copy-Item '..\Certs\localhost-openssl.pfx' -Destination '..\Publish\Certs\'

Set-Location "$PSSCRIPTROOT\"
docker-compose stop
docker ps --format "{{.Image}}#{{.ID}}" -n 1 -q | Where-Object {$_ -Match "webapinet5"} | foreach-object {docker rm -f $_.split("#")[1]}  
# docker container rm -fv WebApiNet5
prune
docker-compose up -d --build # or run dockerize-another.directWay.ps1 to create image only
try { [System.Diagnostics.Process]::Start("https://localhost:252/Index")  # PS 5 only
} catch { } #Ignore
Set-Location $OldLocaton
prune
