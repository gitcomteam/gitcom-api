<?php


use Phinx\Migration\AbstractMigration;

class CreateWithdrawalRequestsTable extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('withdrawal_requests');
        $table
            ->addColumn('guid', 'string')
            ->addColumn('user_id', 'integer', ['signed' => false, 'null' => false])
            ->addColumn('amount', 'decimal', [
                'default' => 0,
                'null' => false,
                'scale' => 8
            ])
            ->addColumn('paid', 'boolean', ['default' => false])
            ->addTimestamps()

            ->addForeignKey('user_id', 'users', 'id', ['delete'=> 'CASCADE', 'update'=> 'CASCADE'])
            
            ->addIndex(['guid'], ['unique' => true])

            ->create();

        $this->execute("ALTER TABLE withdrawal_requests ADD COLUMN currency_type CurrencyType NOT NULL;");
    }
}
