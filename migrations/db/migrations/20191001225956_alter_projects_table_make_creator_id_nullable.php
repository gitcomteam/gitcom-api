<?php


use Phinx\Migration\AbstractMigration;

class AlterProjectsTableMakeCreatorIdNullable extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('projects');
        $table->changeColumn('creator_id', 'integer', ['signed' => false, 'null' => true])
              ->save();
    }
}
