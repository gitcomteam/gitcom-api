<?php


use Phinx\Migration\AbstractMigration;

class UpdateEntityType extends AbstractMigration
{
    public function change()
    {
        $this->getAdapter()->commitTransaction();
        $this->execute("ALTER TYPE EntityType ADD VALUE 'User' AFTER 'UserBalance';");
        $this->execute("ALTER TYPE EntityType ADD VALUE 'ProjectProductPurchase' AFTER 'User';");
    }
}
