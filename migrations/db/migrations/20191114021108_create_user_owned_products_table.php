<?php


use Phinx\Migration\AbstractMigration;

class CreateUserOwnedProductsTable extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('user_owned_products');
        $table
            ->addColumn('guid', 'string')
            ->addColumn('user_id', 'integer', ['signed' => false, 'null' => false])
            ->addColumn('product_id', 'integer', ['signed' => false, 'null' => false])
            ->addColumn('expiry_at', 'timestamp')
            ->addTimestamps()

            ->addForeignKey('user_id', 'users', 'id', ['delete'=> 'CASCADE', 'update'=> 'CASCADE'])
            ->addForeignKey('product_id', 'project_products', 'id', ['delete'=> 'CASCADE', 'update'=> 'CASCADE'])
            
            ->create();
    }
}
