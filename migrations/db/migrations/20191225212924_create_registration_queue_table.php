<?php


use Phinx\Migration\AbstractMigration;

class CreateRegistrationQueueTable extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('registration_queue');
        $table
            ->addColumn('guid', 'string')
            ->addColumn('user_id', 'integer', ['signed' => false, 'null' => false])
            ->addColumn('email_confirmed', 'boolean', ['default' => false])
            ->addColumn('confirmation_key', 'string')

            ->addForeignKey('user_id', 'users', 'id', ['delete'=> 'CASCADE', 'update'=> 'CASCADE'])
            
            ->addIndex(['guid'], ['unique' => true])
            ->addIndex(['user_id'], ['unique' => true])

            ->create();
    }
}
