USE [TestDB]
GO

/****** Object:  StoredProcedure [dbo].[sp_UpdateCompany]    Script Date: 10/24/2021 5:46:44 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_InsertNewCompany] 
(
    @CompanyName VARCHAR(255),
    @Exchange VARCHAR(100),
    @Ticker VARCHAR(100),
    @Isin VARCHAR(100),
    @Website VARCHAR(255)
)
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION
            INSERT INTO Company (CompanyName, Exchange, Ticker, Isin, Website) VALUES (@CompanyName, @Exchange, @Ticker, @Isin, @Website)
        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        THROW
    END CATCH
END

GO


