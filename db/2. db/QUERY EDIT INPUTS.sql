declare @NameTable NVARCHAR(200);
declare @InputNameNew NVARCHAR(200) = 'Edad';
declare @InputId INT = 12;
declare @FormId INT = 44;
declare @FormType NVARCHAR(30) = 'INT';
DECLARE @FullName NVARCHAR(500);
DECLARE @InputName  NVARCHAR(255);
DECLARE @IsActive BIT;
DECLARE @IsDeleted BIT;

DECLARE @SQLChangeType NVARCHAR(MAX);
DECLARE @SQLChangeValue NVARCHAR(MAX);
DECLARE @SchemaID INT;
SELECT @SchemaID = SCHEMA_ID('dbo');

Select @NameTable = LTRIM(RTRIM(FormName))  from Form where ID = @FormId
select @InputName = LTRIM(RTRIM(InputsName)) from Inputs WHERE IDForm = @FormId AND ID = @InputId


print '@FullName';
print @FullName;

-- [main].[dbo].[Perros]

IF EXISTS (
    SELECT 1
    FROM sys.columns
    WHERE object_id = OBJECT_ID(LTRIM(RTRIM(@NameTable)))
    AND name = @InputNameNew
)
BEGIN

    -- ACTUALIZACION DE TIPO DE CAMPO
    SELECT @FullName = '[main].' + QUOTENAME(SCHEMA_NAME(@SchemaID)) + '.' + QUOTENAME(@NameTable);
    SET @SQLChangeType = N'ALTER TABLE ' + @FullName + N' ALTER COLUMN ' + QUOTENAME(@InputName) + N' '+ @FormType + ';';
    EXEC sp_executesql @SQLChangeType;


   
    PRINT @SQLChangeValue;
    -- EXEC sp_executesql @SQLChangeType;


    PRINT 'La columna existe'; -- Se mostrará este mensaje si la columna existe
END
ELSE
BEGIN -- La columna no existe
    -- entra a proceso de cambio de nombre de campo
    SELECT @FullName = QUOTENAME(SCHEMA_NAME(@SchemaID)) + '.' + QUOTENAME(@NameTable) + '.' + QUOTENAME(@InputName);

    UPDATE Inputs SET InputsName = @InputNameNew WHERE IDForm = @FormId AND ID = @InputId;

    EXEC sp_rename @FullName, @InputNameNew;
END;
