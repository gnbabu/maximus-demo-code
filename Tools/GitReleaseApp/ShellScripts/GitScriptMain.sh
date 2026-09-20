#!/bin/sh

#$1 path
#$2 Outputpath
#$3 p3p4
#$4 type
#$5 p3RepoPath
#$6 p4RepoPath
#$7 startDate
#$8 startTime
#$9 endDate
#$10 endTime
#$11 token
#$12 documentationPath
#$13 uatBranchName
#$14 initialTagName
#$15 finalTagName

echo $1

cd $1

if [[ "$3" == "P3" ]]
then

	if [[ "$4" == "CreateDocumentation" ]]
	then
		sh ./CreateDocumentationP3.sh $5 $7 $8 $9 ${10} ${11} ${12} ${13} ${14} ${15} | tee $2
	elif [[ "$4" == "CheckoutDEV" ]]
	then
		sh ./CheckoutDEVP3.sh $5 $7 $8 $9 ${10} ${11} ${12} ${13} ${14} ${15} | tee $2
	elif [[ "$4" == "CreateBranchUAT" ]]
	then
		sh ./CreateBranchUATP3.sh $5 $7 $8 $9 ${10} ${11} ${12} ${13} ${14} ${15} | tee $2
	elif [[ "$4" == "CheckoutUAT" ]]
	then
		sh ./CheckoutUATP3.sh $5 $7 $8 $9 ${10} ${11} ${12} ${13} ${14} ${15} | tee $2
	elif [[ "$4" == "CreateInitialTag" ]]
	then
		sh ./CreateInitialTagP3.sh $5 $7 $8 $9 ${10} ${11} ${12} ${13} ${14} ${15} | tee $2
	elif [[ "$4" == "CreateFinalTag" ]]
	then
		sh ./CreateFinalTagP3.sh $5 $7 $8 $9 ${10} ${11} ${12} ${13} ${14} ${15} | tee $2	
	elif [[ "$4" == "CommitVersionChange" ]]
	then
		sh ./CommitVersionChangeP3.sh $5 $7 $8 $9 ${10} ${11} ${12} ${13} ${14} ${15} | tee $2	
	fi

else

	if [[ "$4" == "CreateDocumentation" ]]
	then
		sh ./CreateDocumentationP4.sh $6 $7 $8 $9 ${10} ${11} ${12} ${13} ${14} ${15} | tee $2
	elif [[ "$4" == "CheckoutDEV" ]]
	then
		sh ./CheckoutDEVP4.sh $6 $7 $8 $9 ${10} ${11} ${12} ${13} ${14} ${15} | tee $2
	elif [[ "$4" == "CreateBranchUAT" ]]
	then
		sh ./CreateBranchUATP4.sh $6 $7 $8 $9 ${10} ${11} ${12} ${13} ${14} ${15} | tee $2
	elif [[ "$4" == "CheckoutUAT" ]]
	then
		sh ./CheckoutUATP4.sh $6 $7 $8 $9 ${10} ${11} ${12} ${13} ${14} ${15} | tee $2
	elif [[ "$4" == "CreateInitialTag" ]]
	then
		sh ./CreateInitialTagP4.sh $6 $7 $8 $9 ${10} ${11} ${12} ${13} ${14} ${15} | tee $2
	elif [[ "$4" == "CreateFinalTag" ]]
	then
		sh ./CreateFinalTagP4.sh $6 $7 $8 $9 ${10} ${11} ${12} ${13} ${14} ${15} | tee $2	
	elif [[ "$4" == "CommitVersionChange" ]]
	then
		sh ./CommitVersionChangeP4.sh $6 $7 $8 $9 ${10} ${11} ${12} ${13} ${14} ${15} | tee $2	
	fi

fi

