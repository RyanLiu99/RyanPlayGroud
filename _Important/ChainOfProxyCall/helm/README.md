# Chain of Proxy Call - Helm Chart

This Helm chart deploys the Chain of Proxy Call application to Kubernetes.

## Prerequisites

- Kubernetes cluster (Minikube for local development)
- Helm 3.x installed
- kubectl configured to access your cluster
- Docker (for building images)

## Installation

### Quick Start with Minikube

For local development with Minikube:

```batch
# 1. Setup Minikube (first time only)
setup-minikube.bat

# 2. Deploy the application
deploy-local.bat

# 3. Add to your hosts file:
# Windows: C:\Windows\System32\drivers\etc\hosts
# Add: [MINIKUBE-IP] chainofproxy.local

# 4. Visit http://chainofproxy.local
```

### Generate Helm Chart (if needed)
If you want to generate a new Helm chart from scratch:
```bash
helm create chain-of-proxy-call
```

### Deploy to Local Environment (Minikube)
```batch
# Using the enhanced script (recommended)
deploy-local.bat

# Or manually
helm upgrade --install chain-of-proxy-call ./chain-of-proxy-call \
  -f ./chain-of-proxy-call/values-local.yaml \
  --namespace chain-of-proxy-call \
  --create-namespace
```

### Deploy to AWS Environment
```bash
# Using the provided script
./deploy-aws.sh

# Or manually
helm upgrade --install chain-of-proxy-call ./chain-of-proxy-call \
  -f ./chain-of-proxy-call/values-aws.yaml \
  --namespace chain-of-proxy-call \
  --create-namespace
```

### Deploy with Custom Values
```bash
helm upgrade --install chain-of-proxy-call ./chain-of-proxy-call \
  --set replicaCount=5 \
  --set image.tag=v2.0.0 \
  --namespace chain-of-proxy-call \
  --create-namespace
```

## Configuration

The following table lists the configurable parameters and their default values.

| Parameter | Description | Default |
|-----------|-------------|---------|
| `replicaCount` | Number of replicas | `3` |
| `image.repository` | Image repository | `chain-of-proxy-call` |
| `image.tag` | Image tag | `latest` |
| `image.pullPolicy` | Image pull policy | `Never` |
| `service.type` | Service type | `ClusterIP` |
| `service.port` | Service port | `80` |
| `service.targetPort` | Container port | `8080` |
| `ingress.enabled` | Enable ingress | `true` |
| `ingress.hosts[0].host` | Hostname | `chainofproxy.local` |
| `namespace.name` | Namespace name | `chain-of-proxy-call` |
| `environment` | Environment (local/aws) | `local` |

## Environment-Specific Configurations

### Local Development
- Uses local image with `pullPolicy: Never`
- Single replica for development
- ClusterIP service with Ingress
- Host: `chainofproxy.local`

### AWS Production
- Uses ECR image with `pullPolicy: Always`
- 3 replicas with auto-scaling enabled
- LoadBalancer service (no Ingress needed)
- Resource limits and requests configured

## Useful Commands

### Validate the chart
```bash
helm lint ./chain-of-proxy-call
```

### Dry run deployment
```bash
helm install chain-of-proxy-call ./chain-of-proxy-call --dry-run --debug
```

### Template rendering
```bash
helm template chain-of-proxy-call ./chain-of-proxy-call
```

### Check deployment status
```bash
helm status chain-of-proxy-call -n chain-of-proxy-call
```

### Uninstall
```bash
helm uninstall chain-of-proxy-call -n chain-of-proxy-call
```

### Get all releases
```bash
helm list -A
```

### Upgrade with new values
```bash
helm upgrade chain-of-proxy-call ./chain-of-proxy-call \
  -f ./chain-of-proxy-call/values-local.yaml \
  --namespace chain-of-proxy-call
```

## Development Workflow with Minikube

### Available Scripts

| Script | Description |
|--------|-------------|
| `setup-minikube.bat` | Initial Minikube setup with required addons |
| `deploy-local.bat` | Build, load, and deploy to Minikube |
| `cleanup-local.bat` | Clean up local deployment |
| `dev-helper.bat` | Development utilities (status, logs, scaling, etc.) |

### Development Commands

```batch
# Check deployment status
dev-helper.bat status

# View application logs
dev-helper.bat logs

# Port forward for direct access (bypass ingress)
dev-helper.bat port-forward

# Test the application
dev-helper.bat test

# Rebuild and redeploy after code changes
dev-helper.bat rebuild

# Scale the deployment
dev-helper.bat scale 3
```

### Accessing the Application

1. **Via Ingress (recommended)**:
   - Add `[MINIKUBE-IP] chainofproxy.local` to your hosts file
   - Visit: `http://chainofproxy.local`

2. **Via Port Forward**:
   - Run: `dev-helper.bat port-forward`
   - Visit: `http://localhost:8080`

3. **Via Minikube Service**:
   - Run: `minikube service chain-of-proxy-call -n chain-of-proxy-call`

### Testing Proxy Functionality

Test the proxy chain functionality:
```
# Single hop
http://chainofproxy.local/?url=http://chain-of-proxy-call

# Multi-hop (within cluster)
http://chainofproxy.local/?url=http://chain-of-proxy-call|http://chain-of-proxy-call

# Each refresh will hit different pods in the deployment
```

## Testing

After deployment, you can test the application:

### Local Testing
```bash
# Add to /etc/hosts (Linux/Mac) or C:\Windows\System32\drivers\etc\hosts (Windows)
127.0.0.1 chainofproxy.local

# Test the endpoint
curl http://chainofproxy.local/
```

### AWS Testing
```bash
# Get the LoadBalancer external IP
kubectl get svc -n chain-of-proxy-call

# Test the endpoint
curl http://<EXTERNAL-IP>/
```

## Chart Structure

```
helm/
├── chain-of-proxy-call/
│   ├── Chart.yaml              # Chart metadata
│   ├── values.yaml             # Default values
│   ├── values-local.yaml       # Local environment values
│   ├── values-aws.yaml         # AWS environment values
│   └── templates/
│       ├── _helpers.tpl        # Helper templates
│       ├── namespace.yaml      # Namespace resource
│       ├── serviceaccount.yaml # Service account
│       ├── deployment.yaml     # Deployment resource
│       ├── service.yaml        # Service resource
│       ├── ingress.yaml        # Ingress resource
│       └── hpa.yaml           # Horizontal Pod Autoscaler
├── deploy-local.sh             # Local deployment script
├── deploy-aws.sh               # AWS deployment script
├── deploy-local.bat            # Windows local deployment
├── deploy-aws.bat              # Windows AWS deployment
└── README.md                   # This file
```
