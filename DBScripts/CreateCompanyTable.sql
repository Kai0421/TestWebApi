IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Company]') AND type in (N'U'))
CREATE TABLE [dbo].[Company]
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
    CompanyName VARCHAR(255) NOT NULL,
    Exchange VARCHAR(100) Not NULL,
    Ticker VARCHAR(100) Not NULL,
    Isin VARCHAR(100) Not NULL,
    Website VARCHAR(255)
)
GO

