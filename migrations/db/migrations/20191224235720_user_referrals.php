<?php


use Phinx\Migration\AbstractMigration;

class UserReferrals extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('user_referrals');
        $table
            ->addColumn('guid', 'string')
            ->addColumn('user_id', 'integer', ['signed' => false, 'null' => false])
            ->addColumn('referral_id', 'integer', ['signed' => false, 'null' => false])            
            ->addColumn('created_at', 'timestamp', ['default' => 'CURRENT_TIMESTAMP'])

            ->addForeignKey('user_id', 'users', 'id', ['delete'=> 'CASCADE', 'update'=> 'CASCADE'])
            ->addForeignKey('referral_id', 'users', 'id', ['delete'=> 'CASCADE', 'update'=> 'CASCADE'])
            
            ->create();
    }
}
