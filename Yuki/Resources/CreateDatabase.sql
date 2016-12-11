USE [master]

DECLARE @created BIT = 0
IF NOT EXISTS (SELECT * FROM sys.databases WHERE [name] = '{Database}')
BEGIN
    CREATE DATABASE [{Database}]
    SET @created = 1
END
SELECT @created