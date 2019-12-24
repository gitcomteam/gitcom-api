<?php


use Phinx\Migration\AbstractMigration;

class AlterFundingBalancesAddCryptoTokenId extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('funding_balances')
            ->addColumn('crypto_token_id', 'integer', ['signed' => false, 'null' => true])
            ->addForeignKey('crypto_token_id', 'crypto_tokens', 'id', ['delete'=> 'NO_ACTION', 'update'=> 'NO_ACTION'])
            ->update();
    }
}
