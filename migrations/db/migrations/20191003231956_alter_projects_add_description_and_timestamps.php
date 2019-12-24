<?php


use Phinx\Migration\AbstractMigration;

class AlterProjectsAddDescriptionAndTimestamps extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('projects');
        $table
            ->addColumn('description', 'string', ['default' => ''])
            ->addTimestamps()
            ->save();
    }
}
