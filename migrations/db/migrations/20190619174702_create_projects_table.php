<?php


use Phinx\Migration\AbstractMigration;

class CreateProjectsTable extends AbstractMigration
{
    public function change()
    {
      $table = $this->table('projects');
      $table
          ->addColumn('guid', 'string')
          ->addColumn('creator_id', 'integer')
          ->addColumn('name', 'string', ['null' => false])
          ->addColumn('repository_id', 'integer', ['signed' => false], ['null' => false])
          ->addForeignKey('creator_id', 'users', 'id', array('delete'=> 'NO_ACTION', 'update'=> 'NO_ACTION'))
          ->addIndex(['guid'],['unique' => true])
          ->create();
    }
}
