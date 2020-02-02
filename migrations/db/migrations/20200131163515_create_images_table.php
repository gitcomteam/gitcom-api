<?php


use Phinx\Migration\AbstractMigration;

class CreateImagesTable extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('images');
        $table
            ->addColumn('guid', 'string')
            ->addColumn('url', 'string')

            ->addColumn('created_at', 'timestamp', ['default' => 'CURRENT_TIMESTAMP'])
            
            ->addIndex(['guid'], ['unique' => true])

            ->create();
    }
}
