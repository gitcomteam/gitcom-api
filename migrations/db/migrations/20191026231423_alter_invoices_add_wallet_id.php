<?php


use Phinx\Migration\AbstractMigration;

class AlterInvoicesAddWalletId extends AbstractMigration
{
    
    public function change()
    {
        $table = $this->table('invoices');
        $table
            ->addColumn('wallet_id', 'integer', ['signed' => false, 'null' => false])
            ->addForeignKey('wallet_id', 'currency_wallets', 'id', ['delete'=> 'NO_ACTION', 'update'=> 'NO_ACTION'])
            ->save();
    }
}
