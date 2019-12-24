<?php


use Phinx\Migration\AbstractMigration;

class AlterServiceAccessTokensAddOriginUserId extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('service_access_tokens');
        $table
            ->addColumn('origin_user_id', 'string', ['null' => true])
            ->save();
    }
}
