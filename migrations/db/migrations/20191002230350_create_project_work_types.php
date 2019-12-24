<?php

use Phinx\Migration\AbstractMigration;
use Phinx\Db\Adapter\PostgresAdapter;

class CreateProjectWorkTypes extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('project_work_types');
        $table
            ->addColumn('guid', 'string')

            ->addColumn('project_id', 'integer', ['signed' => false, 'null' => false])
            ->addForeignKey('project_id', 'projects', 'id', ['delete'=> 'CASCADE', 'update'=> 'NO_ACTION'])

            ->addColumn('title', 'string')

            ->addColumn('budget_percent', 'integer', [
                'limit' => PostgresAdapter::INT_SMALL,
                'signed' => false
            ])

            ->addTimestamps()

            ->create();
    }
}
