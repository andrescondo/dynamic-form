CREATE PROCEDURE SearchTable
    @Name NVARCHAR(255),
    @Parameters NVARCHAR(MAX)
AS
BEGIN
    DECLARE @sql NVARCHAR(MAX);

    SET @sql = 'SELECT * FROM ' + @Name + ' WHERE ' + @Parameters;

    EXEC sp_executesql @sql;
        
END
