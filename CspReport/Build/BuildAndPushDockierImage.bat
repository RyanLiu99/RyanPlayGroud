docker build -f "C:\gitHub\RyanPlayGroud\CspReport\Medrio.CspReport\Dockerfile" --force-rm -t medriocspreport:rliu "C:\gitHub\RyanPlayGroud\CspReport\" --add-host=nuget.medrio.org:172.16.100.42

gcloud auth configure-docker
docker tag medriocspreport:rliu  us.gcr.io/ryantest1-4fd63/medriocspreport:rliu
docker push  us.gcr.io/ryantest1-4fd63/medriocspreport:rliu
