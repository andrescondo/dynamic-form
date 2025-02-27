CREATE PROCEDURE AddDataInTable
    @TableName NVARCHAR(MAX),
    @Columns NVARCHAR(MAX),
    @Values NVARCHAR(MAX)
AS
BEGIN
    DECLARE @SQL NVARCHAR(MAX);

    SET @SQL = 'INSERT INTO ' + @TableName + ' (' + @Columns + ') VALUES (''' + @Values + ''');  SELECT SCOPE_IDENTITY() AS ID;';

    EXEC sp_executesql @SQL;
END
