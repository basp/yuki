USE master
IF NOT EXISTS(SELECT * FROM sys.databases WHERE [name] = '{0}')
BEGIN
    CREATE DATABASE [{0}]
END
GO

USE [{0}]
IF NOT EXISTS(SELECT * FROM sys.schemas WHERE [name] = '{1}')
BEGIN
    EXEC('CREATE SCHEMA [{1}]')
END
GO

DECLARE @schemaId INT
SET @schemaId = (SELECT TOP 1 [schema_id] FROM sys.schemas WHERE [name] = '{1}')
IF NOT EXISTS(SELECT * FROM sys.tables WHERE [name] = 'Version' AND [schema_id] = @schemaId)
BEGIN
    CREATE TABLE [{1}].[Version] (
        VersionId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
        VersionName NVARCHAR(MAX) NOT NULL,
        RepositoryPath NVARCHAR(MAX) NOT NULL
    )
END
GO

DECLARE @schemaId INT
SET @schemaId = (SELECT TOP 1 [schema_id] FROM sys.schemas WHERE [name] = '{1}')
IF NOT EXISTS(SELECT * FROM sys.tables WHERE [name] = 'ScriptRun' AND [schema_id] = @schemaId)
BEGIN
    CREATE TABLE [{1}].[ScriptRun] (
        ScriptRunId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
        VersionId INT NOT NULL,
        ScriptName NVARCHAR(MAX) NOT NULL,
        TextOfScript NTEXT NOT NULL,
        TextHash NVARCHAR(MAX) NOT NULL,
        OneTimeScript BIT NOT NULL
    )
END
GO

DECLARE @schemaId INT
SET @schemaId = (SELECT TOP 1 [schema_id] FROM sys.schemas WHERE [name] = '{1}')
IF NOT EXISTS(SELECT * FROM sys.tables WHERE [name] = 'ScriptRunError' AND [schema_id] = @schemaId)
BEGIN
    CREATE TABLE [{1}].[ScriptRunError] (
        ScriptRunErrorId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
        ScriptName NVARCHAR(MAX) NOT NULL,
        RepositoryPath NVARCHAR(MAX) NOT NULL,
        VersionName NVARCHAR(MAX) NOT NULL,
        TextOfScript NTEXT NOT NULL,
        ErroneousPart NTEXT NOT NULL,
        ErrorMessage NTEXT NOT NULL
    )
END
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = 'DateCreated' AND object_id = OBJECT_ID('Version'))
BEGIN
    ALTER TABLE [{1}].Version ADD DateCreated DATETIME NULL
END
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = 'EnteredBy' AND object_id = OBJECT_ID('Version'))
BEGIN
    ALTER TABLE [{1}].Version ADD EnteredBy NVARCHAR(MAX) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = 'DateCreated' AND object_id = OBJECT_ID('ScriptRun'))
BEGIN
    ALTER TABLE [{1}].ScriptRun ADD DateCreated DATETIME NULL
END
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = 'EnteredBy' AND object_id = OBJECT_ID('Scriptrun'))
BEGIN
    ALTER TABLE [{1}].ScriptRun ADD EnteredBy NVARCHAR(MAX) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = 'DateCreated' AND object_id = OBJECT_ID('ScriptRunError'))
BEGIN
    ALTER TABLE [{1}].ScriptRunError ADD DateCreated DATETIME NULL
END
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = 'EnteredBy' AND object_id = OBJECT_ID('ScriptRunError'))
BEGIN
    ALTER TABLE [{1}].ScriptRunError ADD EnteredBy NVARCHAR(MAX) NULL
END
GO

DECLARE @schemaId INT
SET @schemaId = (SELECT TOP 1 [schema_id] FROM sys.schemas WHERE [name] = '{1}')
IF NOT EXISTS(SELECT * FROM sys.procedures WHERE [name] = 'InsertScriptRun' AND [schema_id] = @schemaId)
BEGIN
    DECLARE @sql VARCHAR(1000)
    SET @sql = 'CREATE PROCEDURE [{1}].InsertScriptRun AS SELECT * FROM sysobjects'
    EXEC(@sql)
END
GO

ALTER PROCEDURE [{1}].InsertScriptRun (
    @VersionId INT,
    @ScriptName NVARCHAR(MAX),
    @TextOfScript NTEXT,
    @TextHash NVARCHAR(MAX),
    @OneTimeScript BIT,
    @EnteredBy NVARCHAR(MAX)
)
AS
INSERT INTO [{1}].ScriptRun (
    VersionId, 
    ScriptName, 
    TextOfScript, 
    TextHash, 
    OneTimeScript,
    DateCreated,
    EnteredBy
)
VALUES (
    @VersionId, 
    @ScriptName,
    @TextOfScript,
    @TextHash,
    @OneTimeScript,
    GETDATE(),
    @EnteredBy
)
SELECT CAST(SCOPE_IDENTITY() AS INT)
GO

DECLARE @schemaId INT
SET @schemaId = (SELECT TOP 1 [schema_id] FROM sys.schemas WHERE [name] = '{1}')
IF NOT EXISTS(SELECT * FROM sys.procedures WHERE [name] = 'InsertScriptRunError' AND [schema_id] = @schemaId)
BEGIN
    DECLARE @sql VARCHAR(1000)
    SET @sql = 'CREATE PROCEDURE [{1}].InsertScriptRunError AS SELECT * FROM sysobjects'
    EXEC(@sql)
END
GO

ALTER PROCEDURE [{1}].InsertScriptRunError (
    @RepositoryPath NVARCHAR(MAX),
    @ScriptName NVARCHAR(MAX),
    @VersionName NVARCHAR(MAX),
    @TextOfScript NTEXT,
    @ErroneousPart NTEXT,
    @ErrorMessage NTEXT,
    @EnteredBy NVARCHAR(MAX)
)
AS
INSERT INTO [{1}].ScriptRunError (
    RepositoryPath,
    ScriptName,
    VersionName,
    TextOfScript,
    ErroneousPart,
    ErrorMessage,
    DateCreated,
    EnteredBy
)
VALUES (
    @RepositoryPath,
    @ScriptName,
    @VersionName,
    @TextOfScript,
    @ErroneousPart,
    @ErrorMessage,
    GETDATE(),
    @EnteredBy
)
SELECT CAST(SCOPE_IDENTITY() AS INT)
GO

DECLARE @schemaId INT
SET @schemaId = (SELECT TOP 1 [schema_id] FROM sys.schemas WHERE [name] = '{1}')
IF NOT EXISTS(SELECT * FROM sys.procedures WHERE [name] = 'GetVersion' AND [schema_id] = @schemaId)
BEGIN
    DECLARE @sql VARCHAR(1000)
    SET @sql = 'CREATE PROCEDURE [{1}].GetVersion AS SELECT * FROM sysobjects'
    EXEC(@sql)
END
GO

ALTER PROCEDURE [{1}].GetVersion (
    @RepositoryPath NVARCHAR(MAX)
)
AS
SELECT TOP 1 VersionName
FROM [{1}].Version
WHERE RepositoryPath = @RepositoryPath
ORDER BY VersionId DESC
GO

DECLARE @schemaId INT
SET @schemaId = (SELECT TOP 1 [schema_id] FROM sys.schemas WHERE [name] = '{1}')
IF NOT EXISTS(SELECT * FROM sys.procedures WHERE [name] = 'InsertVersion' AND [schema_id] = @schemaId)
BEGIN
    DECLARE @sql VARCHAR(1000)
    SET @sql = 'CREATE PROCEDURE [{1}].InsertVersion AS SELECT * FROM sysobjects'
    EXEC(@sql)
END
GO

ALTER PROCEDURE [{1}].InsertVersion (
    @VersionName NVARCHAR(MAX),
    @RepositoryPath NVARCHAR(MAX),
    @EnteredBy NVARCHAR(MAX)
)
AS
INSERT INTO [{1}].Version (VersionName, RepositoryPath, DateCreated, EnteredBy)
VALUES (@VersionName, @RepositoryPath, GETDATE(), @EnteredBy)
SELECT CAST(SCOPE_IDENTITY() AS INT)
GO

DECLARE @schemaId INT
SET @schemaId = (SELECT TOP 1 [schema_id] FROM sys.schemas WHERE [name] = '{1}')
IF NOT EXISTS(SELECT * FROM sys.procedures WHERE [name] = 'HasScriptRunAlready' AND [schema_id] = @schemaId)
BEGIN
    DECLARE @sql VARCHAR(1000)
    SET @sql = 'CREATE PROCEDURE [{1}].HasScriptRunAlready AS SELECT * FROM sysobjects'
    EXEC(@sql)
END
GO

ALTER PROCEDURE [{1}].HasScriptRunAlready (
    @ScriptName NVARCHAR(MAX)
)
AS
SELECT COUNT(ScriptRunId)
FROM [{1}].ScriptRun
WHERE ScriptName = @ScriptName
GO

DECLARE @schemaId INT
SET @schemaId = (SELECT TOP 1 [schema_id] FROM sys.schemas WHERE [name] = '{1}')
IF NOT EXISTS(SELECT * FROM sys.procedures WHERE [name] = 'GetCurrentScriptHash' AND [schema_id] = @schemaId)
BEGIN
    DECLARE @sql VARCHAR(1000)
    SET @sql = 'CREATE PROCEDURE [{1}].GetCurrentScriptHash AS SELECT * FROM sysobjects'
    EXEC(@sql)
END
GO

ALTER PROCEDURE [{1}].GetCurrentScriptHash (
    @ScriptName NVARCHAR(MAX)
)
AS
SELECT TOP 1 TextHash
FROM [{1}].ScriptRun
WHERE ScriptName = @ScriptName
ORDER BY ScriptRunId DESC
GO