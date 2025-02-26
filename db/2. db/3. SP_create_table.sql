CREATE PROCEDURE CreateTable 
    @Name NVARCHAR(255),
    @Columns NVARCHAR(MAX)
AS
BEGIN
    DECLARE @sql NVARCHAR(MAX);
    
    SET @sql = 'CREATE TABLE ' + @Name + ' (' + @Columns + ')';
    
    EXEC sp_executesql @sql;
      
END
