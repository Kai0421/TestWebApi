DROP PROCEDURE IF EXISTS [dbo].[sp_GetCompanyWithId]
GO

CREATE PROCEDURE [dbo].[sp_GetCompanyWithId] 
(
    @Id int
)
AS
BEGIN
    SELECT TOP 1 *
    FROM 
        dbo.Company
	Where
		dbo.Company.Id = @Id
END
