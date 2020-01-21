<?php

use Phinx\Migration\AbstractMigration;

class CreateProjectPostsTable extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('project_posts');
        $table
            ->addColumn('guid', 'string')
            ->addColumn('project_id', 'integer', ['signed' => false, 'null' => false])
            ->addColumn('title', 'text')
            ->addColumn('content', 'text')

            ->addForeignKey('project_id', 'projects', 'id', ['delete'=> 'CASCADE', 'update'=> 'CASCADE'])

            ->addTimestamps()
            
            ->addIndex(['guid'], ['unique' => true])

            ->create();
    }
}
