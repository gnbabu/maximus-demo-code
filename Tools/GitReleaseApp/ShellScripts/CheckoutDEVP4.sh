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

#src

cd $P4REPOPATH'ohpnm-src\'

git checkout Dev

echo "checkout ohpnm-src DEV completed"

#si-mq

cd $P4REPOPATH'ohpnm-si-mq\'

git checkout Dev

echo "checkout ohpnm-si-mq DEV completed"

#ssrs

cd $P4REPOPATH'ohpnm-ssrs\'

git checkout Dev

echo "checkout ohpnm-ssrs DEV completed"

#tibco

cd $P4REPOPATH'ohpnm-tibco\'

git checkout Dev

echo "checkout ohpnm-tibco DEV completed"