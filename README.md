# GitCom API
<a href="https://circleci.com/gh/gitcomteam/gitcom-api">
  <img src="https://img.shields.io/circleci/build/github/gitcomteam/gitcom-api/master" alt="chat on Discord">
</a>
<a href="https://discord.gg/gRxPXPn">
  <img src="https://img.shields.io/discord/658128774679756820?logo=discord" alt="chat on Discord">
</a>

### Description:
This is the main API of the [GitCom platform](https://start.gitcom.org) available at [api.gitcom.org](https://api.gitcom.org)

### Tech stack: :hammer_and_wrench:
Project is built using .NET Core and it's based on [Micron framework](https://github.com/mxss/micron)

Database: PostgreSQL  
ORM: [Dapper](https://github.com/StackExchange/Dapper)  
Migrations: [Phinx](https://github.com/cakephp/phinx)  
Webserver: [NancyFX](https://github.com/NancyFx/Nancy)

## API Documentation
Check out `swagger.json` file inside repo or view it here: [swagger preview](https://generator.swagger.io/?url=https://raw.githubusercontent.com/gitcomteam/gitcom-api/master/swagger.json)

## Requirements:
1. .NET Core 2.2
2. [composer](https://getcomposer.org/) (PHP dependency manager) - used for migrations

### Set up to develop locally:
1. restore nuget packages  
2. build project  
3. copy config.example.json into:  
**For main app**  
App/bin/Debug/netcoreapp2.2/config/config.json  
**For unit tests**  
Tests/bin/Debug/netcoreapp2.2/config/config.json  

4. edit config files - fill database name / user / etc.
5. copy migrations/phinx.example.yml to migrations/phinx.yml
6. edit phinx.yml - fill database user / password etc.
7. install php & composer dependencies from migrations/composer.json via `composer install`
8. run migrations (in migrations folder):

`php vendor/bin/phinx migrate` - to migrate with default database (development)

`php vendor/bin/phinx migrate -e testing` - to migrate with test database (used for testing)

9. build and run App.dll inside `App/bin/Debug/netcoreapp2.2/bin`

### Contribution:
Thank you for considering contributing to this repo, feel free to open a PR with any improvement, feature or bugfix.  
All bugfixes should go into `release/patch` branch  
All new features should go into `release/minor` branch  
All breaking changes should be in `release/major` branch  
 
### Contribution rewards
For each merged PR you will be rewarded with `contributor` badge and 2500 (or more) GitCom tokens which are tradeable on [Waves Decentralized exchange](https://waves.exchange/dex-demo?assetId2=BkuYDLDunSy7dvep7NgQcmiY4iyqTq3diHwdGPrFUCMC&assetId1=WAVES) (You will need to have a Waves wallet to be able to receive tokens)
