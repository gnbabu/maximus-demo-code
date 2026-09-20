/****** Object:  StoredProcedure [dbo].[DCConv_Validate_SRC]    Script Date: 8/1/2016 3:13:13 PM ******/
IF object_id('[dbo].[DCConv_Validate_SRC]','P') is not null
DROP PROCEDURE [dbo].DCConv_Validate_SRC
GO
/****** Object:  StoredProcedure [dbo].[DCConv_Validate_SRC]    Script Date: 8/1/2016 3:13:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Richard Mays
-- Create date: 5/26/2016
-- Description:	Validates the data
-- =============================================
CREATE PROCEDURE [dbo].[DCConv_Validate_SRC] 
	-- Add the parameters for the stored procedure here
	@pin_src_database_name varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @Query VARCHAR(MAX), @Column VARCHAR(100), @Table VARCHAR(100)
	DECLARE @LkUp_Column varchar(100), @LkUp_Table varchar(100);
	DECLARE @rowCount int;
	DECLARE @tableName varchar(100);
	
	-- Reset the validation columns
	CREATE TABLE #DT_Tables (tableName varchar(100)) 
	SET @query = 'INSERT INTO #DT_Tables (tableName) SELECT Table_Name as tableName	FROM ' + @pin_src_database_name + '.INFORMATION_SCHEMA.TABLES WHERE Table_Name LIKE ''DT_%''';
	EXEC (@query);

	WHILE (SELECT count(*) FROM #DT_Tables) > 0
	BEGIN
	 SELECT @tableName = min(tableName) from #DT_Tables

	 SELECT @query = 'UPDATE ' + @pin_src_database_name + '.dbo.' + @tableName + ' SET EnableConversion = 1, ErrorCode = NULL, ErrorMessage = NULL'

	 EXEC (@query);

	 DELETE FROM #DT_Tables WHERE tableName = @tableName
	 IF (SELECT count(*) FROM #DT_Tables) = 0
	 BREAK
	 ELSE
	 CONTINUE

	END

	
	-- Augment the lookup table LKUP_PROVIDER_OWNERSHIP_TYPE_CODE
	SET @Query = 'IF NOT EXISTS (SELECT [P-OWNER-TY-CD] FROM [' + @pin_src_database_name + '].[dbo].[LKUP_PROVIDER_OWNERSHIP_TYPE_CODE] WHERE [P-OWNER-TY-CD] = ''D'') ';
	SET @Query = @Query + 'INSERT INTO [' + @pin_src_database_name + '].[dbo].[LKUP_PROVIDER_OWNERSHIP_TYPE_CODE] ([P-OWNER-TY-CD], [Long]) VALUES (''D'',  ''Non-profit Corp'')';
	EXEC (@Query);
	
	SET @Query = 'IF NOT EXISTS (SELECT [P-OWNER-TY-CD] FROM [' + @pin_src_database_name + '].[dbo].[LKUP_PROVIDER_OWNERSHIP_TYPE_CODE] WHERE [P-OWNER-TY-CD] = ''H'') ';
	SET @Query = @Query + 'INSERT INTO [' + @pin_src_database_name + '].[dbo].[LKUP_PROVIDER_OWNERSHIP_TYPE_CODE] ([P-OWNER-TY-CD], [Long]) VALUES (''H'', ''Public'');';
	EXEC (@Query);
	
	SET @Query = 'IF NOT EXISTS (SELECT [P-OWNER-TY-CD] FROM [' + @pin_src_database_name + '].[dbo].[LKUP_PROVIDER_OWNERSHIP_TYPE_CODE] WHERE [P-OWNER-TY-CD] = ''L'') ';
	SET @Query = @Query + 'INSERT INTO [' + @pin_src_database_name + '].[dbo].[LKUP_PROVIDER_OWNERSHIP_TYPE_CODE] ([P-OWNER-TY-CD], [Long]) VALUES (''L'',  ''Limited Liability Corporation'')';
	EXEC (@Query);
	
	

	-- Find the bad dates in the data
	SET @Query = 'DECLARE Col CURSOR FOR ';
	SET @Query = @Query + 'SELECT Table_Name, Column_Name ';
	SET @Query = @Query + 'FROM [' + @pin_src_database_name + '].INFORMATION_SCHEMA.COLUMNS ';
	SET @Query = @Query + 'WHERE Column_Name LIKE ''%-DT'' AND Table_Name LIKE ''DT_%'' AND '; 
	SET @Query = @Query + 'RTRIM(LTRIM(Column_Name)) <> ''G-AUD-DT'' AND ';  
	SET @Query = @Query + 'Column_Name IN (SELECT ELEMENT_TO_VALIDATE FROM DCConv_DataElementsToValidate) ';
	SET @Query = @Query + 'ORDER BY TABLE_NAME, ORDINAL_POSITION ';
	EXEC (@Query);

	OPEN Col
	FETCH NEXT FROM Col INTO @Table, @Column
	WHILE @@FETCH_STATUS = 0
	BEGIN
		PRINT @Table; PRINT @Column;

		SET @Query = 'SELECT * FROM [' + @pin_src_database_name + '].[INFORMATION_SCHEMA].[COLUMNS] WHERE Table_Name = ''' +  @Table + ''' AND Column_Name = ''P-SYS-ID''';
		EXEC(@Query)
		SET @rowCount = @@ROWCOUNT;
		IF (@rowCount > 0)
		BEGIN
			IF (CHARINDEX('PROVDR',@Table) > 0) 
			BEGIN
				SET @Query = 'UPDATE [' + @pin_src_database_name + '].[dbo].[' + @Table + '] SET [EnableConversion] = 0, [ErrorCode] = 1,';
				SET @Query = @Query + '[ErrorMessage] = CAST(tgt.[P-SYS-ID] AS varchar(10)) + ''|'' + tgt.[P-ID] + ''|' + @Column +  '|'' + tgt.[' + @Column + '] '; 
				SET @Query = @Query + 'FROM	 [' + @pin_src_database_name + '].[dbo].[' + @Table + '] tgt ';
				SET @Query = @Query + ' WHERE tgt.[P-REC-TY-CD] = ''P'' AND (ISNUMERIC(LEFT(tgt.[' + @Column + '],4)) = 0 OR (CAST(LEFT(tgt.[' + @Column + '], 4) AS int) < 1900 AND tgt.[' + @Column + '] <> ''1753-01-01'')';
				SET @Query = @Query + ' OR (CAST(LEFT(tgt.[' + @Column + '], 4) AS int) > 2100 AND tgt.[' + @Column + '] <> ''9999-12-31''))';
			END
			ELSE
			BEGIN
				SET @Query = 'UPDATE [' + @pin_src_database_name + '].[dbo].[' + @Table + '] SET [EnableConversion] = 0, [ErrorCode] = 1,';
				SET @Query = @Query + '[ErrorMessage] = CAST(tgt.[P-SYS-ID] AS varchar(10)) + ''|'' + prov.[P-ID] + ''|' + @Column +  '|'' + tgt.[' + @Column + '] '; 
				SET @Query = @Query + 'FROM	 [' + @pin_src_database_name + '].[dbo].[' + @Table + '] tgt ';
				SET @Query = @Query + ' INNER JOIN [' + @pin_src_database_name + '].[dbo].[DT_PROVDRTB_STG] prov ON prov.[P-SYS-ID] = tgt.[P-SYS-ID]';
				SET @Query = @Query + ' WHERE prov.[P-REC-TY-CD] = ''P'' AND (ISNUMERIC(LEFT(tgt.[' + @Column + '],4)) = 0 OR (CAST(LEFT(tgt.[' + @Column + '], 4) AS int) < 1900 AND tgt.[' + @Column + '] <> ''1753-01-01'')';
				SET @Query = @Query + ' OR (CAST(LEFT(tgt.[' + @Column + '], 4) AS int) > 2100 AND tgt.[' + @Column + '] <> ''9999-12-31''))';
			END
		END
		ELSE
		BEGIN
			IF (CHARINDEX('AFFIL',@Table) > 0) 
			BEGIN
				SET @Query = 'UPDATE [' + @pin_src_database_name + '].[dbo].[' + @Table + '] SET [EnableConversion] = 0, [ErrorCode] = 1,';
				SET @Query = @Query + '[ErrorMessage] = CAST(grp_prov.[P-SYS-ID] AS varchar(10)) + ''|'' + grp_prov.[P-ID] + ''|'' + CAST(mem_prov.[P-SYS-ID] AS varchar(10)) + ''|'' + mem_prov.[P-ID] + ''|' + @Column + '|'' + tgt.[' + @Column + '] '; 
				SET @Query = @Query + 'FROM	 [' + @pin_src_database_name + '].[dbo].[' + @Table + '] tgt ';
				SET @Query = @Query + ' INNER JOIN [' + @pin_src_database_name + '].[dbo].[DT_PROVDRTB_STG] grp_prov ON grp_prov.[P-SYS-ID] = tgt.[P-GROUP-SYS-ID]';
				SET @Query = @Query + ' INNER JOIN [' + @pin_src_database_name + '].[dbo].[DT_PROVDRTB_STG] mem_prov ON mem_prov.[P-SYS-ID] = tgt.[P-MEMBER-SYS-ID]';
				SET @Query = @Query + ' WHERE grp_prov.[P-REC-TY-CD] = ''P'' AND mem_prov.[P-REC-TY-CD] = ''P'' AND  (ISNUMERIC(LEFT(tgt.[' + @Column + '],4)) = 0 OR (CAST(LEFT(tgt.[' + @Column + '], 4) AS int) < 1900 AND tgt.[' + @Column + '] <> ''1753-01-01'')';
				SET @Query = @Query + ' OR (CAST(LEFT(tgt.[' + @Column + '], 4) AS int) > 2100 AND tgt.[' + @Column + '] <> ''9999-12-31''))';
			END
			ELSE
			BEGIN
				SET @Query = 'UPDATE [' + @pin_src_database_name + '].[dbo].[' + @Table + '] SET [EnableConversion] = 0, [ErrorCode] = 1,';
				SET @Query = @Query + '[ErrorMessage] = ''||' + @Column + '|'' + tgt.[' + @Column + '] '; 
				SET @Query = @Query + 'FROM	 [' + @pin_src_database_name + '].[dbo].[' + @Table + '] tgt ';
				SET @Query = @Query + ' WHERE (ISNUMERIC(LEFT(tgt.[' + @Column + '],4)) = 0 OR (CAST(LEFT(tgt.[' + @Column + '], 4) AS int) < 1900 AND tgt.[' + @Column + '] <> ''1753-01-01'')';
				SET @Query = @Query + ' OR (CAST(LEFT(tgt.[' + @Column + '], 4) AS int) > 2100 AND tgt.[' + @Column + '] <> ''9999-12-31''))';
			END
		END
				 
		PRINT @Query;
		EXEC (@Query)

		FETCH NEXT FROM Col INTO @Table, @Column
	END
	CLOSE Col
	DEALLOCATE Col

	-- Find any bad look up values in the tables
	SET @Query = 'DECLARE LkUp_Col CURSOR FOR ';
	SET @Query = @Query + 'SELECT Table_Name, Column_Name ';
	SET @Query = @Query + 'FROM [' + @pin_src_database_name + '].[INFORMATION_SCHEMA].[COLUMNS] ';
	SET @Query = @Query + 'WHERE Column_Name LIKE ''P-%'' AND Table_Name LIKE ''LKUP_%'' AND Column_Name <> ''P-SYS-ID'' AND ' ;
	SET @Query = @Query + 'Column_Name IN (SELECT ELEMENT_TO_VALIDATE FROM DCConv_DataElementsToValidate) ';
	SET @Query = @Query + 'ORDER BY TABLE_NAME, ORDINAL_POSITION';
	EXEC(@Query);

	OPEN LkUp_Col
	FETCH NEXT FROM LkUp_Col INTO @LkUp_Table, @LkUp_Column
	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF (@LkUp_Column <> 'P-LIC-VRFY-IND')
		BEGIN
			SET @Query = 'DECLARE Dt_Col CURSOR FOR ';
			SET @Query = @Query + 'SELECT Table_Name, Column_Name ';
			SET @Query = @Query + 'FROM ['+ @pin_src_database_name + '].[INFORMATION_SCHEMA].[COLUMNS] ';
			SET @Query = @Query + 'WHERE Column_Name = '''  + @LkUp_Column + ''' AND Table_Name LIKE ''DT_%'' ';
			SET @Query = @Query + 'ORDER BY TABLE_NAME, ORDINAL_POSITION';
			EXEC (@Query);
		
			OPEN Dt_Col
			FETCH NEXT FROM Dt_Col INTO @Table, @Column
			WHILE @@FETCH_STATUS = 0
			BEGIN
				EXEC('SELECT * FROM [' + @pin_src_database_name + '].[INFORMATION_SCHEMA].[COLUMNS] WHERE Table_Name = ''' +  @Table + ''' AND Column_Name = ''P-SYS-ID''')
				SET @rowCount = @@ROWCOUNT;
				IF (@rowCount > 0)
				BEGIN
					IF (CHARINDEX('PROVDR',@Table) > 0) 
					BEGIN
						SET @Query = 'UPDATE ['+ @pin_src_database_name + '].[dbo].[' + @Table + '] SET [EnableConversion] = 0, [ErrorCode] = 2,';
						SET @Query = @Query + '[ErrorMessage] = CAST(dt.[P-SYS-ID] AS varchar(10)) + ''|'' + dt.[P-ID] + ''|' + @Column +  '|'' + dt.[' + @Column + '] '; 
						SET @Query = @Query + 'FROM ['+ @pin_src_database_name + '].[dbo].[' + @Table + '] dt';
						SET @Query = @Query + ' LEFT OUTER JOIN ['+ @pin_src_database_name + '].[dbo].[' + @LkUp_Table + '] lkup ON lkup.[' + @LkUp_Column + '] = dt.[' + @Column + '] ';
						SET @Query = @Query + ' WHERE dt.[P-REC-TY-CD] = ''P'' AND  dt.[' + @Column + '] IS NOT NULL AND LEN(RTRIM(LTRIM(dt.[' + @Column + ']))) > 0 AND lkup.[' + @Column + '] IS NULL';
					END
					ELSE
					BEGIN
						SET @Query = 'UPDATE ['+ @pin_src_database_name + '].[dbo].[' + @Table + '] SET [EnableConversion] = 0, [ErrorCode] = 2,';
						SET @Query = @Query + '[ErrorMessage] = CAST(dt.[P-SYS-ID] AS varchar(10)) + ''|'' + prov.[P-ID] + ''|' + @Column +  '|'' + dt.[' + @Column + '] '; 
						SET @Query = @Query + 'FROM ['+ @pin_src_database_name + '].[dbo].[' + @Table + '] dt';
						SET @Query = @Query + ' INNER JOIN ['+ @pin_src_database_name + '].[dbo].[DT_PROVDRTB_STG] prov ON prov.[P-SYS-ID] = dt.[P-SYS-ID]';
						SET @Query = @Query + ' LEFT OUTER JOIN ['+ @pin_src_database_name + '].[dbo].[' + @LkUp_Table + '] lkup ON lkup.[' + @LkUp_Column + '] = dt.[' + @Column + '] ';
						SET @Query = @Query + ' WHERE prov.[P-REC-TY-CD] = ''P'' AND  dt.[' + @Column + '] IS NOT NULL AND LEN(RTRIM(LTRIM(dt.[' + @Column + ']))) > 0 AND lkup.[' + @Column + '] IS NULL';
					END
				END
				ELSE
				BEGIN
					IF (CHARINDEX('AFFIL',@Table) > 0) 
					BEGIN
						SET @Query = 'UPDATE [' + @pin_src_database_name + '].[dbo].[' + @Table + '] SET [EnableConversion] = 0, [ErrorCode] = 2,';
						SET @Query = @Query + '[ErrorMessage] = CAST(grp_prov.[P-SYS-ID] AS varchar(10)) + ''|'' + grp_prov.[P-ID] + ''|'' + CAST(mem_prov.[P-SYS-ID] AS varchar(10)) + ''|'' + mem_prov.[P-ID] + ''|' + @Column + '|'' + dt.[' + @Column + '] '; 
						SET @Query = @Query + 'FROM ['+ @pin_src_database_name + '].[dbo].[' + @Table + '] dt';
						SET @Query = @Query + ' INNER JOIN [' + @pin_src_database_name + '].[dbo].[DT_PROVDRTB_STG] grp_prov ON grp_prov.[P-SYS-ID] = dt.[P-GROUP-SYS-ID]';
						SET @Query = @Query + ' INNER JOIN [' + @pin_src_database_name + '].[dbo].[DT_PROVDRTB_STG] mem_prov ON mem_prov.[P-SYS-ID] = dt.[P-MEMBER-SYS-ID]';
						SET @Query = @Query + ' LEFT OUTER JOIN ['+ @pin_src_database_name + '].[dbo].[' + @LkUp_Table + '] lkup ON lkup.[' + @LkUp_Column + '] = dt.[' + @Column + '] ';
						SET @Query = @Query + ' WHERE grp_prov.[P-REC-TY-CD] = ''P'' AND mem_prov.[P-REC-TY-CD] = ''P'' AND  dt.[' + @Column + '] IS NOT NULL AND LEN(RTRIM(LTRIM(dt.[' + @Column + ']))) > 0 AND lkup.[' + @Column + '] IS NULL';
					END
					ELSE
					BEGIN
						SET @Query = 'UPDATE [' + @pin_src_database_name + '].[dbo].[' + @Table + '] SET [EnableConversion] = 0, [ErrorCode] = 2,';
						SET @Query = @Query + '[ErrorMessage] = ''||' + @Column + '|'' + dt.[' + @Column + '] '; 
						SET @Query = @Query + 'FROM ['+ @pin_src_database_name + '].[dbo].[' + @Table + '] dt';
						SET @Query = @Query + ' LEFT OUTER JOIN ['+ @pin_src_database_name + '].[dbo].[' + @LkUp_Table + '] lkup ON lkup.[' + @LkUp_Column + '] = dt.[' + @Column + '] ';
						SET @Query = @Query + ' WHERE dt.[' + @Column + '] IS NOT NULL AND LEN(RTRIM(LTRIM(dt.[' + @Column + ']))) > 0 AND lkup.[' + @Column + '] IS NULL';				
					END
				END
				PRINT @Query;
				EXEC(@Query);

				FETCH NEXT FROM Dt_Col INTO @Table, @Column
			END
			CLOSE Dt_Col
			DEALLOCATE Dt_Col
		END
		FETCH NEXT FROM LkUp_Col INTO @LkUp_Table, @LkUp_Column
	END
	CLOSE LkUp_Col
	DEALLOCATE LkUp_Col

	UPDATE [SRC_EnrollmentUpdt] SET [EnableConversion] = 0, 
	[ErrorCode] = 3,[ErrorMessage] = CAST(dt.[P-SYS-ID] AS varchar(10)) + '|' + prov.[P-ID] + '|[P-TY-CD]|'  + dt.[P-TY-CD]
	FROM SRC_EnrollmentUpdt dt
	INNER JOIN SRC_ProviderUpdt prov ON prov.[P-SYS-ID] = dt.[P-SYS-ID]
	WHERE dt.[P-TY-CD] IN ('U01', 'U02', 'U03', 'U04', 'U05', 'U06', 'U07', 'U08', 'V02', 'W03', 'Y00');
	

END


GO
