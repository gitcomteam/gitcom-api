<?php


use Phinx\Migration\AbstractMigration;

class CreateRepositoriesTable extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('repositories');
        $table
            ->addColumn('guid', 'string')
            ->addColumn('creator_id', 'integer', ['signed' => false], ['null' => false])
            ->addColumn('title', 'string')
            ->addColumn('repo_url', 'string')
            ->addColumn('created_at', 'timestamp', ['default' => 'CURRENT_TIMESTAMP'])
            ->addForeignKey('creator_id', 'users', 'id', ['delete'=> 'NO_ACTION', 'update'=> 'NO_ACTION'])

            ->addIndex(['guid'], ['unique' => true])
            ->create();

        $this->execute("CREATE TYPE RepoServiceType AS ENUM ('GitHub', 'GitLab', 'BitBucket');");
        
        $this->execute("ALTER TABLE repositories ADD COLUMN service_type RepoServiceType NOT NULL;");
    }
}
