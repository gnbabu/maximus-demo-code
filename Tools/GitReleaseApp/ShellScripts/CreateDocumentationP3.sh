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
DATE=$(date '+%Y-%m-%d')


cd $P3REPOPATH

#ssis

cd $P3REPOPATH'ohpnm-ssis-p3\'

git checkout Dev

git remote set-url origin https://$TOKEN@github.com/ohpnm/ohpnm-ssis-p3

echo "pulling from repo ohpnm-ssis-p3" 

git pull

git log --first-parent --pretty='format:%Creset%s%C(cyan) [%cn]' --decorate --after="$STARTDATE $STARTDATETIME" --before="$ENDDATE $ENDDATETIME" > $COMMITHISTPATH'P3SSIS.txt'

echo "Documentation ohpnm-ssis-p3 DEV completed"

#src

cd $P3REPOPATH'ohpnm-src-p3\'

git checkout Dev

git remote set-url origin https://$TOKEN@github.com/ohpnm/ohpnm-src-p3

echo "pulling from repo ohpnm-src-p3" 

git pull

git log --first-parent --pretty='format:%Creset%s%C(cyan) [%cn]' --decorate --after="$STARTDATE $STARTDATETIME" --before="$ENDDATE $ENDDATETIME" > $COMMITHISTPATH'P3SRC.txt'

echo "Documentation ohpnm-src-p3 DEV completed"

#si-mq

cd $P3REPOPATH'ohpnm-si-mq-p3\'

git checkout Dev

git remote set-url origin https://$TOKEN@github.com/ohpnm/ohpnm-si-mq-p3

echo "pulling from repo ohpnm-si-mq-p3" 

git pull

git log --first-parent --pretty='format:%Creset%s%C(cyan) [%cn]' --decorate --after="$STARTDATE $STARTDATETIME" --before="$ENDDATE $ENDDATETIME" > $COMMITHISTPATH'P3SIMQ.txt'

echo "Documentation ohpnm-si-mq-p3 DEV completed"

#ssrs

cd $P3REPOPATH'ohpnm-ssrs-p3\'

git checkout Dev

git remote set-url origin https://$TOKEN@github.com/ohpnm/ohpnm-ssrs-p3

echo "pulling from repo ohpnm-ssrs-p3" 

git pull

git log --first-parent --pretty='format:%Creset%s%C(cyan) [%cn]' --decorate --after="$STARTDATE $STARTDATETIME" --before="$ENDDATE $ENDDATETIME" > $COMMITHISTPATH'P3SSRS.txt'

echo "Documentation ohpnm-ssrs-p3 DEV completed"

#tibco

cd $P3REPOPATH'ohpnm-tibco-p3\'

git checkout Dev

git remote set-url origin https://$TOKEN@github.com/ohpnm/ohpnm-tibco-p3

echo "pulling from repo ohpnm-tibco-p3" 

git pull

git log --first-parent --pretty='format:%Creset%s%C(cyan) [%cn]' --decorate --after="$STARTDATE $STARTDATETIME" --before="$ENDDATE $ENDDATETIME" > $COMMITHISTPATH'P3TIBCO.txt'

echo "Documentation ohpnm-tibco-p3 DEV completed"

cd $COMMITHISTPATH

cat P3*.txt > MergedP3V_$DATE.txt