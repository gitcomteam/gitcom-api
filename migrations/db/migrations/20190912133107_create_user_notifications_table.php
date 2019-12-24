<?php


use Phinx\Migration\AbstractMigration;

class CreateUserNotificationsTable extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('user_notifications');
        $table
            ->addColumn('guid', 'string')
            ->addColumn('user_id', 'integer', ['signed' => false, 'null' => false])
            
            ->addColumn('title', 'string')
            ->addColumn('content', 'text')
            ->addColumn('seen', 'boolean')
            ->addTimestamps()

            ->addForeignKey('user_id', 'users', 'id', ['delete'=> 'NO_ACTION', 'update'=> 'NO_ACTION'])
            
            ->create();

        $this->execute("CREATE TYPE UserNotificationType AS ENUM ('Info', 'Success', 'Warning', 'Critical');");
        $this->execute("ALTER TABLE user_notifications ADD COLUMN type UserNotificationType NOT NULL;");
    }
}
