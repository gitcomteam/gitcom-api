<?php


use Phinx\Migration\AbstractMigration;

class AlterTxsAddNote extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('funding_transactions');
        $table->addColumn('note', 'string', ['null' => true, 'default' => null])
            ->save();
    }
}
