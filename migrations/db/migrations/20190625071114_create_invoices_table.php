<?php


use Phinx\Migration\AbstractMigration;

class CreateInvoicesTable extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('invoices');
        $table
            ->addColumn('guid', 'string')
            ->addColumn('user_id', 'integer', ['signed' => false, 'null' => true])
            ->addColumn('entity_id', 'integer', ['signed' => false, 'null' => false])
            ->addColumn('amount', 'decimal', [
                'default' => 0,
                'null' => false,
                'scale' => 8
            ])
            ->addTimestamps()

            ->addForeignKey('user_id', 'users', 'id', ['delete'=> 'NO_ACTION', 'update'=> 'NO_ACTION'])

            ->addIndex(['guid'], ['unique' => true])
            ->create();

        $this->execute("CREATE TYPE InvoiceStatus AS ENUM ('Created','Failed','RequiresConfirmation','Confirmed','Done');");
        
        $this->execute("ALTER TABLE invoices ADD COLUMN status InvoiceStatus NOT NULL;");
        $this->execute("ALTER TABLE invoices ADD COLUMN entity_type EntityType NOT NULL;");
        $this->execute("ALTER TABLE invoices ADD COLUMN currency_type CurrencyType NOT NULL;");
    }
}
