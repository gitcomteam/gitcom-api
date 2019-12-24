<?php


use Phinx\Migration\AbstractMigration;

class CreateUserSettingsTable extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('user_settings');
        $table
            ->addColumn('guid', 'string')
            ->addColumn('user_id', 'integer', ['signed' => false, 'null' => false])
            
            ->addColumn('key', 'string')
            ->addColumn('value', 'string')
            ->addTimestamps()

            ->addForeignKey('user_id', 'users', 'id', ['delete'=> 'NO_ACTION', 'update'=> 'NO_ACTION'])
            
            ->create();
    }
}
