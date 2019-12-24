<?php


use Phinx\Migration\AbstractMigration;

class CreateCryptoTokensTable extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('crypto_tokens');
        $table
            ->addColumn('guid', 'string')
            ->addIndex(['guid'], ['unique' => true])
            ->addColumn('name', 'string')
            ->addColumn('contract', 'string')
            ->addTimestamps()
            ->create();

        $this->execute("ALTER TABLE crypto_tokens ADD COLUMN currency_type CurrencyType NOT NULL;");
    }
}
