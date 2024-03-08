# villains-app


## Export API Specification from AWS API Gateway

```console
aws apigateway get-export --parameters extensions='apigateway' \
    --rest-api-id eujy7vsmkk \
    --stage-name live \
    --export-type swagger \
    doc/villains-gateway.json
```