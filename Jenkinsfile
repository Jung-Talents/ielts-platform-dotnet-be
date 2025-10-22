pipeline {
    agent any
    
    environment {
        AWS_REGION = 'us-east-1'
        AWS_ACCOUNT_ID = credentials('aws-account-id')
        ECR_REPOSITORY = "${AWS_ACCOUNT_ID}.dkr.ecr.${AWS_REGION}.amazonaws.com/ielts-platform-dev-api"
        DOTNET_VERSION = '9.0'
        PROJECT_NAME = 'IeltsPlatform.ApiService'
        TERRAFORM_DIR = 'terraform'
    }
    
    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }
        
        stage('Install Dependencies') {
            steps {
                sh '''
                    dotnet --version
                    dotnet restore IeltsPlatform.sln
                '''
            }
        }
        
        stage('Build') {
            steps {
                sh '''
                    dotnet build IeltsPlatform.sln -c Release --no-restore
                '''
            }
        }
        
        stage('Test') {
            steps {
                sh '''
                    # Run tests if test projects exist
                    if [ -d "tests" ]; then
                        dotnet test IeltsPlatform.sln -c Release --no-build --verbosity normal
                    else
                        echo "No test projects found, skipping tests"
                    fi
                '''
            }
        }
        
        stage('Build Docker Image') {
            steps {
                script {
                    def imageTag = "${env.BUILD_NUMBER}"
                    def gitCommit = sh(returnStdout: true, script: 'git rev-parse --short HEAD').trim()
                    
                    sh """
                        docker build -f ${PROJECT_NAME}/Dockerfile \
                            -t ${ECR_REPOSITORY}:${imageTag} \
                            -t ${ECR_REPOSITORY}:${gitCommit} \
                            -t ${ECR_REPOSITORY}:latest \
                            .
                    """
                }
            }
        }
        
        stage('Push to ECR') {
            steps {
                withAWS(credentials: 'aws-credentials', region: "${AWS_REGION}") {
                    script {
                        def imageTag = "${env.BUILD_NUMBER}"
                        def gitCommit = sh(returnStdout: true, script: 'git rev-parse --short HEAD').trim()
                        
                        sh """
                            # Login to ECR
                            aws ecr get-login-password --region ${AWS_REGION} | docker login --username AWS --password-stdin ${AWS_ACCOUNT_ID}.dkr.ecr.${AWS_REGION}.amazonaws.com
                            
                            # Push images
                            docker push ${ECR_REPOSITORY}:${imageTag}
                            docker push ${ECR_REPOSITORY}:${gitCommit}
                            docker push ${ECR_REPOSITORY}:latest
                        """
                    }
                }
            }
        }
        
        stage('Terraform Plan') {
            when {
                branch 'main'
            }
            steps {
                withAWS(credentials: 'aws-credentials', region: "${AWS_REGION}") {
                    dir("${TERRAFORM_DIR}") {
                        sh """
                            terraform init
                            terraform plan \
                                -var="aws_region=${AWS_REGION}" \
                                -var="environment=dev" \
                                -var="container_image=${ECR_REPOSITORY}:${env.BUILD_NUMBER}" \
                                -out=tfplan
                        """
                    }
                }
            }
        }
        
        stage('Terraform Apply') {
            when {
                allOf {
                    branch 'main'
                    expression { 
                        return params.DEPLOY_TO_AWS == true 
                    }
                }
            }
            steps {
                withAWS(credentials: 'aws-credentials', region: "${AWS_REGION}") {
                    dir("${TERRAFORM_DIR}") {
                        sh """
                            terraform apply -auto-approve tfplan
                        """
                    }
                }
            }
        }
        
        stage('Deploy to ECS') {
            when {
                allOf {
                    branch 'main'
                    expression { 
                        return params.DEPLOY_TO_AWS == true 
                    }
                }
            }
            steps {
                withAWS(credentials: 'aws-credentials', region: "${AWS_REGION}") {
                    script {
                        def imageTag = "${env.BUILD_NUMBER}"
                        
                        sh """
                            # Update ECS service to use new task definition
                            aws ecs update-service \
                                --cluster ielts-platform-dev-cluster \
                                --service ielts-platform-dev-api \
                                --force-new-deployment \
                                --region ${AWS_REGION}
                        """
                    }
                }
            }
        }
    }
    
    post {
        always {
            cleanWs()
        }
        success {
            echo 'Pipeline completed successfully!'
        }
        failure {
            echo 'Pipeline failed!'
        }
    }
    
    parameters {
        booleanParam(
            name: 'DEPLOY_TO_AWS',
            defaultValue: false,
            description: 'Deploy to AWS ECS after successful build'
        )
    }
}
