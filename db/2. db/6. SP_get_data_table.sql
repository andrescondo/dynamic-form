CREATE PROCEDURE SP_SearchTable
    @Name NVARCHAR(255),
    @Parameters NVARCHAR(MAX)
AS
BEGIN
    DECLARE @sql NVARCHAR(MAX);

    IF @Parameters = ''
    BEGIN
        SET @sql = 'SELECT * FROM ' + @Name + ' ; ';
    END
    ELSE
    BEGIN
        SET @sql = 'SELECT * FROM ' + @Name + ' WHERE ' + @Parameters;
    END;

    EXEC sp_executesql @sql;
        
END
