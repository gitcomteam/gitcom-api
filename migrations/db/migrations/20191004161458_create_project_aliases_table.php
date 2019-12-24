<?php


use Phinx\Migration\AbstractMigration;

class CreateProjectAliasesTable extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('project_aliases');
        $table
            ->addColumn('guid', 'string')

            ->addColumn('owner', 'string')
            ->addColumn('alias', 'string')

            ->addColumn('project_id', 'integer', ['signed' => false, 'null' => false])
            ->addForeignKey('project_id', 'projects', 'id', ['delete'=> 'NO_ACTION', 'update'=> 'NO_ACTION'])

            ->addTimestamps()

            ->create();
    }
}
