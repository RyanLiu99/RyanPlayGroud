#depends on DockerfilePublish, and replace docker-compose.yml but 
# still depends on all related files are in .Publish folde already which copied over by  publish-and-deploy.ps1
# this is just build image, not creating container, so all setting for container in docker compose not here either

#build docker image directly, without create any container (so nor setting related to create container)
#build publish dlls first
docker build -f DockerfilePublish -t webapinet5publish ..\_Publish  # -t for tag, but cannot use -tag. tag format is name[:tag]
docker tag webapinet5publish ryan/webapinet5:1.1  # will appears as another entry in images list but with same image id
