#build docker image directly, without create any container (so nor setting related to create container)
#build publish dlls first
docker build -f DockerfilePublish -t webapinet5publish ..\_Publish  # -t for tag, but cannot use -tag. tag format is name[:tag]
docker tag webapinet5 ryan/webapinet5:1.1  # will appears as another entry in images list but with same image id
