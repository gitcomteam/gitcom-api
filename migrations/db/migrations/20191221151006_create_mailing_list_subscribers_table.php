<?php


use Phinx\Migration\AbstractMigration;

class CreateMailingListSubscribersTable extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('mailing_list_subscribers');
        $table
            ->addColumn('guid', 'string')
            ->addColumn('email', 'string')
            ->addColumn('unsubscribe_key', 'string')

            ->addColumn('created_at', 'timestamp', ['default' => 'CURRENT_TIMESTAMP'])

            ->addIndex(['guid'], ['unique' => true])
            ->addIndex(['email'], ['unique' => true])
            
            ->create();
    }
}
