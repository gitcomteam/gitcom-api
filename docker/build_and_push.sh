#!/bin/bash
echo "Building and pushing docker image - version: $1"

echo "Publish build (.net core build)"
dotnet publish --configuration Release

echo "Building docker images - version: $1"
docker build -t registry.gitlab.com/mxss/gitcom-api/gitcom-backend:"$1" -f docker/app/Dockerfile .

echo "Pushing image to registy"
docker push registry.gitlab.com/mxss/gitcom-api/gitcom-backend:"$1"

echo "Done. version $1 is built and pushed."
