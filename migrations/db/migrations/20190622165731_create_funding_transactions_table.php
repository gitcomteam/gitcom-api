<?php


use Phinx\Migration\AbstractMigration;

class CreateFundingTransactionsTable extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('funding_transactions');
        $table
            ->addColumn('guid', 'string')
            ->addColumn('from_user_id', 'integer', ['signed' => false, 'null' => true])
            ->addColumn('entity_id', 'integer', ['signed' => false, 'null' => false])
            ->addColumn('amount', 'decimal', [
                'default' => 0,
                'null' => false,
                'scale' => 8
            ])
            ->addColumn('created_at', 'timestamp', ['default' => 'CURRENT_TIMESTAMP'])

            ->addForeignKey('from_user_id', 'users', 'id', ['delete'=> 'NO_ACTION', 'update'=> 'NO_ACTION'])

            ->addIndex(['guid'], ['unique' => true])
            ->create();

        $this->execute("CREATE TYPE EntityType AS ENUM ('ProjectCategory', 'Project', 'Board', 'Card', 'BacklogItem', 'Issue');");
        $this->execute("CREATE TYPE CurrencyType AS ENUM ('Usd', 'BitCoin', 'Ethereum', 'Erc20Token', 'Waves', 'WavesToken', 'LiteCoin');");

        $this->execute("ALTER TABLE funding_transactions ADD COLUMN entity_type EntityType NOT NULL;");
        $this->execute("ALTER TABLE funding_transactions ADD COLUMN currency_type CurrencyType NOT NULL;");
    }
}
