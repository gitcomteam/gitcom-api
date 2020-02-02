<?php


use Phinx\Migration\AbstractMigration;

class CreateProjectImagesTable extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('project_images');
        $table
            ->addColumn('guid', 'string')
            ->addColumn('project_id', 'integer', ['signed' => false, 'null' => false])
            ->addColumn('image_id', 'integer', ['signed' => false, 'null' => false])

            ->addColumn('created_at', 'timestamp', ['default' => 'CURRENT_TIMESTAMP'])

            ->addIndex(['guid'], ['unique' => true])
            ->addForeignKey('project_id', 'projects', 'id', ['delete'=> 'CASCADE', 'update'=> 'CASCADE'])
            ->addForeignKey('image_id', 'images', 'id', ['delete'=> 'CASCADE', 'update'=> 'CASCADE'])

            ->create();
    }
}
