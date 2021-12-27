function prune{
    docker container prune -f
    docker image prune -f
    docker network prune -f
}

$OldLocaton = Get-Location
$imageName = 'webapinet5/frontend'
$containerName = 'webapifrontend'
$exposePort=808

Set-Location "$PSSCRIPTROOT\..\" #build above \src\ to access docker file in this folder

docker build -t $imageName -f docker\Dockerfile .  #docker file is relative to build context

#delete old container
docker ps --format "{{.Image}}#{{.ID}}" -n 1 -q | Where-Object {$_ -Match $containerName} | foreach-object {docker rm -f $_.split("#")[1]}  

docker run -it -d -p 808:80 --restart=unless-stopped --name=$containerName $imageName 

try { [System.Diagnostics.Process]::Start("http://localhost:$exposePort")  # PS 5 only
} catch { } #Ignore

Set-Location $OldLocaton
prune