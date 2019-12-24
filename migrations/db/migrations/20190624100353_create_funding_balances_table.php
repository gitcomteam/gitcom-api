<?php


use Phinx\Migration\AbstractMigration;

class CreateFundingBalancesTable extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('funding_balances');
        $table
            ->addColumn('guid', 'string')
            ->addColumn('entity_id', 'integer', ['signed' => false], ['null' => false])
            ->addColumn('balance', 'decimal', [
                'default' => 0,
                'null' => false,
                'scale' => 8
            ])
            ->addTimestamps()

            ->addIndex(['guid'], ['unique' => true])
            ->create();

        $this->execute("ALTER TABLE funding_balances ADD COLUMN entity_type EntityType NOT NULL;");
        $this->execute("ALTER TABLE funding_balances ADD COLUMN currency_type CurrencyType NOT NULL;");
    }
}
