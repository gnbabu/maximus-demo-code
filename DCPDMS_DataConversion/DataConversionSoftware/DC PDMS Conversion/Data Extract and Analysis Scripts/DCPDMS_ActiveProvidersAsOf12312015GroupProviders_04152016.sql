SELECT DISTINCT en.[P-SYS-ID]
      ,en.[P-TY-CD]
	  ,pt.Long AS ProviderType
  FROM [dbo].[DT_PENROLTB_STG] en
  INNER JOIN [DT_PROVDRTB_STG] prv ON prv.[P-SYS-ID] = en.[P-SYS-ID] 
  INNER JOIN [DT_PAFFILTB_STG] affil ON affil.[P-GROUP-SYS-ID] = en.[P-SYS-ID]
  LEFT OUTER JOIN [dbo].[LKUP_PROVIDER_TYPE_CODE] pt ON pt.[P-TY-CD] = en.[P-TY-CD]
  WHERE en.[P-ENROL-STAT-TY-CD] = '00' AND prv.[P-REC-TY-CD] = 'P' AND en.[P-STAT-END-DT] IS NOT NULL AND
  CAST(en.[P-STAT-END-DT] AS datetime) > '4/1/2016'​