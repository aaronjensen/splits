require 'env/config.rb'
require 'env/db.rb'

task :setup => [ "setup:all" ]

file ".setup-done" => [ "env/setup.rb", "env/db.rb", "env/config.rb" ] do
  Rake::Task["setup:all"].invoke
end

namespace :setup do
  task :all => [ 
    :create_db, 
    :create_queues,
    :setup_urlstrong ] do
    touch ".setup-done"
  end

  desc "Creating dbs"
  task :create_db do
    create_database($database_name, $database_user, $database_password)
    create_database($test_database_name, $test_database_user, $test_database_password)
    create_database($report_database_name, $report_database_user, $report_database_password)
    create_database($test_report_database_name, $test_report_database_user, $test_report_database_password)
  end

  desc "Creating queues"
  task :create_queues do
    tmpQueueFile = Tempfile.new("queues.txt")
    $queues.each { |queue|
      tmpQueueFile.print ".\\private$\\#{queue}\n"
    }
    tmpQueueFile.close
    sh "cmd /c tools\\ManageQueues.exe --recreate #{tmpQueueFile.path}"
  end

  desc "Setting up urlstrong generator"
  task :setup_urlstrong do
    sh "tools\\regasm.exe /codebase lib\\Machine\\UrlStrong\\Machine.UrlStrong.VisualStudioCodeGenerator.dll" 
  end
end
