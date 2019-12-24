#!/bin/bash

echo "Starting container version: $1 ..."

sudo docker run -it -d --net=host registry.gitlab.com/openhubteam/openhub-api/supporthub:"$1"

echo "Container started"