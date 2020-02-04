<?php


use Phinx\Migration\AbstractMigration;

class AlterReposMakeCreatorIdNullable extends AbstractMigration
{
    public function change()
    {
        $this->getAdapter()->commitTransaction();
        $this->execute("ALTER TABLE repositories ALTER COLUMN creator_id DROP NOT NULL;");
    }
}
