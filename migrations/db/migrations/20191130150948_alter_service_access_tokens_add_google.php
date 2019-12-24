<?php


use Phinx\Migration\AbstractMigration;

class AlterServiceAccessTokensAddGoogle extends AbstractMigration
{
    public function change()
    {
        $this->getAdapter()->commitTransaction();
        $this->execute("ALTER TYPE ServiceType ADD VALUE 'Google' AFTER 'Facebook';");
    }
}
