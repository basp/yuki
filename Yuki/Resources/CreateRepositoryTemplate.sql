USE master
IF NOT EXISTS(SELECT * FROM sys.databases WHERE [name] = '{RepositoryDatabase}')
BEGIN
    CREATE DATABASE [{RepositoryDatabase}]
END
GO

USE [{RepositoryDatabase}]
IF NOT EXISTS(SELECT * FROM sys.schemas WHERE [name] = '{RepositorySchema}')
BEGIN
    EXEC('CREATE SCHEMA [{RepositorySchema}]')
END
GO

DECLARE @schemaId INT
SET @schemaId = (SELECT TOP 1 [schema_id] FROM sys.schemas WHERE [name] = '{RepositorySchema}')
IF NOT EXISTS(SELECT * FROM sys.tables WHERE [name] = 'Version' AND [schema_id] = @schemaId)
BEGIN
    CREATE TABLE [{RepositorySchema}].[Version] (
        VersionId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
        VersionName NVARCHAR(MAX) NOT NULL,
        RepositoryPath NVARCHAR(MAX) NOT NULL
    )
END
GO

DECLARE @schemaId INT
SET @schemaId = (SELECT TOP 1 [schema_id] FROM sys.schemas WHERE [name] = '{RepositorySchema}')
IF NOT EXISTS(SELECT * FROM sys.tables WHERE [name] = 'ScriptRun' AND [schema_id] = @schemaId)
BEGIN
    CREATE TABLE [{RepositorySchema}].[ScriptRun] (
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
SET @schemaId = (SELECT TOP 1 [schema_id] FROM sys.schemas WHERE [name] = '{RepositorySchema}')
IF NOT EXISTS(SELECT * FROM sys.tables WHERE [name] = 'ScriptRunError' AND [schema_id] = @schemaId)
BEGIN
    CREATE TABLE [{RepositorySchema}].[ScriptRunError] (
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
    ALTER TABLE [{RepositorySchema}].Version ADD DateCreated DATETIME NULL
END
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = 'EnteredBy' AND object_id = OBJECT_ID('Version'))
BEGIN
    ALTER TABLE [{RepositorySchema}].Version ADD EnteredBy NVARCHAR(MAX) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = 'DateCreated' AND object_id = OBJECT_ID('ScriptRun'))
BEGIN
    ALTER TABLE [{RepositorySchema}].ScriptRun ADD DateCreated DATETIME NULL
END
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = 'EnteredBy' AND object_id = OBJECT_ID('Scriptrun'))
BEGIN
    ALTER TABLE [{RepositorySchema}].ScriptRun ADD EnteredBy NVARCHAR(MAX) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = 'DateCreated' AND object_id = OBJECT_ID('ScriptRunError'))
BEGIN
    ALTER TABLE [{RepositorySchema}].ScriptRunError ADD DateCreated DATETIME NULL
END
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = 'EnteredBy' AND object_id = OBJECT_ID('ScriptRunError'))
BEGIN
    ALTER TABLE [{RepositorySchema}].ScriptRunError ADD EnteredBy NVARCHAR(MAX) NULL
END
GO

DECLARE @schemaId INT
SET @schemaId = (SELECT TOP 1 [schema_id] FROM sys.schemas WHERE [name] = '{RepositorySchema}')
IF NOT EXISTS(SELECT * FROM sys.procedures WHERE [name] = 'InsertScriptRun' AND [schema_id] = @schemaId)
BEGIN
    DECLARE @sql VARCHAR(1000)
    SET @sql = 'CREATE PROCEDURE [{RepositorySchema}].InsertScriptRun AS SELECT * FROM sysobjects'
    EXEC(@sql)
END
GO

ALTER PROCEDURE [{RepositorySchema}].InsertScriptRun (
    @VersionId INT,
    @ScriptName NVARCHAR(MAX),
    @TextOfScript NTEXT,
    @TextHash NVARCHAR(MAX),
    @OneTimeScript BIT,
    @EnteredBy NVARCHAR(MAX)
)
AS
INSERT INTO [{RepositorySchema}].ScriptRun (
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
SET @schemaId = (SELECT TOP 1 [schema_id] FROM sys.schemas WHERE [name] = '{RepositorySchema}')
IF NOT EXISTS(SELECT * FROM sys.procedures WHERE [name] = 'InsertScriptRunError' AND [schema_id] = @schemaId)
BEGIN
    DECLARE @sql VARCHAR(1000)
    SET @sql = 'CREATE PROCEDURE [{RepositorySchema}].InsertScriptRunError AS SELECT * FROM sysobjects'
    EXEC(@sql)
END
GO

ALTER PROCEDURE [{RepositorySchema}].InsertScriptRunError (
    @RepositoryPath NVARCHAR(MAX),
    @ScriptName NVARCHAR(MAX),
    @VersionName NVARCHAR(MAX),
    @TextOfScript NTEXT,
    @ErroneousPart NTEXT,
    @ErrorMessage NTEXT,
    @EnteredBy NVARCHAR(MAX)
)
AS
INSERT INTO [{RepositorySchema}].ScriptRunError (
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
SET @schemaId = (SELECT TOP 1 [schema_id] FROM sys.schemas WHERE [name] = '{RepositorySchema}')
IF NOT EXISTS(SELECT * FROM sys.procedures WHERE [name] = 'GetVersion' AND [schema_id] = @schemaId)
BEGIN
    DECLARE @sql VARCHAR(1000)
    SET @sql = 'CREATE PROCEDURE [{RepositorySchema}].GetVersion AS SELECT * FROM sysobjects'
    EXEC(@sql)
END
GO

ALTER PROCEDURE [{RepositorySchema}].GetVersion (
    @RepositoryPath NVARCHAR(MAX)
)
AS
SELECT TOP 1 VersionName
FROM [{RepositorySchema}].Version
WHERE RepositoryPath = @RepositoryPath
ORDER BY VersionId DESC
GO

DECLARE @schemaId INT
SET @schemaId = (SELECT TOP 1 [schema_id] FROM sys.schemas WHERE [name] = '{RepositorySchema}')
IF NOT EXISTS(SELECT * FROM sys.procedures WHERE [name] = 'InsertVersion' AND [schema_id] = @schemaId)
BEGIN
    DECLARE @sql VARCHAR(1000)
    SET @sql = 'CREATE PROCEDURE [{RepositorySchema}].InsertVersion AS SELECT * FROM sysobjects'
    EXEC(@sql)
END
GO

ALTER PROCEDURE [{RepositorySchema}].InsertVersion (
    @VersionName NVARCHAR(MAX),
    @RepositoryPath NVARCHAR(MAX),
    @EnteredBy NVARCHAR(MAX)
)
AS
INSERT INTO [{RepositorySchema}].Version (VersionName, RepositoryPath, DateCreated, EnteredBy)
VALUES (@VersionName, @RepositoryPath, GETDATE(), @EnteredBy)
SELECT CAST(SCOPE_IDENTITY() AS INT)
GO

DECLARE @schemaId INT
SET @schemaId = (SELECT TOP 1 [schema_id] FROM sys.schemas WHERE [name] = '{RepositorySchema}')
IF NOT EXISTS(SELECT * FROM sys.procedures WHERE [name] = 'HasScriptRunAlready' AND [schema_id] = @schemaId)
BEGIN
    DECLARE @sql VARCHAR(1000)
    SET @sql = 'CREATE PROCEDURE [{RepositorySchema}].HasScriptRunAlready AS SELECT * FROM sysobjects'
    EXEC(@sql)
END
GO

ALTER PROCEDURE [{RepositorySchema}].HasScriptRunAlready (
    @ScriptName NVARCHAR(MAX)
)
AS
SELECT COUNT(ScriptRunId)
FROM [{RepositorySchema}].ScriptRun
WHERE ScriptName = @ScriptName
GO

DECLARE @schemaId INT
SET @schemaId = (SELECT TOP 1 [schema_id] FROM sys.schemas WHERE [name] = '{RepositorySchema}')
IF NOT EXISTS(SELECT * FROM sys.procedures WHERE [name] = 'GetCurrentScriptHash' AND [schema_id] = @schemaId)
BEGIN
    DECLARE @sql VARCHAR(1000)
    SET @sql = 'CREATE PROCEDURE [{RepositorySchema}].GetCurrentScriptHash AS SELECT * FROM sysobjects'
    EXEC(@sql)
END
GO

ALTER PROCEDURE [{RepositorySchema}].GetCurrentScriptHash (
    @ScriptName NVARCHAR(MAX)
)
AS
SELECT TOP 1 TextHash
FROM [{RepositorySchema}].ScriptRun
WHERE ScriptName = @ScriptName
ORDER BY ScriptRunId DESC
GO