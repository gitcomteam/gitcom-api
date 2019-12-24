<?php


use Phinx\Migration\AbstractMigration;

class CreateExternalServicesDataTable extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('external_services_data');
        $table
            ->addColumn('guid', 'string')
            ->addColumn('user_id', 'integer', ['signed' => false, 'null' => false])
            ->addColumn('origin_id', 'string')
            ->addColumn('login', 'string')
            ->addTimestamps()

            ->addForeignKey('user_id', 'users', 'id', ['delete'=> 'NO_ACTION', 'update'=> 'NO_ACTION'])
            ->create();

        $this->execute("ALTER TABLE external_services_data ADD COLUMN service_type ServiceType NOT NULL;");
    }
}
