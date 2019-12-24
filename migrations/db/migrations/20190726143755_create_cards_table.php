<?php

use Phinx\Db\Adapter\MysqlAdapter;
use Phinx\Db\Adapter\PostgresAdapter;
use Phinx\Migration\AbstractMigration;

class CreateCardsTable extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('cards');
        $table
            ->addColumn('name', 'string')
            ->addColumn('guid', 'string')
            ->addColumn('column_order', 'integer')
            ->addColumn('creator_id', 'integer', [
                'signed' => false,
                'null' => false
            ])
            ->addColumn('assigned_id', 'integer', [
                'signed' => false,
                'null' => true
            ])
            ->addColumn('column_id', 'integer')
            ->addColumn('description', 'string', [
                'default' => ""
            ])
            ->addForeignKey('creator_id', 'users', 'id', array('delete'=> 'NO_ACTION', 'update'=> 'NO_ACTION'))
            ->addForeignKey('assigned_id', 'users', 'id', array('delete'=> 'NO_ACTION', 'update'=> 'NO_ACTION'))
            ->addForeignKey('column_id', 'board_columns', 'id', array('delete'=> 'NO_ACTION', 'update'=> 'NO_ACTION'))

            ->addColumn('created_at', 'timestamp', ['default' => 'CURRENT_TIMESTAMP'])
            ->addColumn('updated_at', 'timestamp', ['default' => 'CURRENT_TIMESTAMP'])
            ->create();
    }
}
