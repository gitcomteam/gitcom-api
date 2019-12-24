<?php


use Phinx\Migration\AbstractMigration;

class CreateEntityDecisionOptionsTable extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('entity_decision_options');
        $table
            ->addColumn('guid', 'string')

            ->addColumn('decision_id', 'integer', ['signed' => false, 'null' => false])
            ->addForeignKey('decision_id', 'entity_decisions', 'id', ['delete'=> 'NO_ACTION', 'update'=> 'NO_ACTION'])

            ->addColumn('title', 'string')

            ->addColumn('order', 'integer')

            ->addTimestamps()

            ->create();
    }
}
