<?php


use Phinx\Migration\AbstractMigration;

class CreateProjectTeamMembersTable extends AbstractMigration
{
  public function change()
  {
    $table = $this->table('project_team_members');
    $table
        ->addColumn('project_id', 'integer')
        ->addColumn('user_id', 'integer')
        ->addForeignKey('project_id', 'projects', 'id', array('delete'=> 'NO_ACTION', 'update'=> 'NO_ACTION'))
        ->addForeignKey('user_id', 'users', 'id', array('delete'=> 'NO_ACTION', 'update'=> 'NO_ACTION'))
        ->addColumn('created_at', 'timestamp', ['default' => 'CURRENT_TIMESTAMP'])
        ->create();
  }
}
