SELECT DISTINCT en.[P-SYS-ID]
	  ,prv.[P-NAM]
	  ,prv.[P-ID]
      ,[P-STAT-EFF-DT]
      ,en.[P-TY-CD]
	  ,pt.Long AS ProviderType
	  ,addr.[P-ZIP5-CD] AS ZipCode
	  ,addr.[P-ZIP4-CD] AS ZipExt
	  ,prv.[P-NPI-NUM]
	  ,tax.[P-TAXONOMY-CD]
  FROM [DT_PENROLTB_STG] en
  INNER JOIN [DC_PDMS_CONVSRC01].[dbo].[DT_PROVDRTB_STG] prv ON prv.[P-SYS-ID] = en.[P-SYS-ID]
  INNER JOIN [DT_PTAXONTB_STG] tax ON tax.[P-SYS-ID]=en.[P-SYS-ID]
  INNER JOIN [DT_PADDRSTB_STG] addr ON addr.[P-SYS-ID] = en.[P-SYS-ID]
  LEFT OUTER JOIN [LKUP_PROVIDER_TYPE_CODE] pt ON pt.[P-TY-CD] = en.[P-TY-CD]
 WHERE addr.[P-ADR-TY-CD] = 'L' AND prv.[P-REC-TY-CD] = 'P'  AND 
  EXISTS(SELECT * FROM [DT_PENROLTB_STG] en1
  INNER JOIN [DC_PDMS_CONVSRC01].[dbo].[DT_PROVDRTB_STG] prv1 ON prv1.[P-SYS-ID] = en1.[P-SYS-ID]
  INNER JOIN [DT_PTAXONTB_STG] tax1 ON tax1.[P-SYS-ID]=en1.[P-SYS-ID]
  INNER JOIN [DT_PADDRSTB_STG] addr1 ON addr1.[P-SYS-ID] = en1.[P-SYS-ID]
  WHERE  prv1.[P-REC-TY-CD] = 'P' AND
  addr1.[P-ADR-TY-CD] = 'L' AND
  en1.[P-SYS-ID] <> en.[P-SYS-ID] AND 
 prv1.[P-NPI-NUM] = prv.[P-NPI-NUM] AND 
 tax1.[P-TAXONOMY-CD] = tax.[P-TAXONOMY-CD] AND
 addr1.[P-ZIP4-CD] = addr.[P-ZIP4-CD] AND 
 addr1.[P-ZIP5-CD] = addr.[P-ZIP5-CD]
  ) ORDER BY prv.[P-NPI-NUM], tax.[P-TAXONOMY-CD],addr.[P-ZIP5-CD] ,addr.[P-ZIP4-CD] 

