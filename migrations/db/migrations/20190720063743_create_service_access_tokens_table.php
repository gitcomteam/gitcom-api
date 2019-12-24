<?php


use Phinx\Migration\AbstractMigration;

class CreateServiceAccessTokensTable extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('service_access_tokens');
        $table
            ->addColumn('guid', 'string')
            ->addColumn('user_id', 'integer', ['signed' => false, 'null' => false])
            ->addColumn('access_token', 'string')
            ->addTimestamps()

            ->addForeignKey('user_id', 'users', 'id', ['delete'=> 'NO_ACTION', 'update'=> 'NO_ACTION'])
            ->create();

        $this->execute("CREATE TYPE ServiceType AS ENUM ('GitHub', 'GitLab', 'BitBucket');");

        $this->execute("ALTER TABLE service_access_tokens ADD COLUMN service_type ServiceType NOT NULL;");
    }
}
