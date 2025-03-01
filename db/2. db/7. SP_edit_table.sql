CREATE PROCEDURE EditTable
    @Id INT,
    @Name NVARCHAR(255),
    @Type NVARCHAR(MAX),
    @IsActive bool,
    @IsDeleted bool

AS
BEGIN
    DECLARE @sql NVARCHAR(MAX);

    -- Verifica que exista el nombre de la columna
    SELECT 1
    FROM sys.columns
    WHERE object_id = OBJECT_ID(Select 1 from Form where Id)
    AND name = 'nombre_de_la_columna';

    -- SET @sql = 'SELECT * FROM ' + @Name + ' WHERE ' + @Parameters;

    -- EXEC sp_executesql @sql;
        
END
