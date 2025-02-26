CREATE PROCEDURE AddDataInputs
    @Columns NVARCHAR(MAX),
    @Values NVARCHAR(MAX)
AS
BEGIN
    DECLARE @SQL NVARCHAR(MAX);

    SET @SQL = 'INSERT INTO Inputs (' + @Columns + ') VALUES (' + @Values + ')';

    EXEC sp_executesql @SQL;
END
