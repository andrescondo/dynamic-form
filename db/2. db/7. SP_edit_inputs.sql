CREATE PROCEDURE SP_EditInputs
    @InputNameNew NVARCHAR(200) = 'Edades',
    @InputId INT = 12,
    @FormId INT = 44,
    @FormType NVARCHAR(30) = 'INT',
    @IsActive BIT,
    @IsDeleted BIT

AS
BEGIN
    --VARIABLES INTERNAS
    DECLARE @FullName NVARCHAR(500);
    declare @NameTable NVARCHAR(200);
    DECLARE @InputName  NVARCHAR(255);
    DECLARE @SchemaID INT;
    DECLARE @newType NVARCHAR(MAX);

    DECLARE @SQLChangeType NVARCHAR(MAX);


    SELECT @SchemaID = SCHEMA_ID('dbo');
    Select @NameTable = LTRIM(RTRIM(FormName))  from Form where ID = @FormId
    select @InputName = LTRIM(RTRIM(InputsName)) from Inputs WHERE IDForm = @FormId AND ID = @InputId

    IF EXISTS (
        SELECT 1
        FROM sys.columns
        WHERE object_id = OBJECT_ID(LTRIM(RTRIM(@NameTable)))
        AND name = @InputNameNew
    )
    BEGIN -- La columna existe

        -- ACTUALIZACION DE TIPO DE CAMPO - tabla buscada
        SELECT @FullName = '[main].' + QUOTENAME(SCHEMA_NAME(@SchemaID)) + '.' + QUOTENAME(@NameTable);
        SET @SQLChangeType = N'ALTER TABLE ' + @FullName + N' ALTER COLUMN ' + QUOTENAME(@InputName) + N' '+ @FormType + ';';
        EXEC sp_executesql @SQLChangeType;

    END
    ELSE
    BEGIN -- La columna no existe

        -- entra a proceso de cambio de nombre de campo
        SELECT @FullName = QUOTENAME(SCHEMA_NAME(@SchemaID)) + '.' + QUOTENAME(@NameTable) + '.' + QUOTENAME(@InputName);

        -- ACTUALIZAR tabla inputs con el nuevo nombre
        UPDATE Inputs SET InputsName = @InputNameNew WHERE IDForm = @FormId AND ID = @InputId;

        -- actualizar el nombre de la tabla
        EXEC sp_rename @FullName, @InputNameNew;
    END;

    --actualizar el VALOR EN INPUTS
    SET @newType = 
    CASE @FormType
        WHEN 'INT' THEN 'number'  
        WHEN 'DATETIME' THEN 'date'
        WHEN 'NVARCHAR(MAX)' THEN 'text' 
    END;

    UPDATE Inputs
    SET InputsType = @newType, IsActive = @IsActive, IsDeleted = @IsDeleted
    WHERE ID = @InputId and IDForm = @FormId
        
END
