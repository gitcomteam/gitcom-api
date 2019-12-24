<?php


use Phinx\Migration\AbstractMigration;

class AlterRepositoriesAddOriginId extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('repositories');
        $table
            ->addColumn('origin_id', 'string', ['null' => true])
            ->save();
    }
}
