<?php


use Phinx\Migration\AbstractMigration;

class AlterEntityTypeEnumAddUserBalance extends AbstractMigration
{
    public function up()
    {
        $this->getAdapter()->commitTransaction();
        $this->execute("ALTER TYPE EntityType ADD VALUE 'UserBalance' AFTER 'Issue';");
    }
}
