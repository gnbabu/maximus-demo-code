SELECT DISTINCT en.[P-SYS-ID]
      ,[P-STAT-EFF-DT]
      ,en.[P-TY-CD]
	  ,prv.[P-ID]
	  ,pt.Long AS ProviderType
      ,[P-STAT-END-DT]
      ,[P-ENROL-STAT-TY-CD]
      ,en.[G-AUD-USER-ID]
      ,[G-AUD-TS]
      ,[P-BRND-DISCT-PCT]
      ,[P-GENR-DISCT-PCT]
      ,[P-DISP-FEE-AMT]
      ,[P-RE-ENROL-STAT-CD]
      ,[P-ENROL-APP-SRC-CD]
  FROM [dbo].[DT_PENROLTB_STG] en
  INNER JOIN [dbo].[DT_PROVDRTB_STG] prv ON prv.[P-SYS-ID] = en.[P-SYS-ID] 
  LEFT OUTER JOIN [dbo].[LKUP_PROVIDER_TYPE_CODE] pt ON pt.[P-TY-CD] = en.[P-TY-CD]
  WHERE en.[P-ENROL-STAT-TY-CD] = '00' AND prv.[P-REC-TY-CD] = 'P' AND en.[P-STAT-END-DT] IS NOT NULL AND
  CASE WHEN ISDATE(en.[P-STAT-END-DT]) = 1 THEN CAST(en.[P-STAT-END-DT] AS datetime) ELSE '1/1/1753' END > '12/31/2015'​