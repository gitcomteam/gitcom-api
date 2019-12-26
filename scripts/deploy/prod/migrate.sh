#!/bin/bash
echo "PRODUCTION MIGRATING VERSION: $1"

docker pull registry.gitlab.com/mxss/gitcom-api/gitcom-migrations:"$1"
docker run -v /usr/local/gitcom/backend/migrations/phinx.yml:/usr/local/gitcom/migrations/phinx.yml --net=host registry.gitlab.com/mxss/gitcom-api/gitcom-migrations:"$1"

echo "MIGRATING VERSION $1 ON PRODUCTION FINISHED!"