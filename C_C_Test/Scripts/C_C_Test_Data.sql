USE [TEST1]
GO

/****** Object:  Table [dbo].[C_C_Test_Data]    Script Date: 15/07/2024 13:09:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[C_C_Test_Data](
	[MPAN] [numeric](13, 0) NOT NULL,
	[MeterSerial] [varchar](10) NOT NULL,
	[DateOfInstallation] [date] NOT NULL,
	[AddressLine1] [varchar](40) NULL,
	[PostCode] [varchar](10) NULL
) ON [PRIMARY]
GO


