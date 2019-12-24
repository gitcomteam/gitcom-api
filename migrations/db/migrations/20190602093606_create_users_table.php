<?php


use Phinx\Migration\AbstractMigration;

class CreateUsersTable extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('users');
        $table
            ->addColumn('login', 'string')
            ->addColumn('guid', 'string')
            ->addColumn('password', 'string')
            ->addColumn('email', 'string')
            ->addColumn('register_date', 'timestamp', ['default' => 'CURRENT_TIMESTAMP'])
            
            ->addIndex(['login'], ['unique' => true])
            ->addIndex(['email'], ['unique' => true])
            ->addIndex(['guid'], ['unique' => true])
            ->create();
    }
}
