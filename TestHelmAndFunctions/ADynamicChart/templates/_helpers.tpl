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


{{/*
Recursively expands a configuration value using Values in the provided context
*/}}
{{- define "util.expand" -}}
{{- $context := (index . 0) -}}
{{- $context := $context.Values | mergeOverwrite (pick $context "Release" "Chart" "Files" "Capabilities") -}}
{{- $res := dict -}}
{{- range rest . -}}
  {{- if and (or . (kindOf . | regexMatch "string|int|float|bool")) (not $res.done) -}} {{/* falsy scalars are considered a valid value as well */}}
    {{- $_ := set $res "value" (include "medrio.util.expand.direct" (tuple $context .)) -}}
    {{- $_ := set $res "done" true -}}
  {{- end -}}
{{- end -}}
{{- $res.value | default "" -}}
{{- end -}}

{{- define "dotisdollar" -}}
{{  ternary "true" "false" (deepEqual  . $) }}{{  ternary "true" "false" (deepEqual  $ .) }}  #must be on same line to be valid in yaml
{{-  end -}}