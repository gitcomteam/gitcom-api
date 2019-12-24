<?php


use Phinx\Migration\AbstractMigration;

class AlterFundingTransactionsAddInvoiceId extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('funding_transactions')
            ->addColumn('invoice_id', 'integer', ['signed' => false, 'null' => false])
            ->addForeignKey('invoice_id', 'invoices', 'id', ['delete'=> 'NO_ACTION', 'update'=> 'NO_ACTION'])
            ->update();
    }
}