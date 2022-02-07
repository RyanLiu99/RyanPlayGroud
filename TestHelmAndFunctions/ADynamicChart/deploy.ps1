# helm upgrade --install ingress-nginx ingress-nginx --repo https://kubernetes.github.io/ingress-nginx --namespace ingress-nginx --create-namespace --set controller.watchIngressWithoutClass=true

helm upgrade ARelease . --install --create-namespace --namespace ryantesthelm

