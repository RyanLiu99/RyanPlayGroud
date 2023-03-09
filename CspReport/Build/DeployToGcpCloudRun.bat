gcloud run deploy medriocspreport2 --image=us.gcr.io/ryantest1-4fd63/medriocspreport:rliu --region=us-west1 --project=ryantest1-4fd63 

gcloud run services update-traffic medriocspreport2 --to-latest 
