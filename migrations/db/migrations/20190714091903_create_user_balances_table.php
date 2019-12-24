<?php


use Phinx\Migration\AbstractMigration;

class CreateUserBalancesTable extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('user_balances');
        $table
            ->addColumn('guid', 'string')
            ->addColumn('user_id', 'integer', ['signed' => false, 'null' => false])
            ->addColumn('balance', 'decimal', [
                'default' => 0,
                'null' => false,
                'scale' => 8
            ])
            ->addColumn('crypto_token_id', 'integer', ['signed' => false, 'null' => true])
            
            ->addForeignKey('crypto_token_id', 'crypto_tokens', 'id', ['delete'=> 'NO_ACTION', 'update'=> 'NO_ACTION'])
            
            ->addTimestamps()
            ->addIndex(['guid'], ['unique' => true])
            ->create();

        $this->execute("ALTER TABLE user_balances ADD COLUMN currency_type CurrencyType NOT NULL;");
    }
}
