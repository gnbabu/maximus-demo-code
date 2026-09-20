	DECLARE @Query VARCHAR(MAX), @Column VARCHAR(100), @Table VARCHAR(100)
	DECLARE @LkUp_Column varchar(100), @LkUp_Table varchar(100);

	CREATE TABLE #Results(Validation_Type varchar(30), Provider_Sys_ID varchar(20), Provider_ID varchar(20), Group_Provider_ID varchar(20), MemberProviderID varchar(20), Table_Name VARCHAR(100), Column_Name VARCHAR(100), Bad_Value varchar(100))

	---- Find the bad dates in the data
	DECLARE Col CURSOR FOR
	SELECT Table_Name, Column_Name
	FROM INFORMATION_SCHEMA.COLUMNS
	WHERE Column_Name LIKE '%-DT' AND Table_Name LIKE 'DT_%'
	ORDER BY TABLE_NAME, ORDINAL_POSITION

	OPEN Col
	FETCH NEXT FROM Col INTO @Table, @Column
	WHILE @@FETCH_STATUS = 0
	BEGIN
		PRINT @Table; PRINT @Column;
		IF (EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE Table_Name = @Table AND Column_Name = 'P-SYS-ID'))
		BEGIN
			SET @Query = 'INSERT INTO #Results SELECT DISTINCT ''INVALID DATE'' AS Validation_Type, tgt.[P-SYS-ID] AS Provider_Sys_ID, prov.[P-ID] AS Provider_ID, '''' AS Group_Provider_ID, '''' AS Member_Provider_ID, ''' + @Table + ''' AS Table_Name, ''' + @Column + ''' AS Column_Name, tgt.[' + @Column + '] AS Bad_Value FROM [' + @Table + '] tgt';
			SET @Query = @Query + ' INNER JOIN dbo.[DT_PROVDRTB_STG] prov ON prov.[P-SYS-ID] = tgt.[P-SYS-ID]';
			SET @Query = @Query + ' WHERE prov.[P-REC-TY-CD] = ''P'' AND (ISNUMERIC(LEFT(tgt.[' + @Column + '],4)) = 0 OR (CAST(LEFT(tgt.[' + @Column + '], 4) AS int) < 1900 AND tgt.[' + @Column + '] <> ''1753-01-01'')';
			SET @Query = @Query + ' OR (CAST(LEFT(tgt.[' + @Column + '], 4) AS int) > 2100 AND tgt.[' + @Column + '] <> ''9999-12-31''))';
		END
		ELSE
		BEGIN
			IF (CHARINDEX('AFFIL',@Table) > 0) 
			BEGIN
				SET @Query = 'INSERT INTO #Results SELECT DISTINCT ''INVALID DATE'' AS Validation_Type, tgt.[P-GROUP-SYS-ID] AS Provider_Sys_ID, '''' AS Provider_ID, grp_prov.[P-ID] AS Group_Provider_ID, mem_prov.[P-ID] AS Member_Provider_ID,''' + @Table + ''' AS Table_Name, ''' + @Column + ''' AS Column_Name, tgt.[' + @Column + '] AS Bad_Value FROM [' + @Table + '] tgt';
				SET @Query = @Query + ' INNER JOIN dbo.[DT_PROVDRTB_STG] grp_prov ON grp_prov.[P-SYS-ID] = tgt.[P-GROUP-SYS-ID]';
				SET @Query = @Query + ' INNER JOIN dbo.[DT_PROVDRTB_STG] mem_prov ON mem_prov.[P-SYS-ID] = tgt.[P-MEMBER-SYS-ID]';
				SET @Query = @Query + ' WHERE grp_prov.[P-REC-TY-CD] = ''P'' AND mem_prov.[P-REC-TY-CD] = ''P'' AND  (ISNUMERIC(LEFT(tgt.[' + @Column + '],4)) = 0 OR (CAST(LEFT(tgt.[' + @Column + '], 4) AS int) < 1900 AND tgt.[' + @Column + '] <> ''1753-01-01'')';
				SET @Query = @Query + ' OR (CAST(LEFT(tgt.[' + @Column + '], 4) AS int) > 2100 AND tgt.[' + @Column + '] <> ''9999-12-31''))';
			END
			ELSE
			BEGIN
				SET @Query = 'INSERT INTO #Results SELECT DISTINCT ''INVALID DATE'' AS Validation_Type, ''No Sys ID'' AS Provider_Sys_ID, ''No Provider ID'' AS Provider_ID, '''' AS Group_Provider_ID, '''' AS Member_Provider_ID, ''' + @Table + ''' AS Table_Name, ''' + @Column + ''' AS Column_Name, [' + @Column + '] AS Bad_Value FROM [' + @Table + ']';
				SET @Query = @Query + ' WHERE ISNUMERIC(LEFT([' + @Column + '],4)) = 0 OR (CAST(LEFT([' + @Column + '], 4) AS int) < 1900 AND [' + @Column + '] <> ''1753-01-01'')';
				SET @Query = @Query + ' OR ( CAST(LEFT([' + @Column + '], 4) AS int) > 2100 AND [' + @Column + '] <> ''9999-12-31'')';
			END
		END
				 
		PRINT @Query;
		EXEC (@Query)

		FETCH NEXT FROM Col INTO @Table, @Column
	END
	CLOSE Col
	DEALLOCATE Col

	-- Find any bad look up values in the tables
	DECLARE LkUp_Col CURSOR FOR
	SELECT Table_Name, Column_Name
	FROM INFORMATION_SCHEMA.COLUMNS
	WHERE Column_Name LIKE 'P-%' AND Table_Name LIKE 'LKUP_%' AND Column_Name <> 'P-SYS-ID'
	ORDER BY TABLE_NAME, ORDINAL_POSITION

	OPEN LkUp_Col
	FETCH NEXT FROM LkUp_Col INTO @LkUp_Table, @LkUp_Column
	WHILE @@FETCH_STATUS = 0
	BEGIN
		DECLARE Dt_Col CURSOR FOR
		SELECT Table_Name, Column_Name
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE Column_Name = @LkUp_Column AND Table_Name LIKE 'DT_%'
		ORDER BY TABLE_NAME, ORDINAL_POSITION

		OPEN Dt_Col
		FETCH NEXT FROM Dt_Col INTO @Table, @Column
		WHILE @@FETCH_STATUS = 0
		BEGIN
			SET @Table = RTRIM(LTRIM(@Table));
			SET @Column = RTRIM(LTRIM(@Column));
		
			IF (EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE Table_Name = @Table AND Column_Name = 'P-SYS-ID'))
			BEGIN
				SET @Query = 'INSERT INTO #Results SELECT DISTINCT ''INVALID REF VALUE'' AS Validation_Type, dt.[P-SYS-ID] AS Provider_Sys_ID, prov.[P-ID] AS Provider_ID, '''' AS Group_Provider_ID, '''' AS Member_Provider_ID, ''' + @Table + ''' AS Table_Name, ''' + @Column + ''' AS Column_Name, dt.[' + @Column + '] AS Bad_Value FROM [' + @Table + '] dt';
				SET @Query = @Query + ' INNER JOIN dbo.[DT_PROVDRTB_STG] prov ON prov.[P-SYS-ID]=dt.[P-SYS-ID] '
				SET @Query = @Query + ' LEFT OUTER JOIN [' + @LkUp_Table + '] lkup ON lkup.[' + @LkUp_Column + '] = dt.[' + @Column + '] ';
				SET @Query = @Query + ' WHERE prov.[P-REC-TY-CD] = ''P'' AND  dt.[' + @Column + '] IS NOT NULL AND LEN(RTRIM(LTRIM(dt.[' + @Column + ']))) > 0 AND lkup.[' + @Column + '] IS NULL';
			END
			ELSE
			BEGIN
				IF (CHARINDEX('AFFIL',@Table) > 0) 
				BEGIN
					SET @Query = 'INSERT INTO #Results SELECT DISTINCT ''INVALID REF VALUE'' AS Validation_Type, '''' AS Provider_Sys_ID, '''' AS Provider_ID, '''' AS Group_Provider_ID, '''' AS Member_Provider_ID, ''' + @Table + ''' AS Table_Name, ''' + @Column + ''' AS Column_Name, dt.[' + @Column + '] AS Bad_Value FROM [' + @Table + '] dt';
					SET @Query = @Query + ' INNER JOIN dbo.[DT_PROVDRTB_STG] grp_prov ON grp_prov.[P-SYS-ID]=dt.[P-GROUP-SYS-ID] '
					SET @Query = @Query + ' INNER JOIN dbo.[DT_PROVDRTB_STG] mem_prov ON mem_prov.[P-SYS-ID]=dt.[P-MEMBER-SYS-ID] '
					SET @Query = @Query + ' LEFT OUTER JOIN [' + @LkUp_Table + '] lkup ON lkup.[' + @LkUp_Column + '] = dt.[' + @Column + '] ';
					SET @Query = @Query + ' WHERE grp_prov.[P-REC-TY-CD] = ''P'' AND mem_prov.[P-REC-TY-CD] = ''P'' AND  dt.[' + @Column + '] IS NOT NULL AND LEN(RTRIM(LTRIM(dt.[' + @Column + ']))) > 0 AND lkup.[' + @Column + '] IS NULL';
				END
				ELSE
				BEGIN
					SET @Query = 'INSERT INTO #Results SELECT DISTINCT ''INVALID REF VALUE'' AS Validation_Type, ''No Sys ID'' AS Provider_Sys_ID, ''No Provider ID'' AS Provider_ID,'''' AS Group_Provider_ID, '''' AS Member_Provider_ID, ''' + @Table + ''' AS Table_Name, ''' + @Column + ''' AS Column_Name, dt.[' + @Column + '] AS Bad_Value FROM [' + @Table + '] dt';
					SET @Query = @Query + ' LEFT OUTER JOIN [' + @LkUp_Table + '] lkup ON lkup.[' + @LkUp_Column + '] = dt.[' + @Column + '] ';
					SET @Query = @Query + ' WHERE dt.[' + @Column + '] IS NOT NULL AND LEN(RTRIM(LTRIM(dt.[' + @Column + ']))) > 0 AND lkup.[' + @Column + '] IS NULL';
				END
			END
			PRINT @Query;
			EXEC(@Query);

			FETCH NEXT FROM Dt_Col INTO @Table, @Column
		END
		CLOSE Dt_Col
		DEALLOCATE Dt_Col
		FETCH NEXT FROM LkUp_Col INTO @LkUp_Table, @LkUp_Column
	END
	CLOSE LkUp_Col
	DEALLOCATE LkUp_Col

	-- Find empty required fields in the data
	DECLARE ColR CURSOR FOR
	SELECT TableName, ColumnName
	FROM dbo.[META_MMISRequiredFields]
	ORDER BY TABLENAME

	OPEN ColR
	FETCH NEXT FROM ColR INTO @Table, @Column
	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @Table = 'DT_' + RTRIM(LTRIM(@Table)) + '_STG';
		SET @Column = RTRIM(LTRIM(@Column));
		IF (EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE Table_Name = @Table AND Column_Name = 'P-SYS-ID'))
		BEGIN
			SET @Query = 'INSERT INTO #Results SELECT ''MMIS REQ NOT FOUND'' AS Validation_Type, dt.[P-SYS-ID] AS Provider_Sys_ID, prov.[P-ID] AS Provider_ID,'''' AS Group_Provider_ID, '''' AS Member_Provider_ID, ''' + @Table + ''' AS Table_Name, ''' + @Column + ''' AS Column_Name, dt.[' + @Column + '] AS Bad_Value FROM [' + @Table + '] dt';
			SET @Query = @Query + ' INNER JOIN dbo.[DT_PROVDRTB_STG] prov ON prov.[P-SYS-ID]=dt.[P-SYS-ID] '
			SET @Query = @Query + ' WHERE prov.[P-REC-TY-CD] = ''P'' AND LEN(RTRIM(LTRIM(dt.[' + @Column + ']))) = 0 '
		END
		ELSE 		 
		BEGIN 
			IF (CHARINDEX('AFFIL',@Table) > 0) 
			BEGIN
				SET @Query = 'INSERT INTO #Results SELECT ''MMIS REQ NOT FOUND'' AS Validation_Type, dt.[P-SYS-ID] AS Provider_Sys_ID, '''' AS Provider_ID,grp_prov.[P-ID] AS Group_Provider_ID, mem_prov.[P-ID] AS Member_Provider_ID, ''' + @Table + ''' AS Table_Name, ''' + @Column + ''' AS Column_Name, [' + @Column + '] AS Bad_Value FROM [' + @Table + '] dt';
				SET @Query = @Query + ' INNER JOIN dbo.[DT_PROVDRTB_STG] grp_prov ON grp_prov.[P-SYS-ID]=dt.[P-GROUP-SYS-ID] '
				SET @Query = @Query + ' INNER JOIN dbo.[DT_PROVDRTB_STG] mem_prov ON mem_prov.[P-SYS-ID]=dt.[P-MEMBER-SYS-ID] '
				SET @Query = @Query + ' WHERE grp_prov.[P-REC-TY-CD] = ''P'' AND mem_prov.[P-REC-TY-CD] = ''P'' AND LEN(RTRIM(LTRIM(dt.[' + @Column + ']))) = 0 '
			END
			ELSE
			BEGIN
				SET @Query = 'INSERT INTO #Results SELECT ''MMIS REQ NOT FOUND'' AS Validation_Type, ''None'' AS Provider_Sys_ID, ''No Provider ID'' AS Provider_ID,'''' AS Group_Provider_ID, '''' AS Member_Provider_ID, ''' + @Table + ''' AS Table_Name, ''' + @Column + ''' AS Column_Name, [' + @Column + '] AS Bad_Value FROM [' + @Table + '] dt';
				SET @Query = @Query + ' WHERE LEN(RTRIM(LTRIM(dt.[' + @Column + ']))) = 0 '
			END
		END 
		PRINT @Query;
		EXEC (@Query);

		FETCH NEXT FROM ColR INTO @Table, @Column
	END
	CLOSE ColR
	DEALLOCATE ColR


	SELECT * FROM #Results ORDER BY Validation_Type

	DROP TABLE #Results;