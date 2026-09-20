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

cd $P3REPOPATH'ohpnm-ssis-p3\'

if [ `git branch --list $UATBRANCHNAME` ]
then

	git checkout $UATBRANCHNAME

fi

echo "checkout ohpnm-ssis-p3 $UATBRANCHNAME completed"

#ssis

cd $P3REPOPATH'ohpnm-ssis-p3\'

if [ `git branch --list $UATBRANCHNAME` ]
then

	git checkout $UATBRANCHNAME

fi

echo "checkout ohpnm-ssis-p3 $UATBRANCHNAME completed"

#src

cd $P3REPOPATH'ohpnm-src-p3\'

if [ `git branch --list $UATBRANCHNAME` ]
then

	git checkout $UATBRANCHNAME

fi

echo "checkout ohpnm-src-p3 $UATBRANCHNAME completed"

#si-mq

cd $P3REPOPATH'ohpnm-si-mq-p3\'

if [ `git branch --list $UATBRANCHNAME` ]
then

	git checkout $UATBRANCHNAME

fi

echo "checkout ohpnm-si-mq-p3 $UATBRANCHNAME completed"

#ssrs

cd $P3REPOPATH'ohpnm-ssrs-p3\'

if [ `git branch --list $UATBRANCHNAME` ]
then

	git checkout $UATBRANCHNAME

fi

echo "checkout ohpnm-ssrs-p3 $UATBRANCHNAME completed"

#tibco

cd $P3REPOPATH'ohpnm-tibco-p3\'

if [ `git branch --list $UATBRANCHNAME` ]
then

	git checkout $UATBRANCHNAME

fi

echo "checkout ohpnm-tibco-p3 $UATBRANCHNAME completed"