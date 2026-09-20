#!/bin/sh

P3REPOPATH=$1
STARTDATE=$2
STARTDATETIME=$3
ENDDATE=$4
ENDDATETIME=$5
TOKEN=$6
COMMITHISTPATH=$7
UATBRANCHNAME=$8
INITIALTAGNAME=$9
FINALTAGNAME=${10}

cd $P3REPOPATH

#ssis

cd $P3REPOPATH'ohpnm-ssis-p3\'

git remote set-url origin https://$TOKEN@github.com/ohpnm/ohpnm-ssis-p3

if [ `git branch --list $UATBRANCHNAME` ]
then
   
	echo "Branch name $UATBRANCHNAME already exists."
   
	git checkout $UATBRANCHNAME
      
fi

echo "checkout ohpnm-ssis-p3 $UATBRANCHNAME completed"

if [ $(git tag -l "$INITIALTAGNAME") ]; then

    echo "Tag $INITIALTAGNAME already exists"
	
else

	git tag $INITIALTAGNAME

	git push origin $INITIALTAGNAME

fi

echo "Initial Tag $INITIALTAGNAME for repo ohpnm-ssis-p3  branch $UATBRANCHNAME completed"

#src

cd $P3REPOPATH'ohpnm-src-p3\'

git remote set-url origin https://$TOKEN@github.com/ohpnm/ohpnm-src-p3

if [ `git branch --list $UATBRANCHNAME` ]
then
   
	echo "Branch name $UATBRANCHNAME already exists."
   
	git checkout $UATBRANCHNAME
      
fi

echo "checkout ohpnm-src-p3 $UATBRANCHNAME completed"

if [ $(git tag -l "$INITIALTAGNAME") ]; then

    echo "Tag $INITIALTAGNAME already exists"
	
else

	git tag $INITIALTAGNAME

	git push origin $INITIALTAGNAME

fi

echo "Initial Tag $INITIALTAGNAME for repo ohpnm-src-p3  branch $UATBRANCHNAME completed"

#si-mq

cd $P3REPOPATH'ohpnm-si-mq-p3\'

git remote set-url origin https://$TOKEN@github.com/ohpnm/ohpnm-si-mq-p3

if [ `git branch --list $UATBRANCHNAME` ]
then
   
	echo "Branch name $UATBRANCHNAME already exists."
   
	git checkout $UATBRANCHNAME
      
fi

echo "checkout ohpnm-si-mq-p3 $UATBRANCHNAME completed"

if [ $(git tag -l "$INITIALTAGNAME") ]; then

    echo "Tag $INITIALTAGNAME already exists"
	
else

	git tag $INITIALTAGNAME

	git push origin $INITIALTAGNAME

fi

echo "Initial Tag $INITIALTAGNAME for repo ohpnm-si-mq-p3  branch $UATBRANCHNAME completed"

# ssrs

cd $P3REPOPATH'ohpnm-ssrs-p3\'

git remote set-url origin https://$TOKEN@github.com/ohpnm/ohpnm-ssrs-p3

if [ `git branch --list $UATBRANCHNAME` ]
then
   
	echo "Branch name $UATBRANCHNAME already exists."
   
	git checkout $UATBRANCHNAME
      
fi

echo "checkout ohpnm-ssrs-p3 $UATBRANCHNAME completed"

if [ $(git tag -l "$INITIALTAGNAME") ]; then

    echo "Tag $INITIALTAGNAME already exists"
	
else

	git tag $INITIALTAGNAME

	git push origin $INITIALTAGNAME

fi

echo "Initial Tag $INITIALTAGNAME for repo ohpnm-ssrs-p3  branch $UATBRANCHNAME completed"

# tibco

cd $P3REPOPATH'ohpnm-tibco-p3\'

git remote set-url origin https://$TOKEN@github.com/ohpnm/ohpnm-tibco-p3

if [ `git branch --list $UATBRANCHNAME` ]
then
   
	echo "Branch name $UATBRANCHNAME already exists."
   
	git checkout $UATBRANCHNAME
      
fi

echo "checkout ohpnm-tibco-p3 $UATBRANCHNAME completed"

if [ $(git tag -l "$INITIALTAGNAME") ]; then

    echo "Tag $INITIALTAGNAME already exists"
	
else

	git tag $INITIALTAGNAME

	git push origin $INITIALTAGNAME

fi

echo "Initial Tag $INITIALTAGNAME for repo ohpnm-tibco-p3  branch $UATBRANCHNAME completed"