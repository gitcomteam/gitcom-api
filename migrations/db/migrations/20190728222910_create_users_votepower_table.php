<?php


use Phinx\Migration\AbstractMigration;

class CreateUsersVotepowerTable extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('users_votepower');
        $table
            ->addColumn('guid', 'string')
            ->addColumn('user_id', 'integer', ['signed' => false, 'null' => false])
            ->addColumn('project_id', 'integer', ['signed' => false, 'null' => false])
            ->addColumn('votepower', 'integer')
            ->addTimestamps()

            ->addForeignKey('user_id', 'users', 'id', ['delete'=> 'NO_ACTION', 'update'=> 'NO_ACTION'])
            ->addForeignKey('project_id', 'projects', 'id', ['delete'=> 'NO_ACTION', 'update'=> 'NO_ACTION'])
            ->create();
    }
}
