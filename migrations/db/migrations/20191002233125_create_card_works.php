<?php


use Phinx\Migration\AbstractMigration;

class CreateCardWorks extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('card_works');
        $table
            ->addColumn('guid', 'string')

            ->addColumn('user_id', 'integer', ['signed' => false, 'null' => false])
            ->addForeignKey('user_id', 'users', 'id', ['delete'=> 'NO_ACTION', 'update'=> 'NO_ACTION'])

            ->addColumn('card_id', 'integer', ['signed' => false, 'null' => false])
            ->addForeignKey('card_id', 'cards', 'id', ['delete'=> 'NO_ACTION', 'update'=> 'NO_ACTION'])

            ->addColumn('work_type_id', 'integer', ['signed' => false, 'null' => false])
            ->addForeignKey('work_type_id', 'project_work_types', 'id', ['delete'=> 'NO_ACTION', 'update'=> 'NO_ACTION'])

            ->addColumn('proof', 'text')

            ->addTimestamps()

            ->create();
    }
}
