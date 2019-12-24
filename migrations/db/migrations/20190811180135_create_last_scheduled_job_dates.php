<?php


use Phinx\Migration\AbstractMigration;

class CreateLastScheduledJobDates extends AbstractMigration
{
    public function change()
    {
        $table = $this->table('last_scheduled_job_dates');
        $table
            ->addColumn('guid', 'string')
            ->addColumn('name', 'string')
            ->addColumn('last_executed', 'timestamp')
            ->addTimestamps()

            ->addIndex(['guid'], ['unique' => true])
            ->addIndex(['name'], ['unique' => true])
            ->create();
    }
}
