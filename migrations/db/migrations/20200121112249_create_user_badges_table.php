<?php


use Phinx\Migration\AbstractMigration;

class CreateUserBadgesTable extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('user_badges');
        $table
            ->addColumn('guid', 'string')
            ->addColumn('user_id', 'integer', ['signed' => false, 'null' => false])
            ->addColumn('badge', 'string')

            ->addForeignKey('user_id', 'users', 'id', ['delete'=> 'CASCADE', 'update'=> 'CASCADE'])
            
            ->addIndex(['guid'], ['unique' => true])

            ->create();
    }
}
