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

#src

cd $P3REPOPATH'ohpnm-src-p3\'

git checkout Dev

echo "checkout ohpnm-src-p3 DEV completed"

#si-mq

cd $P3REPOPATH'ohpnm-si-mq-p3\'

git checkout Dev

echo "checkout ohpnm-si-mq-p3 DEV completed"

#ssrs

cd $P3REPOPATH'ohpnm-ssrs-p3\'

git checkout Dev

echo "checkout ohpnm-ssrs-p3 DEV completed"

#tibco

cd $P3REPOPATH'ohpnm-tibco-p3\'

git checkout Dev

echo "checkout ohpnm-tibco-p3 DEV completed"