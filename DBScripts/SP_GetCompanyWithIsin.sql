DROP PROCEDURE IF EXISTS [dbo].[sp_GetCompanyWithIsin]
GO

CREATE PROCEDURE [dbo].[sp_GetCompanyWithIsin] 
(
    @Isin VARCHAR(100)
)
AS
BEGIN
    SELECT TOP 1 *
    FROM 
        dbo.Company
	Where
		dbo.Company.Isin = @Isin
END
