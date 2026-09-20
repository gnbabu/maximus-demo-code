-- Count of providers by provider type
SELECT 'Total providers by type' AS [Description], pt.Short AS CountDescription, COUNT(*) AS [Count] 
FROM dbo.[DT_PENROLTB_STG] prv
INNER JOIN dbo.[LKUP_PROVIDER_TYPE_CODE] pt ON pt.[P-TY-CD]=prv.[P-TY-CD] 
WHERE [P-ENROL-STAT-TY-CD] = '00' AND [P-STAT-END-DT] > '4/1/2016'
GROUP BY pt.Short
GO

-- Count of group provider members 
SELECT DISTINCT 'Total group members by group' AS [Description], grpProv.[P-SYS-ID], grpProv.[P-NAM] AS GroupName, grpType.Short AS GroupProviderType, grpProv.[P-ID] AS MedicareID, COUNT(DISTINCT memProv.[P-SYS-ID]) AS NumberOfMembers 
FROM dbo.[DT_PAFFILTB_STG] aff
INNER JOIN dbo.[DT_PROVDRTB_STG] grpProv ON grpProv.[P-SYS-ID] = aff.[P-GROUP-SYS-ID]
INNER JOIN dbo.[DT_PENROLTB_STG] grpEnrol ON grpEnrol.[P-SYS-ID] = grpProv.[P-SYS-ID] AND grpEnrol.[P-ENROL-STAT-TY-CD] = '00' 
INNER JOIN dbo.[DT_PROVDRTB_STG] memProv ON memProv.[P-SYS-ID] = aff.[P-MEMBER-SYS-ID]
INNER JOIN dbo.[DT_PENROLTB_STG] memEnrol ON memEnrol.[P-SYS-ID] = memProv.[P-SYS-ID] AND grpEnrol.[P-ENROL-STAT-TY-CD] = '00' 
INNER JOIN dbo.[LKUP_PROVIDER_TYPE_CODE] grpType ON grpType.[P-TY-CD] = grpEnrol.[P-TY-CD]
WHERE grpEnrol.[P-ENROL-STAT-TY-CD] = '00' GROUP BY grpProv.[P-SYS-ID], grpProv.[P-NAM],grpType.Short,grpProv.[P-ID]
GO

-- Count of group provider members currently active (status end date in the future)
SELECT DISTINCT 'Total group members by group with an end date' AS [Description],grpProv.[P-SYS-ID], grpProv.[P-NAM] AS GroupName, grpType.Short AS GroupProviderType, grpProv.[P-ID] AS MedicareID, COUNT(DISTINCT memProv.[P-SYS-ID]) AS NumberOfMembers 
FROM dbo.[DT_PAFFILTB_STG] aff
INNER JOIN dbo.[DT_PROVDRTB_STG] grpProv ON grpProv.[P-SYS-ID] = aff.[P-GROUP-SYS-ID]
INNER JOIN dbo.[DT_PENROLTB_STG] grpEnrol ON grpEnrol.[P-SYS-ID] = grpProv.[P-SYS-ID] AND grpEnrol.[P-ENROL-STAT-TY-CD] = '00' 
INNER JOIN dbo.[DT_PROVDRTB_STG] memProv ON memProv.[P-SYS-ID] = aff.[P-MEMBER-SYS-ID]
INNER JOIN dbo.[DT_PENROLTB_STG] memEnrol ON memEnrol.[P-SYS-ID] = memProv.[P-SYS-ID] AND grpEnrol.[P-ENROL-STAT-TY-CD] = '00' 
INNER JOIN dbo.[LKUP_PROVIDER_TYPE_CODE] grpType ON grpType.[P-TY-CD] = grpEnrol.[P-TY-CD]
WHERE grpEnrol.[P-ENROL-STAT-TY-CD] = '00' AND memEnrol.[P-STAT-END-DT] > '4/1/2016' GROUP BY grpProv.[P-SYS-ID], grpProv.[P-NAM],grpType.Short,grpProv.[P-ID]
GO

-- Count of providers by provider type with re-enrollment within 120 days of conversion

DECLARE @120DaysBeforeConversion datetime, @120DaysAfterConversion datetime;

SET @120DaysBeforeConversion = DATEADD(day, -120, '7/31/2016');
SET @120DaysAfterConversion = DATEADD(day, 120, '7/31/2016');

SELECT 'Total providers re-enroll within 120 days of Conversion' AS [Description],  pt.Short AS CountDescription, COUNT(*) AS [Count] 
FROM dbo.[DT_PENROLTB_STG] prv
INNER JOIN dbo.[LKUP_PROVIDER_TYPE_CODE] pt ON pt.[P-TY-CD]=prv.[P-TY-CD] 
WHERE [P-ENROL-STAT-TY-CD] = '00' AND [P-STAT-END-DT] >= @120DaysBeforeConversion AND [P-STAT-END-DT] <= @120DaysAfterConversion
GROUP BY pt.Short
GO

SELECT 'Total providers enrolled by paper to electronic by type' AS [Description],  pt.Short AS CountDescription, COUNT(*) AS [Count] 
FROM dbo.[DT_PENROLTB_STG] prv
INNER JOIN dbo.[LKUP_PROVIDER_TYPE_CODE] pt ON pt.[P-TY-CD]=prv.[P-TY-CD] 
WHERE [P-ENROL-STAT-TY-CD] = '00' AND [P-ENROL-APP-SRC-CD] = 'O' AND [P-STAT-END-DT] > '4/1/2016'
GROUP BY pt.Short
GO
