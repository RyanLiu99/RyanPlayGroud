function prune{
    docker container prune -f
    docker image prune -f
    docker network prune -f
}

$OldLocaton = Get-Location
Set-Location "$PSSCRIPTROOT\..\" #build above \src\ to access docker file in this folder
docker build -t frontend -f docker\Dockerfile .  #docker file is relative to build context
docker run -d -p 808:80 frontend

try { [System.Diagnostics.Process]::Start("http://localhost:808")  # PS 5 only
} catch { } #Ignore

Set-Location $OldLocaton
prune