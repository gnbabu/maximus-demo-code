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

git checkout Dev

echo "checkout ohpnm-ssis-p3 DEV completed"

git remote set-url origin https://$TOKEN@github.com/ohpnm/ohpnm-ssis-p3

echo "pulling from repo ohpnm-ssis-p3" 

git pull

if [ `git branch --list $UATBRANCHNAME` ]
then
   
	echo "Branch name $UATBRANCHNAME already exists."
   
	git checkout $UATBRANCHNAME
      
else 

	echo "creating a new branch $UATBRANCHNAME in ohpnm-ssis-p3" 

	git checkout -b $UATBRANCHNAME

	echo "pushing the new branch $UATBRANCHNAME in ohpnm-ssis-p3 to origin" 

	git push -u origin $UATBRANCHNAME

fi

echo "New Branch for repo ohpnm-ssis-p3 and branch $UATBRANCHNAME completed"

#src

cd $P3REPOPATH'ohpnm-src-p3\'

git checkout Dev

echo "checkout ohpnm-src-p3 DEV completed"

git remote set-url origin https://$TOKEN@github.com/ohpnm/ohpnm-src-p3

echo "pulling from repo ohpnm-src-p3" 

git pull

if [ `git branch --list $UATBRANCHNAME` ]
then
   
	echo "Branch name $UATBRANCHNAME already exists."
   
	git checkout $UATBRANCHNAME
      
else 

	echo "creating a new branch $UATBRANCHNAME in ohpnm-src-p3" 

	git checkout -b $UATBRANCHNAME

	echo "pushing the new branch $UATBRANCHNAME in ohpnm-src-p3 to origin" 

	git push -u origin $UATBRANCHNAME

fi

echo "New Branch for repo ohpnm-src-p3 and branch $UATBRANCHNAME completed"

#si-mq

cd $P3REPOPATH'ohpnm-si-mq-p3\'

git checkout Dev

echo "checkout ohpnm-si-mq-p3 DEV completed"

git remote set-url origin https://$TOKEN@github.com/ohpnm/ohpnm-si-mq-p3

echo "pulling from repo ohpnm-si-mq-p3" 

git pull

if [ `git branch --list $UATBRANCHNAME` ]
then
   
	echo "Branch name $UATBRANCHNAME already exists."
   
	git checkout $UATBRANCHNAME
      
else 

	echo "creating a new branch $UATBRANCHNAME in ohpnm-si-mq-p3" 

	git checkout -b $UATBRANCHNAME

	echo "pushing the new branch $UATBRANCHNAME in ohpnm-si-mq-p3 to origin" 

	git push -u origin $UATBRANCHNAME

fi

echo "New Branch for repo ohpnm-si-mq-p3 and branch $UATBRANCHNAME completed"

#ssrs

cd $P3REPOPATH'ohpnm-ssrs-p3\'

git checkout Dev

echo "checkout ohpnm-ssrs-p3 DEV completed"

git remote set-url origin https://$TOKEN@github.com/ohpnm/ohpnm-ssrs-p3

echo "pulling from repo ohpnm-ssrs-p3" 

git pull

if [ `git branch --list $UATBRANCHNAME` ]
then
   
	echo "Branch name $UATBRANCHNAME already exists."
   
	git checkout $UATBRANCHNAME
      
else 

	echo "creating a new branch $UATBRANCHNAME in ohpnm-ssrs-p3" 

	git checkout -b $UATBRANCHNAME

	echo "pushing the new branch $UATBRANCHNAME in ohpnm-ssrs-p3 to origin" 

	git push -u origin $UATBRANCHNAME

fi

echo "New Branch for repo ohpnm-ssrs-p3 and branch $UATBRANCHNAME completed"

#tibco

cd $P3REPOPATH'ohpnm-tibco-p3\'

git checkout Dev

echo "checkout ohpnm-tibco-p3 DEV completed"

git remote set-url origin https://$TOKEN@github.com/ohpnm/ohpnm-tibco-p3

echo "pulling from repo ohpnm-tibco-p3" 

git pull

if [ `git branch --list $UATBRANCHNAME` ]
then
   
	echo "Branch name $UATBRANCHNAME already exists."
   
	git checkout $UATBRANCHNAME
      
else 

	echo "creating a new branch $UATBRANCHNAME in ohpnm-tibco-p3" 

	git checkout -b $UATBRANCHNAME

	echo "pushing the new branch $UATBRANCHNAME in ohpnm-tibco-p3 to origin" 

	git push -u origin $UATBRANCHNAME

fi

echo "New Branch for repo ohpnm-tibco-p3 and branch $UATBRANCHNAME completed"