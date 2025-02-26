CREATE PROCEDURE AddDataInTable
    @TableName NVARCHAR(MAX),
    @Columns NVARCHAR(MAX),
    @Values NVARCHAR(MAX)
AS
BEGIN
    DECLARE @SQL NVARCHAR(MAX);

    SET @SQL = 'INSERT INTO ' + @TableName + ' (' + @Columns + ') VALUES (' + @Values + ')';

    EXEC sp_executesql @SQL;
END
