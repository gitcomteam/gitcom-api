<?php


use Phinx\Migration\AbstractMigration;

class AlterCurrencyTypeAddGitComToken extends AbstractMigration
{
    public function change()
    {
        $this->getAdapter()->commitTransaction();
        $this->execute("ALTER TYPE CurrencyType ADD VALUE 'GitComToken' AFTER 'LiteCoin';");
    }
}
