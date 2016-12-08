USE master

DECLARE @created BIT = 0
IF NOT EXISTS (SELECT * FROM sys.databases WHERE [name] = '{0}')
BEGIN
    CREATE DATABASE [{0}]
    SET @created = 1
END
SELECT @created