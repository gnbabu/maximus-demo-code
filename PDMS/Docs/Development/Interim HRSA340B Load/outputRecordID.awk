{
	if ($1 == record)
	{
		print $0
	}
	if ($1 > record)
	{
		exit
	}
}
