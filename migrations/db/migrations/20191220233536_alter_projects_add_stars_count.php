<?php


use Phinx\Migration\AbstractMigration;

class AlterProjectsAddStarsCount extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('projects');
        $table
            ->addColumn('stars_count', 'integer', ['signed' => false, 'null' => false, 'default' => 0])
            ->save();
    }
}
