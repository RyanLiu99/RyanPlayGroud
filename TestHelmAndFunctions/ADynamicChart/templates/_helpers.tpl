{{- define "app.fullname" -}}
{{- if .Values.fullnameOverride -}}
{{- .Values.fullnameOverride | trunc 63 | trimSuffix "-" -}}
{{- else -}}
{{- printf "%s-%s" .Chart.Name .Release.Name| trunc 63 | trimSuffix "-" -}}
{{- end -}}
{{- end -}}


{{- define "app.testfail" -}}
{{- fail "failed"   -}}
{{- end -}}

{{- define "testTernary" -}}
{{- ternary "true value" "empty value" true   -}}
{{- end -}}
