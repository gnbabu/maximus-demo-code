/****** Object:  StoredProcedure [dbo].[fn_ConvertProviderEntityTypeToPDMS]    Script Date: 8/1/2016 3:13:13 PM ******/
IF object_id('[dbo].[fn_ConvertProviderEntityTypeToPDMS]','FN') is not null
DROP FUNCTION [dbo].fn_ConvertProviderEntityTypeToPDMS
GO

/****** Object:  UserDefinedFunction [dbo].[fn_ConvertProviderEntityTypeToPDMS]    Script Date: 8/1/2016 3:13:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Richard Mays
-- Create date: 5/11/2016
-- Description:	Analyzes the DC data to get the Provider Entity Type
-- The provider entity type is also know as the provider category
-- =============================================
CREATE FUNCTION [dbo].[fn_ConvertProviderEntityTypeToPDMS] 
(
	-- Add the parameters for the function here
	@pin_provider_sys_id int,
	@pin_conv_data_export_date datetime
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result int
	DECLARE @ssn varchar(9);
	DECLARE @provider_type varchar(3);
	DECLARE @provider_name_org_ind varchar(1);
	DECLARE @enrollment_status varchar(2);
	DECLARE @hasMembers bit;

	SELECT @ssn = RTRIM(LTRIM([P-SSN-NUM])), 
		@provider_name_org_ind = [P-NAM-ORG-IND],
		@hasMembers = CASE WHEN [P-INDIV-GRP-CD] IN ('G','B') THEN 1 ELSE 0 END
	FROM SRC_Providers 
	WHERE [P-SYS-ID] = @pin_provider_sys_id;

	SELECT @enrollment_status = [P-ENROL-STAT-TY-CD],
		   @provider_type = [P-TY-CD]
	FROM SRC_Enrollments
	WHERE [P-SYS-ID] = @pin_provider_sys_id;

	
--	SELECT @hasMembers = 
		--CASE WHEN 
		--	EXISTS(SELECT 1 
		--	FROM SRC_ProviderAffiliates 
		--	WHERE [P-GROUP-SYS-ID] = @pin_provider_sys_id AND 
		--	[P-MEMBER-SYS-ID] IS NOT NULL AND
		--	dbo.fn_ConvertDCDateToPDMS([P-AFFL-END-DT]) > @pin_conv_data_export_date) 
		--THEN 1 ELSE 0 END;

	IF @provider_type IN ('A01', 'K01') AND @enrollment_status <> '80'
		SET @Result = 2;

	IF @provider_type IN ('S00', 'A00', 'A02', 'A04', 'K00', 'K02', 'M00', 'N00', 'N01', 'S01') AND @enrollment_status <> '80'
	BEGIN
		IF @hasMembers = 1
			SET @Result = 2;
		ELSE
			SET @Result = 1;
	END

	IF @provider_type IN ('A05','B00', 'B01', 'C00', 'D00', 'D01', 'D02', 'D03', 'D04', 'D05', 'E00', 'F00', 'G00', 'I00', 'J00', 'J01', 'J02', 'L00', 'L01', 'M01', 'P00', 'P01', 'P02', 'Q01', 'Q02', 'T00', 'T01', 'V00', 'V01', 'W03', 'X00', 'X01', 'X02', 'X03', 'X04', 'X05', 'X06','Z00', 'Z01', 'Z02') AND @enrollment_status <> '80'
		SET @Result = 3;

	IF @provider_type IN ('H00', 'H01', 'H02', 'H03') AND @enrollment_status <> '80'
		SET @Result = 5;

	IF @provider_type = 'W04' AND @enrollment_status <> '80'
		SET @Result = 3;

	IF @provider_type = 'W02' AND @enrollment_status <> '80'
		SET @Result = 3;

	IF @provider_type = 'X01' AND @enrollment_status <> '80'
	BEGIN
		IF @provider_name_org_ind = 'Y'
			SET @Result = 3;
		ELSE
			SET @Result = 1; 
	END

	IF @enrollment_status = '80'
	BEGIN
		IF @provider_name_org_ind = 'Y'
			SET @Result = 3;
		ELSE
			SET @Result = 1;
	END

	IF @provider_type IN ('R01', 'R02') 
	BEGIN
		IF @provider_type = 'R02'
			SET @Result = 3;
		ELSE
			SET @Result = 1; 
	END

	IF @provider_type = 'W01' AND @enrollment_status <> '80'
	BEGIN
		IF @provider_name_org_ind = 'Y'
			SET @Result = 3;
		ELSE
			SET @Result = 1; 
	END
		
	--IF @enrollment_status = '52'
	--BEGIN
	--	IF @provider_name_org_ind = 'Y'
	--		SET @Result = 3;
	--	ELSE
	--		SET @Result = 1;
	--END




	RETURN @Result

END

GO
