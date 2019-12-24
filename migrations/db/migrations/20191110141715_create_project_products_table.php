<?php


use Phinx\Migration\AbstractMigration;

class CreateProjectProductsTable extends AbstractMigration
{
    public function change()
    {
    	$table = $this->table('project_products');
        $table
            ->addColumn('guid', 'string')
            ->addColumn('name', 'string')
            ->addColumn('description', 'text')
            ->addColumn('url', 'string', ['null' => true])
            ->addColumn('use_url', 'string', ['null' => true])
            ->addColumn('project_id', 'integer', ['signed' => false, 'null' => false])
            ->addColumn('usd_price_pennies', 'integer', ['null' => false, 'default' => 0])
            ->addColumn('duration_hours', 'integer', ['null' => false, 'default' => 0])
            ->addTimestamps()

            ->addForeignKey('project_id', 'projects', 'id', ['delete'=> 'CASCADE', 'update'=> 'CASCADE'])
            
            ->create();
    }
}
