FROM php:7.3

RUN apt-get update && apt-get install -y libpq-dev
RUN docker-php-ext-install pdo_pgsql

COPY migrations /usr/local/gitcom/migrations

WORKDIR /usr/local/gitcom/migrations

ENTRYPOINT ["php", "vendor/bin/phinx", "migrate"];
