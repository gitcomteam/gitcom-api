<?php


use Phinx\Migration\AbstractMigration;

class AlterBoardsAndCardsMakeCreatorIdNullable extends AbstractMigration
{
    public function change()
    {
        $this->execute("ALTER TABLE boards ALTER COLUMN creator_id DROP NOT NULL;");
        $this->execute("ALTER TABLE cards ALTER COLUMN creator_id DROP NOT NULL;");
    }
}
