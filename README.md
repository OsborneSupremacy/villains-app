# villains-app

## Deploy Lambdas

This isn't the best way, but it's what I'm going with for now.

```console
dotnet lambda deploy-function --project-location src/Villains.Lambda.Image.Get/src/Villains.Lambda.Image.Get --function-name villains-image-get
dotnet lambda deploy-function --project-location src/Villains.Lambda.Image.Upload/src/Villains.Lambda.Image.Upload --function-name villains-image-upload
dotnet lambda deploy-function --project-location src/Villains.Lambda.Create/src/Villains.Lambda.Villain.Create --function-name villain-create
dotnet lambda deploy-function --project-location src/Villains.Lambda.Villain.Get/src/Villains.Lambda.Villain.Get --function-name villain-get
dotnet lambda deploy-function --project-location src/Villains.Lambda.Villain.Update/src/Villains.Lambda.Villain.Update --function-name villain-update 
dotnet lambda deploy-function --project-location src/Villains.Lambda.Villains.Get/src/Villains.Lambda.Villains.Get --function-name villains-get
```

## Export API Specification from AWS API Gateway

```console
aws apigateway get-export --parameters extensions='apigateway' \
    --rest-api-id eujy7vsmkk \
    --stage-name live \
    --export-type swagger \
    doc/villains-gateway.json
```