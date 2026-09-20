USE [NPI]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

/*
	create the provider_npi table in the NPI database
	glaframboise - 24-Jul-2012

*/
-- building the npi table:
 drop table [PROVIDER_NPI]
 go

CREATE TABLE [NPI].[dbo].[PROVIDER_NPI](
	NPI  				[bigint] NOT NULL,
	Last_Name 			[varchar](35) NULL,
	First_Name 		[varchar](20) NULL,
	Middle_Name 		[varchar](20) NULL,
	STATE_NAME			[varchar](80) NULL,
	CONSTRAINT PROVIDER_NPI_PK PRIMARY KEY CLUSTERED (NPI)	
) 
go

