# helm upgrade --install ingress-nginx ingress-nginx --repo https://kubernetes.github.io/ingress-nginx --namespace ingress-nginx --create-namespace --set controller.watchIngressWithoutClass=true

helm upgrade webapichartv2 . --install --create-namespace --namespace ryanplay

try { [System.Diagnostics.Process]::Start("http://webapichartv2.local.medrio.com/?q=afda")  # PS 5 only, admin mode
} catch { } #Ignore