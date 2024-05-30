USE master;
DECLARE @command nvarchar(max) = '';

SELECT @command = @command + 
'IF ''[' + name + ']'' NOT IN (''[master]'', ''[model]'', ''[msdb]'', ''[tempdb]'')
BEGIN
    ALTER DATABASE [' + name + '] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [' + name + '];
END
'
FROM sys.databases
WHERE database_id > 4;

EXEC sp_executesql @command;
