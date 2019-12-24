#!/bin/bash
echo "DEPLOYING VERSION $1 -> $2"

docker pull registry.gitlab.com/mxss/gitcom-api/gitcom-backend:"$2"

# stop running container
docker ps -q --filter ancestor="registry.gitlab.com/mxss/gitcom-api/gitcom-backend:$1" | xargs -r docker stop

# start new container
docker run -d -it --net=host --restart unless-stopped --volume=/usr/local/gitcom/backend/config:/usr/local/supporthub/config registry.gitlab.com/mxss/gitcom-api/gitcom-backend:"$2"

docker container ps

docker image prune -a --force --filter "until=24h"

docker image ls

df -h

free -m

docker images -q |xargs docker rmi -f

sleep 2

curl -I https://api.gitcom.org/version
curl -I https://api.gitcom.org/api/v1/projects/newest/get

echo "Deploy finished!"
