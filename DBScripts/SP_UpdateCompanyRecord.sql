USE [TestDB]
GO

DROP PROCEDURE IF EXISTS [dbo].[sp_UpdateCompanyRecord]
GO

CREATE PROCEDURE [dbo].[sp_UpdateCompanyRecord] 
(
    @CompanyName VARCHAR(255),
	@Exchange varchar(100),
	@Ticker varchar(100),
	@Isin varchar(100),
	@Website varchar(255)
)
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION
			Update Company
			Set CompanyName = @CompanyName, Exchange = @Exchange, Ticker = @Ticker, @Isin = isin, Website = @Website
            Where Isin = @Isin
        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        THROW
    END CATCH
END

GO