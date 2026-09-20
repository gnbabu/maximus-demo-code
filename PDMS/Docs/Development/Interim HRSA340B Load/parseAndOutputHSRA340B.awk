func trim(input)
{
	output = gensub(/[", ]/,"","g",input)
	return output
}
BEGIN {
	FS = "\t"
	OFS = "\t"
	#getline
}
{
	#if (NR < 100)
	if (NR < 3)
	{
		Record = 0
	}
	else
	{
		if (length($23) > 0)
		{
			n = split($23, MedicaidParts, ",")
			for (i=1; i<= n; i++)
			{
				bracketLoc = index(MedicaidParts[i], "(")
				if (bracketLoc >0)
				{
					number = substr(MedicaidParts[i],1,bracketLoc-1)
					state = substr(MedicaidParts[i],bracketLoc+1,2)
				}
				else
				{
					state = "All"
					number = MedicaidParts[i]
				}
				print Record OFS "Medicaid" OFS trim(state) OFS trim(number) >"IDS.txt"
			}
		#print $23
		}
		if (length($24) > 0)
		{
			n = split($24, NPIParts, ",")
			for (i=1; i<= n; i++)
			{
				bracketLoc = index(NPIParts[i], "(")
				if (bracketLoc >0)
				{
					number = substr(NPIParts[i],1,bracketLoc-1)
					state = substr(NPIParts[i],bracketLoc+1,2)
				}
				else
				{
					state = "All"
					number = NPIParts[i]
				}
				print Record OFS "NPI" OFS trim(state) OFS trim(number) >"IDS.txt"
			}
		#print $24
		}
		print Record OFS $0>"Record.txt"
		Record++
	}
}
END {
}
