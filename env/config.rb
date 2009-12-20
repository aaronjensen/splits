class String
  def to_underscore
    self.gsub(/::/, '/'). gsub(/([A-Z]+)([A-Z][a-z])/,'\1_\2').gsub(/([a-z\d])([A-Z])/,'\1_\2').tr("-", "_").downcase
  end
end

$project = "Splits"

$database_name = $project.to_underscore
$database_user = $project.to_underscore
$database_password = "asdf1234!"

$test_database_name = "#{$project.to_underscore}_test"
$test_database_user = "#{$project.to_underscore}_test"
$test_database_password = "asdf1234!"

$report_database_name = "#{$project.to_underscore}_reports"
$report_database_user = "#{$project.to_underscore}_reports"
$report_database_password = "asdf1234!"

$test_report_database_name = "#{$project.to_underscore}_reports_test"
$test_report_database_user = "#{$project.to_underscore}_reports_test"
$test_report_database_password = "asdf1234!"

$queues = ["#{$project.to_underscore}", "#{$project.to_underscore}_poison"]
