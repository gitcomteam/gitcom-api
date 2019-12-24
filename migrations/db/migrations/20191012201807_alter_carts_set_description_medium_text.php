<?php


use Phinx\Migration\AbstractMigration;

class AlterCartsSetDescriptionMediumText extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('cards');
        $table->changeColumn('description', 'text', [
            'default' => ''
        ])
            ->save();
    }
}
