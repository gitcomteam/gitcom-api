<?php


use Phinx\Migration\AbstractMigration;

class AlterProjectsTableUpdateRepositoryId extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('projects')
            ->addForeignKey('repository_id', 'repositories', 'id', ['delete'=> 'NO_ACTION', 'update'=> 'NO_ACTION'])
            ->update();
    }
}
