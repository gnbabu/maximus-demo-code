SELECT DISTINCT en.[P-SYS-ID]
	  ,prv.[P-NAM]
      ,en.[P-TY-CD]
	  ,pcos.[P-COS-CD]
      ,pt.Long AS ProviderType
  FROM [DT_PENROLTB_STG] en
  INNER JOIN [DT_PROVDRTB_STG] prv ON prv.[P-SYS-ID] = en.[P-SYS-ID]
  INNER JOIN [DT_PRVCOSTB_STG] pcos ON en.[P-SYS-ID] = pcos.[P-SYS-ID]
  LEFT OUTER JOIN [DC_PDMS_CONVSRC01].[dbo].[LKUP_PROVIDER_TYPE_CODE] pt ON pt.[P-TY-CD] = en.[P-TY-CD]
 WHERE [P-ENROL-STAT-TY-CD] = '00' AND 
 prv.[P-REC-TY-CD] = 'P' AND 
 [P-STAT-END-DT] IS NOT NULL AND
 CAST([P-STAT-END-DT] AS datetime) > '12/31/2015'
 AND 
 CAST(pcos.[P-COS-END-DT] AS datetime) > '12/31/2015'
 AND 
 NOT EXISTS(SELECT * FROM LKUP_VALID_COS vcos WHERE  CAST(vcos.[COS] AS int) = CAST(pcos.[P-COS-CD] AS int) AND vcos.ProviderType = en.[P-TY-CD])