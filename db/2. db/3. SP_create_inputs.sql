CREATE PROCEDURE SP_AddDataInputs
    @Columns NVARCHAR(MAX),
    @Values NVARCHAR(MAX)
AS
BEGIN
    DECLARE @SQL NVARCHAR(MAX);

    SET @SQL = 'INSERT INTO Inputs (' + @Columns + ') VALUES (' + @Values + ');  SELECT SCOPE_IDENTITY() AS ID;';

    EXEC sp_executesql @SQL;
END
