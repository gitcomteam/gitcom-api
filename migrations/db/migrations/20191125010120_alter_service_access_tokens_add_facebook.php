<?php


use Phinx\Migration\AbstractMigration;

class AlterServiceAccessTokensAddFacebook extends AbstractMigration
{
    public function change()
    {
    	$this->getAdapter()->commitTransaction();
        $this->execute("ALTER TYPE ServiceType ADD VALUE 'Facebook' AFTER 'BitBucket';");
    }
}