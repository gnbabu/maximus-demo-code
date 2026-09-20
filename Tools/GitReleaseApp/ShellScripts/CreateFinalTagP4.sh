#!/bin/sh

P4REPOPATH=$1
STARTDATE=$2
STARTDATETIME=$3
ENDDATE=$4
ENDDATETIME=$5
TOKEN=$6
COMMITHISTPATH=$7
UATBRANCHNAME=$8
INITIALTAGNAME=$9
FINALTAGNAME=${10}

cd $P4REPOPATH

#ssis

cd $P4REPOPATH'ohpnm-ssis\'

if [ `git branch --list $UATBRANCHNAME` ]
then
   
	echo "Branch name $UATBRANCHNAME already exists."
   
	git checkout $UATBRANCHNAME
      
fi

echo "checkout ohpnm-ssis $UATBRANCHNAME completed"

if [ $(git tag -l "$FINALTAGNAME") ]; then

    echo "Tag $FINALTAGNAME already exists"
	
else

	git tag $FINALTAGNAME

	git push origin $FINALTAGNAME

fi

echo "Final Tag $FINALTAGNAME for repo ohpnm-ssis  branch $UATBRANCHNAME completed"

#src

cd $P4REPOPATH'ohpnm-src\'

if [ `git branch --list $UATBRANCHNAME` ]
then
   
	echo "Branch name $UATBRANCHNAME already exists."
   
	git checkout $UATBRANCHNAME
      
fi

echo "checkout ohpnm-src $UATBRANCHNAME completed"

if [ $(git tag -l "$FINALTAGNAME") ]; then

    echo "Tag $FINALTAGNAME already exists"
	
else

	git tag $FINALTAGNAME

	git push origin $FINALTAGNAME

fi

echo "Final Tag $FINALTAGNAME for repo ohpnm-src  branch $UATBRANCHNAME completed"

#si-mq

cd $P4REPOPATH'ohpnm-si-mq\'

if [ `git branch --list $UATBRANCHNAME` ]
then
   
	echo "Branch name $UATBRANCHNAME already exists."
   
	git checkout $UATBRANCHNAME
      
fi

echo "checkout ohpnm-si-mq $UATBRANCHNAME completed"

if [ $(git tag -l "$FINALTAGNAME") ]; then

    echo "Tag $FINALTAGNAME already exists"
	
else

	git tag $FINALTAGNAME

	git push origin $FINALTAGNAME

fi

echo "Final Tag $FINALTAGNAME for repo ohpnm-si-mq  branch $UATBRANCHNAME completed"

#tibco

cd $P4REPOPATH'ohpnm-tibco\'

if [ `git branch --list $UATBRANCHNAME` ]
then
   
	echo "Branch name $UATBRANCHNAME already exists."
   
	git checkout $UATBRANCHNAME
      
fi

echo "checkout ohpnm-tibco $UATBRANCHNAME completed"

if [ $(git tag -l "$FINALTAGNAME") ]; then

    echo "Tag $FINALTAGNAME already exists"
	
else

	git tag $FINALTAGNAME

	git push origin $FINALTAGNAME

fi

echo "Final Tag $FINALTAGNAME for repo ohpnm-tibco  branch $UATBRANCHNAME completed"

#ssrs

cd $P4REPOPATH'ohpnm-ssrs\'

if [ `git branch --list $UATBRANCHNAME` ]
then
   
	echo "Branch name $UATBRANCHNAME already exists."
   
	git checkout $UATBRANCHNAME
      
fi

echo "checkout ohpnm-ssrs $UATBRANCHNAME completed"

if [ $(git tag -l "$FINALTAGNAME") ]; then

    echo "Tag $FINALTAGNAME already exists"
	
else

	git tag $FINALTAGNAME

	git push origin $FINALTAGNAME

fi

echo "Final Tag $FINALTAGNAME for repo ohpnm-ssrs  branch $UATBRANCHNAME completed"
