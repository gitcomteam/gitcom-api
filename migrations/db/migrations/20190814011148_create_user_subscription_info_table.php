<?php


use Phinx\Migration\AbstractMigration;

class CreateUserSubscriptionInfoTable extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('user_subscription_info');
        $table
            ->addColumn('user_id', 'integer', ['signed' => false, 'null' => false])
            ->addColumn('last_paid', 'timestamp', ['null' => true])
            ->addColumn('selected_amount', 'decimal', [
                'default' => 0,
                'null' => false,
                'scale' => 8
            ])
            ->addTimestamps()

            ->addIndex(['user_id'], ['unique' => true])
            ->addForeignKey('user_id', 'users', 'id', ['delete'=> 'NO_ACTION', 'update'=> 'NO_ACTION'])
            ->create();

        $this->execute("ALTER TABLE user_subscription_info ADD COLUMN selected_currency CurrencyType DEFAULT 'BitCoin';");
    }
}
