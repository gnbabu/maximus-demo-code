''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' NPI Data Dissemination Rewrite Program
'
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Option Explicit

''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Some global variables
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Dim TabStop
Dim NewLine
Dim CarrRetn

''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Constants for opening files
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Const OpenFileForReading = 1 
Const OpenFileForWriting = 2 
Const OpenFileForAppending = 8 

''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Main Routine
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

	Dim objFSO

	Dim strIPath, strOPath, strAPath
	Dim objFolder, objFiles, objFile, objInFile, strInFileName, SkipFile
	Dim objSpecDict, arrSpecSrcName(), arrSpecTgtDest(), arrSpecTgtName(), arrFileSrcName(), arrFileTgtDest(), arrFileTgtName()
	Dim strInLine, arrHdrCols(), intNumCols, strFldVal, intNumRecsRead, intNumRecsWrite
	Dim strSrcFldName, intSrcFldNum, strTgtOutput, strTgtFldName
	Dim strStSrch, binStFilt, strStFlag
	Dim objBaseFile, objONameFile, objMAddrFile, objPAddrFile, objAuthOfflFile, objTaxLicFile, objOtherIDFile
	Dim strBaseHdr, strONameHdr, strMAddrHdr, strPAddrHdr, strAuthOfflHdr, strTaxLicHdr, strOtherIDHdr
	Dim strBaseLine, strONameLine, strMAddrLine, strPAddrLine, strAuthOfflLine, strTaxLicLine, strOtherIDLine
	Dim strBaseTxt, strONameTxt, strMAddrTxt, strPAddrTxt, strAuthOfflTxt, strTaxLicTxt, strOtherIDTxt
	Dim intTaxLicSeq, intOtherIDSeq, intLastTaxLicSeq, intLastOtherIDSeq
	Dim strNPI, strTaxonomyCode, strTaxonomyFlag, strLicenseNum, strLicenseState, strOtherID, strOtherIDType, strOtherIDState, strOtherIDIssuer

	' Set up global data.
	TabStop = Chr(9)
	NewLine = Chr(10)
	CarrRetn = Chr(13)
	strIPath = "C:\NPI\in\"
	strOPath = "C:\NPI\out\"

	' The following variables are used to tag or filter records by state

	' Set strStSrch to the state value to search/filter
	' The value should be in the following format ",""XX"","" where XX is replaced by the state abbreviation
	' This will identify comma and quote delimited occurences of the state abbreviation in the source file record
	strStSrch = ",""TN"","

	' Set binStFilt to False to simply tag records for the desired state
	' Set binStFilt to True to filter and only export records for the desired state
	binStFilt = False
	
	Set objFSO = CreateObject("Scripting.FileSystemObject")

	' Read the header format for the source file
	Set objSpecDict = CreateObject("Scripting.Dictionary")
	Set objInFile = objFSO.OpenTextFile(strIPath&"npi_specs_v2.csv", OpenFileForReading)
	intNumRecsRead = 0
	Do While Not objInFile.AtEndOfStream
		strInLine = Null
		strSrcFldName = Null
		intSrcFldNum = Null
		strTgtOutput = Null
		strTgtFldName = Null
		intNumCols = 0
		strInLine = objInFile.ReadLine
		intNumRecsRead = intNumRecsRead + 1
		ReDim Preserve arrSpecSrcName(intNumRecsRead)
		ReDim Preserve arrSpecTgtDest(intNumRecsRead)
		ReDim Preserve arrSpecTgtName(intNumRecsRead)
		Do While InStr(strInline,"""") > 0
			intNumCols = intNumCols + 1
			ParseInLine
			If intNumCols = 1 Then
				strSrcFldName = strFldVal
			ElseIf intNumCols = 2 Then
				intSrcFldNum = strFldVal
			ElseIf intNumCols = 3 Then
				strTgtOutput = strFldVal
			ElseIf intNumCols = 4 Then
				strTgtFldName = strFldVal
			End If
		Loop
		objSpecDict.Add strSrcFldName, intSrcFldNum
		arrSpecSrcName(intNumRecsRead) = strSrcFldName
		arrSpecTgtDest(intNumRecsRead) = strTgtOutput
		arrSpecTgtName(intNumRecsRead) = strTgtFldName
	Loop
	intNumRecsRead = 0
	objInFile.Close

	Set objFolder = objFSO.GetFolder(strIPath)
	Set objFiles = objFolder.Files
	
	If objFiles.Count <> 0 Then
		For Each objFile In objFiles
		'Modify to trap the latest file
		'Modify to archive all other files
		
			strInFileName = UCase(objFile.Name)
			
			If InStr(strInFileName,"NPIDATA_LOAD.CSV") > 0 Then
			' Modify to check also for file type (e.g., CSV)
				SkipFile = "N"
			Else
				SkipFile = "Y"
			End If
				
			If SkipFile = "N" Then

				Set objInFile = objFSO.OpenTextFile(strIPath&objFile.Name, OpenFileForReading)

				Set objBaseFile = objFSO.CreateTextFile(strOPath&"NPI_BASE.csv", True)
				Set objONameFile = objFSO.CreateTextFile(strOPath&"NPI_OTHER_NAME.csv", True)
				Set objMAddrFile = objFSO.CreateTextFile(strOPath&"NPI_MAIL_ADDR.csv", True)
				Set objPAddrFile = objFSO.CreateTextFile(strOPath&"NPI_PRAC_ADDR.csv", True)
				Set objAuthOfflFile = objFSO.CreateTextFile(strOPath&"NPI_AUTH_OFFL.csv", True)
				Set objTaxLicFile = objFSO.CreateTextFile(strOPath&"NPI_TAX_LIC.csv", True)
				Set objOtherIDFile = objFSO.CreateTextFile(strOPath&"NPI_OTHER_ID.csv", True)

				intNumRecsRead = -1
				intNumRecsWrite = 0

				Do While Not objInFile.AtEndOfStream

					strInLine = Null
					strStFlag = ""
					strNPI = ""
					
					strBaseLine = ""
					strBaseTxt = " "
					strONameLine = ""
					strONameTxt = " "
					strMAddrLine = ""
					strMAddrTxt = " "
					strPAddrLine = ""
					strPAddrTxt = " "
					strAuthOfflLine = ""
					strAuthOfflTxt = " "

					intTaxLicSeq = 0
					intOtherIDSeq = 0
					intLastTaxLicSeq = 0
					intLastOtherIDSeq = 0

					intNumCols = 0
					intSrcFldNum = 0

					'Read record from input file
					strInLine = objInFile.ReadLine

					intNumRecsRead = intNumRecsRead + 1

					If intNumRecsRead = 0 Then
						Do While InStr(strInLine,"""") > 0

							intSrcFldNum = Null
							intNumCols = intNumCols + 1
							ParseInLine

							intSrcFldNum = objSpecDict.Item(strFldVal)

							Redim Preserve arrFileSrcName(intNumCols)
							arrFileSrcName(intNumCols) = strFldVal
							Redim Preserve arrFileTgtDest(intNumCols)
							arrFileTgtDest(intNumCols) = arrSpecTgtDest(intSrcFldNum)
							Redim Preserve arrFileTgtName(intNumCols)
							arrFileTgtName(intNumCols) = arrSpecTgtName(intSrcFldNum)
						Loop
					Else
						If InStr(strInLine,strStSrch) > 0 Then
							strStFlag = "Y"
						Else
							strStFlag = "N"
						End If

						Do While InStr(strInLine,"""") > 0
							strSrcFldName = Null
							intSrcFldNum = Null
							strTgtOutput = Null
							strTgtFldName = Null

							intNumCols = intNumCols + 1
							ParseInLine

							strSrcFldName = arrFileSrcName(intNumCols)
							strTgtOutput = arrFileTgtDest(intNumCols)
							strTgtFldName = arrFileTgtName(intNumCols)

							If intNumRecsRead = 1 Then
								If strSrcFldName = "NPI" Then
									strBaseHdr = """" & strTgtFldName & """"
									strONameHdr = """" & strTgtFldName & """"
									strMAddrHdr = """" & strTgtFldName & """"
									strPAddrHdr = """" & strTgtFldName & """"
									strAuthOfflHdr = """" & strTgtFldName & """"
									strTaxLicHdr = """" & strTgtFldName & """,""Record Sequence"""
									strOtherIDHdr = """" & strTgtFldName & """,""Record Sequence"""
								Else
									If strTgtOutput = "Base" Then

										strBaseHdr = strBaseHdr & ",""" & strTgtFldName & """"

									ElseIf strTgtOutput = "OtherName" Then

										strONameHdr = strONameHdr & ",""" & strTgtFldName & """"

									ElseIf strTgtOutput = "MailAddr" Then

										strMAddrHdr = strMAddrHdr & ",""" & strTgtFldName & """"

									ElseIf strTgtOutput = "PracAddr" Then

										strPAddrHdr = strPAddrHdr & ",""" & strTgtFldName & """"

									ElseIf strTgtOutput = "AuthOffl" Then

										strAuthOfflHdr = strAuthOfflHdr & ",""" & strTgtFldName & """"

									ElseIf strTgtOutput = "TaxLic" Then

										If strSrcFldName = "Healthcare Provider Taxonomy Code_1" Then
											strTaxLicHdr = strTaxLicHdr & ",""" & strTgtFldName & """"
										ElseIf strSrcFldName = "Provider License Number_1" Then
											strTaxLicHdr = strTaxLicHdr & ",""" & strTgtFldName & """"
										ElseIf strSrcFldName = "Provider License Number State Code_1" Then
											strTaxLicHdr = strTaxLicHdr & ",""" & strTgtFldName & """"
										ElseIf strSrcFldName = "Healthcare Provider Primary Taxonomy Switch_1" Then
											strTaxLicHdr = strTaxLicHdr & ",""" & strTgtFldName & """"
											objTaxLicFile.WriteLine(strTaxLicHdr)
										End If

									ElseIf strTgtOutput = "OtherID" Then

										If strSrcFldName = "Other Provider Identifier_1" Then
											strOtherIDHdr = strOtherIDHdr & ",""" & strTgtFldName & """"
										ElseIf strSrcFldName = "Other Provider Identifier Type Code_1" Then
											strOtherIDHdr = strOtherIDHdr & ",""" & strTgtFldName & """"
										ElseIf strSrcFldName = "Other Provider Identifier State_1" Then
											strOtherIDHdr = strOtherIDHdr & ",""" & strTgtFldName & """"
										ElseIf strSrcFldName = "Other Provider Identifier Issuer_1" Then
											strOtherIDHdr = strOtherIDHdr & ",""" & strTgtFldName & """"
											objOtherIDFile.WriteLine(strOtherIDHdr)
										End If

									End If

								End If

							End If

							If strSrcFldName = "NPI" Then
							
								strNPI = strFldVal
								strBaseLine = """" & strNPI & """"
								strONameLine = """" & strNPI & """"
								strMAddrLine = """" & strNPI & """"
								strPAddrLine = """" & strNPI & """"
								strAuthOfflLine = """" & strNPI & """"

							Else
								If strTgtOutput = "Base" Then

									strBaseLine = strBaseLine & ",""" & strFldVal & """"
									strBaseTxt = strBaseTxt & strFldVal

								ElseIf strTgtOutput = "OtherName" Then

									strONameLine = strONameLine & ",""" & strFldVal & """"
									strONameTxt = strONameTxt & strFldVal

								ElseIf strTgtOutput = "MailAddr" Then

									strMAddrLine = strMAddrLine & ",""" & strFldVal & """"
									strMAddrTxt = strMAddrTxt & strFldVal

								ElseIf strTgtOutput = "PracAddr" Then

									strPAddrLine = strPAddrLine & ",""" & strFldVal & """"
									strPAddrTxt = strPAddrTxt & strFldVal

								ElseIf strTgtOutput = "AuthOffl" Then

									strAuthOfflLine = strAuthOfflLine & ",""" & strFldVal & """"
									strAuthOfflTxt = strAuthOfflTxt + strFldVal

								ElseIf strTgtOutput = "TaxLic" Then

									If InStr(strSrcFldName,"Healthcare Provider Taxonomy Code_") > 0 Then
										intTaxLicSeq = CInt(Mid(strSrcFldName,InStrRev(strSrcFldName,"_")+1))
									End If
									
									strTaxLicTxt = strTaxLicTxt & strFldVal

									If strTgtFldName = "Healthcare Provider Taxonomy Code" Then
										strTaxonomyCode = strFldVal
									ElseIf strTgtFldName = "Provider License Number" Then
										strLicenseNum = strFldVal
									ElseIf strTgtFldName = "Provider License Number State Code" Then
										strLicenseState = strFldVal
									ElseIf strTgtFldName = "Healthcare Provider Primary Taxonomy Switch" Then
										strTaxonomyFlag = strFldVal
										If intLastTaxLicSeq < intTaxLicSeq Then
											If binStFilt = False Or (binStFilt = True And strStFlag = "Y") Then
												If Trim(strTaxLicTxt) > "" Then
													strTaxLicLine = """" & strNPI & """,""" & intTaxLicSeq & """,""" & strTaxonomyCode & """,""" & strLicenseNum & """,""" & strLicenseState & """,""" & strTaxonomyFlag & """"
													objTaxLicFile.WriteLine(strTaxLicLine)
												End If
											End If
											intLastTaxLicSeq = intTaxLicSeq
											strTaxLicLine = Null
											strTaxLicTxt = Null
											strTaxonomyCode = Null
											strLicenseNum = Null
											strLicenseState = Null
											strTaxonomyFlag = Null
										End If
									End If

								ElseIf strTgtOutput = "OtherID" Then

									If InStr(strSrcFldName,"Other Provider Identifier_") > 0 Then
										intOtherIDSeq = CInt(Mid(strSrcFldName,InStrRev(strSrcFldName,"_")+1))
									End If
									
									strOtherIDTxt = strOtherIDTxt & strFldVal

									If strTgtFldName = "Other Provider Identifier" Then
										strOtherID = strFldVal
									ElseIf strTgtFldName = "Other Provider Identifier Type Code" Then
										strOtherIDType = strFldVal
									ElseIf strTgtFldName = "Other Provider Identifier State" Then
										strOtherIDState = strFldVal
									ElseIf strTgtFldName = "Other Provider Identifier Issuer" Then
										strOtherIDIssuer = strFldVal
										If intLastOtherIDSeq < intOtherIDSeq Then
											If binStFilt = False Or (binStFilt = True And strStFlag = "Y") Then
												If Trim(strOtherIDTxt) > "" Then
													strOtherIDLine = """" & strNPI & """,""" & intOtherIDSeq & """,""" & strOtherID & """,""" & strOtherIDType & """,""" & strOtherIDState & """,""" & strOtherIDIssuer & """"
													objOtherIDFile.WriteLine(strOtherIDLine)
												End If
											End If
											intLastOtherIDSeq = intOtherIDSeq
											strOtherIDLine = Null
											strOtherIDTxt = Null
											strOtherID = Null
											strOtherIDType = Null
											strOtherIDState = Null
											strOtherIDIssuer = Null
										End If
									End If

								End If

							End If


						Loop

						If intNumRecsRead = 1 Then
							objBaseFile.WriteLine(strBaseHdr & ",""State Flag""")
							objONameFile.WriteLine(strONameHdr)
							objMAddrFile.WriteLine(strMAddrHdr)
							objPAddrFile.WriteLine(strPAddrHdr)
							objAuthOfflFile.WriteLine(strAuthOfflHdr)
						End If

						If binStFilt = False Or (binStFilt = True And strStFlag = "Y") Then
							If Trim(strBaseTxt) > "" Then
								objBaseFile.WriteLine(strBaseLine & ",""" & strStFlag & """")
							End If
							If Trim(strONameTxt) > "" Then
								objONameFile.WriteLine(strONameLine)
							End If
							If Trim(strMAddrTxt) > "" Then
								objMAddrFile.WriteLine(strMAddrLine)
							End If
							If Trim(strPAddrTxt) > "" Then
								objPAddrFile.WriteLine(strPAddrLine)
							End If
							If Trim(strAuthOfflTxt) > "" Then
								objAuthOfflFile.WriteLine(strAuthOfflLine)
							End If
							intNumRecsWrite = intNumRecsWrite + 1
						End If
						
					End If
					
				Loop
				objInFile.Close   
				objBaseFile.Close
				objONameFile.Close
				objMAddrFile.Close
				objPAddrFile.Close
				objAuthOfflFile.Close
				objTaxLicFile.Close
				objOtherIDFile.Close
			End If
		Next
	End If
	MsgBox("Rewrite of NPI file(s) complete. " & intNumRecsRead & " records read, " & intNumRecsWrite & " records written.")


''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Functions and Subs
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Function ParseInLine

If Left(strInLine,1) = """" Then
	strInLine = Mid(strInLine,2)
	strFldVal = Left(strInLine,InStr(strInLine,"""")-1)
	If InStr(strInLIne,""",") > 0 Then
		strInLine = Mid(strInLine,InStr(strInLine,"""")+2)
	Else
		strInLine = ""
	End If
End If

End Function