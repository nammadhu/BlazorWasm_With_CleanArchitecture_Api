use master
go
declare @dbnames nvarchar(max)
declare @statement nvarchar(max)
set @dbnames = ''
set @statement = ''
select @dbnames = @dbnames + ',[' + name + ']' from sys.databases 
    where name 
    NOT IN ('master','model','msdb','tempdb') 
    AND name NOT LIKE '%AdventureWorks%' -- Database to keep 
    AND name NOT LIKE '%DW%' -- Data warehouse database
    AND name NOT LIKE '%ReportServer%' -- Report server database
	and name not like '11marchvote' --any specific to exclude from deletion
if len(@dbnames) = 0
    begin
    print 'no databases to drop'
    end
else
    begin
    set @statement = 'drop database ' + substring(@dbnames, 2, len(@dbnames))
    print @statement
    exec sp_executesql @statement
    end
go