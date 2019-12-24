<?php


use Phinx\Migration\AbstractMigration;

class UpdateFundingTransactionTableMakeInvoiceNullable extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('funding_transactions');
        $table->changeColumn('invoice_id', 'integer', ['signed' => false, 'null' => true])
              ->save();
    }
}
