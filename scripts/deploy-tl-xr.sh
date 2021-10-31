#!/bin/bash
dotnet lambda package \
    --output packaged/tl-xr.zip \
    --project-location src/TL.XR \
    --msbuild-parameters "/p:PublishReadyToRun=true --self-contained false"

dotnet lambda deploy-serverless \
    --package packaged/tl-xr.zip \
    --region $AWS_DEPLOY_REGION \
    --aws-access-key-id $AWS_DEPLOY_ACCESS_KEY \
    --aws-secret-key $AWS_DEPLOY_ACCESS_SECRET \
    --template src/TL.XR/serverless.template.json \
    --stack-name tl-xr-stack \
    --s3-bucket cyberdisco-deploy-$AWS_DEPLOY_REGION