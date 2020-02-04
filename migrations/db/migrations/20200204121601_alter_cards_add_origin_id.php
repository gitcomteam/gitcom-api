<?php


use Phinx\Migration\AbstractMigration;

class AlterCardsAddOriginId extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('cards');
        $table->addColumn('origin_id', 'string', ['null' => true])
              ->save();
    }
}
