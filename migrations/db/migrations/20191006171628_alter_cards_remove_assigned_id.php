<?php


use Phinx\Migration\AbstractMigration;

class AlterCardsRemoveAssignedId extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('cards');
        $table->removeColumn('assigned_id')
            ->save(); 
    }
}
