<?php


use Phinx\Migration\AbstractMigration;

class AlterUserBadgesAddTimetamps extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('user_badges');
        $table->addColumn('created_at', 'timestamp', ['default' => 'CURRENT_TIMESTAMP'])
            ->save();
    }
}
