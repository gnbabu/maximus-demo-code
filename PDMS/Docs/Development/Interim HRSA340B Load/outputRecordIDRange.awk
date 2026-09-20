func trim(input)
{
	output = gensub(/[", ]/,"","g",input)
	return output
}
BEGIN {
	FS = "\t"
	RS = "\r\n"
	ORS = "\r\n"
	#printf "Min: " Min "\tMax: " Max "\r\n"
	#exit
}
{
	RecordID = trim($1)
	#print RecordID 
	if ((RecordID  >= Min) && (RecordID  <= Max))
	{
		#print RecordID
		print $0
	}
	if (RecordID  >= Max)
	{
		exit
	}
	if (NR > 100)
	{
		#exit
	}
}
