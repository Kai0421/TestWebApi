DROP PROCEDURE IF EXISTS [dbo].[sp_GetAllCompany]
GO

CREATE PROCEDURE [dbo].[sp_GetAllCompany]
AS
BEGIN
    SELECT *            
    FROM 
        dbo.Company
END