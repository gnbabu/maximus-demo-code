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
DATE=$(date '+%Y-%m-%d')

cd $P4REPOPATH

#ssis

cd $P4REPOPATH'ohpnm-ssis\'

git checkout Dev

git remote set-url origin https://$TOKEN@github.com/ohpnm/ohpnm-ssis

echo "pulling from repo ohpnm-ssis" 

git pull

git log --first-parent --pretty='format:%Creset%s%C(cyan) [%cn]' --decorate --after="$STARTDATE $STARTDATETIME" --before="$ENDDATE $ENDDATETIME" > $COMMITHISTPATH'P4SSIS.txt'

echo "Documentation ohpnm-ssis DEV completed"

#src

cd $P4REPOPATH'ohpnm-src\'

git checkout Dev

git remote set-url origin https://$TOKEN@github.com/ohpnm/ohpnm-src

echo "pulling from repo ohpnm-src" 

git pull

git log --first-parent --pretty='format:%Creset%s%C(cyan) [%cn]' --decorate --after="$STARTDATE $STARTDATETIME" --before="$ENDDATE $ENDDATETIME" > $COMMITHISTPATH'P4SRC.txt'

echo "Documentation ohpnm-src DEV completed"

#si-mq

cd $P4REPOPATH'ohpnm-si-mq\'

git checkout Dev

git remote set-url origin https://$TOKEN@github.com/ohpnm/ohpnm-si-mq

echo "pulling from repo ohpnm-si-mq" 

git pull

git log --first-parent --pretty='format:%Creset%s%C(cyan) [%cn]' --decorate --after="$STARTDATE $STARTDATETIME" --before="$ENDDATE $ENDDATETIME" > $COMMITHISTPATH'P4SIMQ.txt'

echo "Documentation ohpnm-si-mq DEV completed"

#ssrs

cd $P4REPOPATH'ohpnm-ssrs\'

git checkout Dev

git remote set-url origin https://$TOKEN@github.com/ohpnm/ohpnm-ssrs

echo "pulling from repo ohpnm-ssrs" 

git pull

git log --first-parent --pretty='format:%Creset%s%C(cyan) [%cn]' --decorate --after="$STARTDATE $STARTDATETIME" --before="$ENDDATE $ENDDATETIME" > $COMMITHISTPATH'P4SSRS.txt'

echo "Documentation ohpnm-ssrs DEV completed"

#tibco

cd $P4REPOPATH'ohpnm-tibco\'

git checkout Dev

git remote set-url origin https://$TOKEN@github.com/ohpnm/ohpnm-tibco

echo "pulling from repo ohpnm-tibco" 

git pull

git log --first-parent --pretty='format:%Creset%s%C(cyan) [%cn]' --decorate --after="$STARTDATE $STARTDATETIME" --before="$ENDDATE $ENDDATETIME" > $COMMITHISTPATH'P4TIBCO.txt'

echo "Documentation ohpnm-tibco DEV completed"

cd $COMMITHISTPATH

cat P4*.txt > MergedP4V_$DATE.txt