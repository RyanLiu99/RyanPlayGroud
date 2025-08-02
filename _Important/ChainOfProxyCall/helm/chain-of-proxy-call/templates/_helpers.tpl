{{/*
Expand the name of the chart.
*/}}
{{- define "chain-of-proxy-call.name" -}}
{{- default .Chart.Name .Values.nameOverride | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Create a default fully qualified app name.
We truncate at 63 chars because some Kubernetes name fields are limited to this (by the DNS naming spec).
If release name contains chart name it will be used as a full name.
*/}}
{{- define "chain-of-proxy-call.fullname" -}}
{{- if .Values.fullnameOverride }}
{{- .Values.fullnameOverride | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- $name := default .Chart.Name .Values.nameOverride }}
{{- if contains $name .Release.Name }}
{{- .Release.Name | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- printf "%s-%s" .Release.Name $name | trunc 63 | trimSuffix "-" }}
{{- end }}
{{- end }}
{{- end }}

{{/*
Create chart name and version as used by the chart label.
*/}}
{{- define "chain-of-proxy-call.chart" -}}
{{- printf "%s-%s" .Chart.Name .Chart.Version | replace "+" "_" | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Common labels
*/}}
{{- define "chain-of-proxy-call.labels" -}}
helm.sh/chart: {{ include "chain-of-proxy-call.chart" . }}
{{ include "chain-of-proxy-call.selectorLabels" . }}
{{- if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{- end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end }}

{{/*
Selector labels
*/}}
{{- define "chain-of-proxy-call.selectorLabels" -}}
app.kubernetes.io/name: {{ include "chain-of-proxy-call.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
{{- end }}

{{/*
Create the name of the service account to use
*/}}
{{- define "chain-of-proxy-call.serviceAccountName" -}}
{{- if .Values.serviceAccount.create }}
{{- default (include "chain-of-proxy-call.fullname" .) .Values.serviceAccount.name }}
{{- else }}
{{- default "default" .Values.serviceAccount.name }}
{{- end }}
{{- end }}

{{/*
Get the image repository based on environment
*/}}
{{- define "chain-of-proxy-call.image" -}}
{{- if eq .Values.environment "aws" }}
{{- printf "%s:%s" .Values.aws.image.repository .Values.aws.image.tag }}
{{- else }}
{{- printf "%s:%s" .Values.image.repository .Values.image.tag }}
{{- end }}
{{- end }}

{{/*
Get the image pull policy based on environment
*/}}
{{- define "chain-of-proxy-call.imagePullPolicy" -}}
{{- if eq .Values.environment "aws" }}
{{- .Values.aws.image.pullPolicy }}
{{- else }}
{{- .Values.image.pullPolicy }}
{{- end }}
{{- end }}
