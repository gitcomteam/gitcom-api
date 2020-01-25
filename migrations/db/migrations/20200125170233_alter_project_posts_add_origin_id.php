<?php


use Phinx\Migration\AbstractMigration;

class AlterProjectPostsAddOriginId extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('project_posts');
        $table->addColumn('origin_id', 'string', ['null' => true])
            ->save();
    }
}
