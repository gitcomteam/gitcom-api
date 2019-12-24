<?php


use Phinx\Migration\AbstractMigration;

class CreateCurrencyWalletsTable extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('currency_wallets');
        $table
            ->addColumn('guid', 'string')

            ->addColumn('address', 'string')

            ->addTimestamps()

            ->create();

        $this->execute("ALTER TABLE currency_wallets ADD COLUMN currency_type CurrencyType NOT NULL;");
    }
}
