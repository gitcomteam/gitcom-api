<?php


use Phinx\Migration\AbstractMigration;

class CreateEntityDecisionVotesTable extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('entity_decision_votes');
        $table
            ->addColumn('guid', 'string')

            ->addColumn('user_id', 'integer', ['signed' => false, 'null' => false])
            ->addForeignKey('user_id', 'users', 'id', ['delete'=> 'NO_ACTION', 'update'=> 'NO_ACTION'])

            ->addColumn('option_id', 'integer', ['signed' => false, 'null' => false])
            ->addForeignKey('option_id', 'entity_decision_options', 'id', ['delete'=> 'NO_ACTION', 'update'=> 'NO_ACTION'])

            ->addColumn('votepower', 'integer', ['signed' => false, 'null' => false])

            ->addTimestamps()

            ->create();
    }
}
