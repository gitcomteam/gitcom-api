<?php


use Phinx\Migration\AbstractMigration;

class CreateUserProjectsLibraryTable extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('user_projects_library');
        $table
            ->addColumn('guid', 'string')
            ->addColumn('user_id', 'integer', ['signed' => false, 'null' => false])
            ->addColumn('project_id', 'integer', ['signed' => false, 'null' => false])
            ->addTimestamps()

            ->addForeignKey('user_id', 'users', 'id', ['delete'=> 'CASCADE', 'update'=> 'CASCADE'])
            ->addForeignKey('project_id', 'projects', 'id', ['delete'=> 'CASCADE', 'update'=> 'CASCADE'])
            
            ->create();
    }
}
