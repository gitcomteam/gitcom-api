<?php

use Phinx\Db\Adapter\MysqlAdapter;
use Phinx\Db\Adapter\PostgresAdapter;
use Phinx\Migration\AbstractMigration;

class CreateBoardColumnsTable extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('board_columns');
        $table
            ->addColumn('name', 'string')
            ->addColumn('guid','string')
            ->addColumn('board_order', 'integer', [
                'limit' => PostgresAdapter::INT_SMALL,
                'signed' => false
            ])
            ->addColumn('board_id', 'integer',[
                'signed' => false,
                'default' => 0
            ])
            ->addForeignKey('board_id', 'boards', 'id', array('delete'=> 'NO_ACTION', 'update'=> 'NO_ACTION'))
            ->create();
    }
}
