require 'tempfile'

def truncate_database(database)
  sql_file(database, 'db/truncate.sql')
end

def sql_file(database, filepath) 
  `sqlcmd -H localhost -E -i \"#{filepath}\" -d #{database}`
end

def sql(database, query) 
  file = Tempfile.new("query") 
  file.print(query)
  file.flush
  file.close

  result = sql_file(database, file.path)

  file.delete

  return result
end

def grant_access_to_database(database, username) 
  sql(database, "CREATE USER #{username} FOR LOGIN #{username} WITH DEFAULT_SCHEMA = dbo")
  sql(database, "EXEC sp_addrolemember 'db_owner', '#{username}'")
end

def create_database(name, username, password)
  result = sql('master', "SELECT COUNT(*) AS Number FROM sys.master_files s_mf WHERE s_mf.state = 0 and db_name(s_mf.database_id) = '#{name}' AND has_dbaccess(db_name(s_mf.database_id)) = 1")

  return false unless result.to_s.gsub(/\n/, '') =~ /Number\s*-*\s*0/

  puts "Creating database #{name}"

  sql('master', "IF (
  SELECT COUNT(*) AS Number 
  FROM sys.master_files s_mf 
  WHERE 
    s_mf.state = 0 AND db_name(s_mf.database_id) = '#{name}' AND has_dbaccess(db_name(s_mf.database_id)) = 1
) = 0 
BEGIN
CREATE DATABASE [#{name}]
END")

  sql('master', "IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'#{name}')
BEGIN
CREATE LOGIN #{username} WITH PASSWORD = '#{password}', DEFAULT_DATABASE = #{name}
END")

  grant_access_to_database(name, username)

  return true
end
