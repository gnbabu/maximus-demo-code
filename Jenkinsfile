def projectBranchName = '*/Dev'

pipeline {
  agent any
  stages {
    stage('Checkout') {
      parallel {
        stage('info') {
          steps {
            echo 'Checkout the latest software from Dev branch'
          }
        }

        stage('ohpnm-src') {
          steps {
            catchError(message: 'Failed checkout repo ohpnm-src for slected tag https://github.com/ohpnm/ohpnm-src/commits/Dev', stageResult: 'FAILURE') {
              checkout([$class: 'GitSCM', branches: [
                [name: projectBranchName]
              ], extensions: [
                [$class: 'CheckoutOption', timeout: 5],
                [$class: 'RelativeTargetDirectory', relativeTargetDir: 'ohpnm-src']
              ], userRemoteConfigs: [
                [credentialsId: 'github-token-ohpnm', url: 'https://github.com/ohpnm/ohpnm-src.git']
              ]])
            }
          }
          post {
            failure {
              error 'Failed checkout repo ohpnm-src for slected tag https://github.com/ohpnm/ohpnm-src/commits/Dev'
            }
          }
        }
      }
    }

    stage('Build') {
      parallel {
        stage('info') {
          steps {
            echo 'Build the software'
          }
        }
        stage('ohpnm-ssrs') {
          steps {
            catchError(message: 'Failed Building Source Code - Check for recent commits in GitHub Repo https://github.com/ohpnm/ohpnm-src/commits/Dev', stageResult: 'FAILURE') {
              bat "\"${tool 'MSBuildVS2019'}\" ohpnm-src/PDMS/PDMS.sln /t:clean /p:Configuration=Release"
              bat "\"${tool 'MSBuildVS2019'}\" ohpnm-src/PDMS/PDMS.sln /t:Build -t:restore /m /p:GenerateBuildInfoConfigFile=false /p:Configuration=Release /p:VisualStudioVersion=16.0"
              bat "\"${tool 'MSBuildVS2019'}\" ohpnm-src/PDMS/PDMS/website.publishproj /p:DeployOnBuild=true /p:PublishProfile=AWSDev /p:VisualStudioVersion=16.0 /p:Configuration=Release /p:Platform=AnyCPU"
              bat "\"${tool 'MSBuildVS2019'}\" ohpnm-src/PDMS/PDMS.sln /p:DeployOnBuild=true /p:PublishProfile=AWSDev /p:VisualStudioVersion=16.0 /p:Configuration=Release"
            }
          }
          post {
            failure {
              error 'Failed Building Source Code - Check for recent commits in GitHub Repo https://github.com/ohpnm/ohpnm-src/commits/Dev'
            }
          }
        }
      }
    }
  }
}
