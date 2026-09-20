SELECT DISTINCT en.[P-SYS-ID]
      ,en.[P-TY-CD] AS ProviderTypeCode
  	  ,pt.Long AS ProviderType
	  ,prv.[P-ID] AS ProviderID
	  ,prv.[P-NPI-NUM] AS PrimaryNPI
	  ,prv.[P-NAM] AS ProviderName
	  ,[P-ALT-ID] AS AltID
      ,[P-ALT-ID-TY-CD] AS AltIDType
      ,[P-ALT-ID-BEG-DT] AS AltIDStartDate
      ,[P-ALT-ID-VOID-IND] AS AlitIDVoid
      ,[P-ALT-ID-END-DT] AS AltIDEndDate
  FROM [dbo].[DT_PENROLTB_STG] en
  INNER JOIN [dbo].[DT_PROVDRTB_STG] prv ON prv.[P-SYS-ID] = en.[P-SYS-ID] 
  INNER JOIN [dbo].[DT_PALTIDTB_STG] alt ON alt.[P-SYS-ID] = en.[P-SYS-ID]
  LEFT OUTER JOIN [DC_PDMS_CONVSRC01].[dbo].[LKUP_PROVIDER_TYPE_CODE] pt ON pt.[P-TY-CD] = en.[P-TY-CD]
  WHERE [P-ENROL-STAT-TY-CD] = '00' AND prv.[P-REC-TY-CD] = 'P' AND [P-STAT-END-DT] IS NOT NULL AND
  CASE WHEN ISDATE([P-STAT-END-DT]) = 1 THEN CAST([P-STAT-END-DT] AS datetime) ELSE '1/1/1753' END > '12/31/2015'​ AND
  CASE WHEN ISDATE([P-ALT-ID-END-DT]) = 1 THEN CAST([P-ALT-ID-END-DT] AS datetime) ELSE '1/1/1753' END > '12/31/2015' AND
  alt.[P-ALT-ID-TY-CD]= 'XX' AND [P-ALT-ID-VOID-IND] = 'N' AND
  (SELECT COUNT(*) FROM [dbo].[DT_PALTIDTB_STG] en1
  INNER JOIN [dbo].[DT_PROVDRTB_STG] prv1 ON prv1.[P-SYS-ID] = en1.[P-SYS-ID] 
  WHERE prv1.[P-ID] = prv.[P-ID] AND CASE WHEN ISDATE([P-ALT-ID-END-DT]) = 1 THEN CAST([P-ALT-ID-END-DT] AS datetime) ELSE '1/1/1753' END > '12/31/2015' AND
  en1.[P-ALT-ID-TY-CD]= 'XX' AND en1.[P-ALT-ID-VOID-IND] = 'N') > 1
  ORDER BY ProviderID, ProviderTypeCode, ProviderName
GO

