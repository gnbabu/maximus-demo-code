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

git checkout Dev

echo "checkout ohpnm-ssis DEV completed"

git remote set-url origin https://$TOKEN@github.com/ohpnm/ohpnm-ssis

echo "pulling from repo ohpnm-ssis" 

git pull

if [ `git branch --list $UATBRANCHNAME` ]
then
   
	echo "Branch name $UATBRANCHNAME already exists."
   
	git checkout $UATBRANCHNAME
      
else 

	echo "creating a new branch $UATBRANCHNAME in ohpnm-ssis" 

	git checkout -b $UATBRANCHNAME

	echo "pushing the new branch $UATBRANCHNAME in ohpnm-ssis to origin" 

	git push -u origin $UATBRANCHNAME

fi

echo "New Branch for repo ohpnm-ssis and branch $UATBRANCHNAME completed"

#src

cd $P4REPOPATH'ohpnm-src\'

git checkout Dev

echo "checkout ohpnm-src DEV completed"

git remote set-url origin https://$TOKEN@github.com/ohpnm/ohpnm-src

echo "pulling from repo ohpnm-src" 

git pull

if [ `git branch --list $UATBRANCHNAME` ]
then
   
	echo "Branch name $UATBRANCHNAME already exists."
   
	git checkout $UATBRANCHNAME
      
else 

	echo "creating a new branch $UATBRANCHNAME in ohpnm-src" 

	git checkout -b $UATBRANCHNAME

	echo "pushing the new branch $UATBRANCHNAME in ohpnm-src to origin" 

	git push -u origin $UATBRANCHNAME

fi

echo "New Branch for repo ohpnm-src and branch $UATBRANCHNAME completed"

#si-mq

cd $P4REPOPATH'ohpnm-si-mq\'

git checkout Dev

echo "checkout ohpnm-si-mq DEV completed"

git remote set-url origin https://$TOKEN@github.com/ohpnm/ohpnm-si-mq

echo "pulling from repo ohpnm-si-mq" 

git pull

if [ `git branch --list $UATBRANCHNAME` ]
then
   
	echo "Branch name $UATBRANCHNAME already exists."
   
	git checkout $UATBRANCHNAME
      
else 

	echo "creating a new branch $UATBRANCHNAME in ohpnm-si-mq" 

	git checkout -b $UATBRANCHNAME

	echo "pushing the new branch $UATBRANCHNAME in ohpnm-si-mq to origin" 

	git push -u origin $UATBRANCHNAME

fi

echo "New Branch for repo ohpnm-si-mq and branch $UATBRANCHNAME completed"

#tibco

cd $P4REPOPATH'ohpnm-tibco\'

git checkout Dev

echo "checkout ohpnm-tibco DEV completed"

git remote set-url origin https://$TOKEN@github.com/ohpnm/ohpnm-tibco

echo "pulling from repo ohpnm-tibco" 

git pull

if [ `git branch --list $UATBRANCHNAME` ]
then
   
	echo "Branch name $UATBRANCHNAME already exists."
   
	git checkout $UATBRANCHNAME
      
else 

	echo "creating a new branch $UATBRANCHNAME in ohpnm-tibco" 

	git checkout -b $UATBRANCHNAME

	echo "pushing the new branch $UATBRANCHNAME in ohpnm-tibco to origin" 

	git push -u origin $UATBRANCHNAME

fi

echo "New Branch for repo ohpnm-tibco and branch $UATBRANCHNAME completed"

#ssrs

cd $P4REPOPATH'ohpnm-ssrs\'

git checkout Dev

echo "checkout ohpnm-ssrs DEV completed"

git remote set-url origin https://$TOKEN@github.com/ohpnm/ohpnm-ssrs

echo "pulling from repo ohpnm-ssrs" 

git pull

if [ `git branch --list $UATBRANCHNAME` ]
then
   
	echo "Branch name $UATBRANCHNAME already exists."
   
	git checkout $UATBRANCHNAME
      
else 

	echo "creating a new branch $UATBRANCHNAME in ohpnm-ssrs" 

	git checkout -b $UATBRANCHNAME

	echo "pushing the new branch $UATBRANCHNAME in ohpnm-ssrs to origin" 

	git push -u origin $UATBRANCHNAME

fi

echo "New Branch for repo ohpnm-ssrs and branch $UATBRANCHNAME completed"