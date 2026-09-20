USE [NPI]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

/*

	sp_Verify_NPI is used to verify a providers npi.  
	It looks for a count(*).  If the count returns 1
	then it is a valid NPI assuming that you have put 
	in enough information to verify the npi.

	exec dbo.[sp_Verify_NPI]  @ProviderNPI, @LastName,@FirstName, @MiddleName, @StateName
	
	for example: exec dbo.[sp_Verify_NPI] 1003000282, 'BLAKEMORE', 'ROSIE', '', 'TN' 

	
	known bugs:  You have to prefix input parms with a wildcard.  see above.
	
	glaframboise, 23-Jul-2012

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

-- loading the npi table
 INSERT INTO [NPI].[dbo].[PROVIDER_NPI]
           ([NPI]
           ,[Last_Name]
           ,[First_Name]
           ,[Middle_Name]
           ,[STATE_NAME])           
select [NPI]
      ,ltrim(rtrim([Provider Last Name (Legal Name)]))
      ,ltrim(rtrim([Provider First Name]))
      ,ltrim(rtrim([Provider Middle Name] ))
      ,ltrim(rtrim([Provider Business Mailing Address State Name]))

*/

ALTER PROCEDURE [dbo].[sp_Verify_NPI]
	@ProviderNPI	bigint,
	@LastName		varchar(35),
	@FirstName		varchar(20),
	@MiddleName		varchar(20),
	@StateName		varchar(80)
AS  
BEGIN

	-- if count(*) returns 1 then NPI is good.
	SELECT COUNT(*)						-- returns 1 if found correctly.
	  FROM [NPI].[dbo].[PROVIDER_NPI]
     WHERE (NPI = @ProviderNPI)
	 AND (Last_Name   LIKE @LastName   or @LastName   like '')
       AND (FIRST_Name  LIKE @FirstName  or @FirstName  like '')
       AND (Middle_Name LIKE @MiddleName or @MiddleName like '')
       AND (state_name  LIKE @StateName  or @StateName  like '')   

END

GO


