WITH addTax as (
	SELECT tax.[P-SYS-ID], tax.[P-SSN-NUM], tax.[P-FED-TAX-ID]
	FROM SRC_ProviderTaxIds tax
	JOIN (SELECT [P-SYS-ID], MAX([P-TAX-END-DT]) as  [P-TAX-END-DT]
		FROM SRC_ProviderTaxIds 
		WHERE [EnableConversion] = 1
		GROUP BY [P-SYS-ID]) sub
	ON tax.[P-SYS-ID] = sub.[P-SYS-ID] AND tax.[P-TAX-END-DT] = sub.[P-TAX-END-DT]
)
UPDATE REG_PROVIDER 
	SET TAX_ID = 
		CASE WHEN s_prov.[P-SSN-NUM] IS NOT NULL AND 
				LEN(RTRIM(LTRIM(s_prov.[P-SSN-NUM]))) > 0 AND 
				s_prov.[P-SSN-NUM] <> '000000000' THEN
			s_prov.[P-SSN-NUM]
		ELSE
			CASE WHEN s_prov.[P-INDIV-GRP-CD] IN ('G', 'B') OR s_prov.[P-NAM-ORG-IND] = 'Y' THEN
				tax.[P-FED-TAX-ID]
			ELSE
				NULL
			END
		END,
		TAX_ID_TYPE_ID = 
		CASE WHEN s_prov.[P-SSN-NUM] IS NOT NULL AND 
				LEN(RTRIM(LTRIM(s_prov.[P-SSN-NUM]))) > 0 AND 
				s_prov.[P-SSN-NUM] <> '000000000' THEN
			15
		ELSE
			CASE WHEN s_prov.[P-INDV-GRP-CD] IN ('G', 'B') OR s_prov.[P-NAM-ORG-IND] = 'Y' THEN
				16
			ELSE
				15
			END
		END
FROM REG_PROVIDER prov
INNER JOIN DCConv_KeyCrossReferences map ON map.RegistrationID=prov.REG_ID
INNER JOIN addTax tax ON tax.[P-SYS-ID] = map.SysID
INNER JOIN SRC_Providers s_prov ON s_prov.[P-SYS-ID] = map.SysID
WHERE prov.[ENROLLMENT_STATUS_CODE] = '00' AND 
	dbo.fn_ConvertDCDateToPDMS(prov.END_DATE) > '12/31/2015' AND 
	(prov.TAX_ID IS NULL OR LEN(RTRIM(LTRIM(prov.TAX_ID))) = 0 OR prov.TAX_ID = '000000000')