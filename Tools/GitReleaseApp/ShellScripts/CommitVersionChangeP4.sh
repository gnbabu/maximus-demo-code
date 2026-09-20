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

#src

cd $P4REPOPATH'ohpnm-src\'

if [ `git branch --list $UATBRANCHNAME` ]
then

	git checkout $UATBRANCHNAME

fi

echo "checkout ohpnm-src $UATBRANCHNAME completed"

git remote set-url origin https://$TOKEN@github.com/ohpnm/ohpnm-src

echo "pulling from repo ohpnm-src" 

git pull

git add "PDMS/PDMS/web.config"

git commit -m "Version Change"

git push -u origin $UATBRANCHNAME