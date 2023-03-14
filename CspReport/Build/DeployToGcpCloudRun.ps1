# gcloud init
gcloud config set run/region us-west1

gcloud run deploy medriocspreport --image=us.gcr.io/ryantest1-4fd63/medriocspreport:rliu --project=ryantest1-4fd63 --set-env-vars=ASPNETCORE_ENVIRONMENT=Production

gcloud run services update-traffic medriocspreport --to-latest 
