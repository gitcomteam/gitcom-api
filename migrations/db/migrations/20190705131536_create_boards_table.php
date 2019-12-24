<?php


use Phinx\Migration\AbstractMigration;

class CreateBoardsTable extends AbstractMigration
{

    public function change()
    {
      $table = $this->table('boards');
      $table
          ->addColumn('guid', 'string')
          ->addColumn('creator_id', 'integer', ['signed' => false])
          ->addColumn('project_id', 'integer', ['signed' => false])
          ->addColumn('name', 'string', ['null' => false])
          ->addColumn('description', 'string')
          ->addColumn('created_at', 'timestamp', ['default' => 'CURRENT_TIMESTAMP'])
          ->addColumn('updated_at', 'timestamp', ['default' => 'CURRENT_TIMESTAMP'])
          ->addForeignKey('creator_id', 'users', 'id', array('delete'=> 'SET_NULL', 'update'=> 'NO_ACTION'))
          ->addForeignKey('project_id', 'projects', 'id', array('delete'=> 'SET_NULL', 'update'=> 'NO_ACTION'))
          ->addIndex(['guid'],['unique' => true])
          ->create();
    }
}
