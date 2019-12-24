## How to run deploy scripts

### Example
Deploying from version 0.12.0 to 0.13.0

```
ssh root@server_ip -i ~/.ssh/gitcom_key "bash -s" < ./scripts/deploy/prod/deploy.sh "0.12.0" "0.13.0"
```
