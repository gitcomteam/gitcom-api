<?php


use Phinx\Migration\AbstractMigration;

class CreateEntityDecisionsTable extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('entity_decisions');
        $table
            ->addColumn('guid', 'string')
            ->addColumn('creator_id', 'integer', ['signed' => false, 'null' => false])
            ->addColumn('entity_id', 'integer', ['signed' => false, 'null' => false])
            
            ->addColumn('title', 'string')
            ->addColumn('content', 'text')
            ->addColumn('deadline', 'timestamp')

            ->addTimestamps()

            ->addForeignKey('creator_id', 'users', 'id', ['delete'=> 'NO_ACTION', 'update'=> 'NO_ACTION'])

            ->create();

        $this->execute("ALTER TABLE entity_decisions ADD COLUMN entity_type EntityType NOT NULL;");

        $this->execute("CREATE TYPE DecisionStatus AS ENUM ('Open', 'Closed', 'Canceled');");
        $this->execute("ALTER TABLE entity_decisions ADD COLUMN status DecisionStatus DEFAULT 'Open' NOT NULL;");
    }
}
