# IELTS Platform - .NET 9 API Backend

A modern, cloud-native IELTS platform backend built with .NET 9, .NET Aspire, AWS services, and infrastructure as code using Terraform.

## 🚀 Features

- **Modern Architecture**: Built with .NET 9 and .NET Aspire for cloud-native development
- **AWS Integration**: Leverages AWS services including ECS Fargate, S3, and DynamoDB
- **Infrastructure as Code**: Complete Terraform configuration for AWS infrastructure
- **CI/CD Pipeline**: Jenkins pipeline for automated build, test, and deployment
- **Containerization**: Docker support for consistent deployments
- **Health Checks**: Built-in health monitoring endpoints
- **Observability**: CloudWatch logging and ECS Container Insights

## 📋 Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker](https://www.docker.com/get-started)
- [Terraform](https://www.terraform.io/downloads) (>= 1.0)
- [AWS CLI](https://aws.amazon.com/cli/)
- AWS Account with appropriate permissions
- Jenkins (for CI/CD)

## 🏗️ Project Structure

```
├── IeltsPlatform.ApiService/       # Main API service
├── IeltsPlatform.AppHost/          # Aspire orchestration
├── IeltsPlatform.ServiceDefaults/  # Shared service configurations
├── IeltsPlatform.Web/              # Web frontend (Blazor)
├── terraform/                      # AWS infrastructure definitions
│   ├── main.tf                    # Provider configuration
│   ├── variables.tf               # Input variables
│   ├── outputs.tf                 # Output values
│   ├── vpc.tf                     # VPC and networking
│   ├── ecs.tf                     # ECS, ECR, and container definitions
│   └── alb.tf                     # Application Load Balancer
├── Jenkinsfile                     # CI/CD pipeline
└── IeltsPlatform.sln              # Solution file
```

## 🛠️ Local Development

### Running with .NET Aspire

1. Clone the repository:
```bash
git clone https://github.com/Jung-Talents/ielts-platform-dotnet-be.git
cd ielts-platform-dotnet-be
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Build the solution:
```bash
dotnet build
```

4. Run the Aspire AppHost:
```bash
cd IeltsPlatform.AppHost
dotnet run
```

This will start the Aspire dashboard and all services.

### Running the API Service Directly

```bash
cd IeltsPlatform.ApiService
dotnet run
```

The API will be available at `http://localhost:5000` (or the port specified in launch settings).

### Running with Docker

Build the Docker image:
```bash
docker build -f IeltsPlatform.ApiService/Dockerfile -t ielts-platform-api:latest .
```

Run the container:
```bash
docker run -p 8080:8080 ielts-platform-api:latest
```

## ☁️ AWS Deployment

### Infrastructure Setup with Terraform

1. Navigate to the terraform directory:
```bash
cd terraform
```

2. Initialize Terraform:
```bash
terraform init
```

3. Review the planned changes:
```bash
terraform plan -var="aws_region=us-east-1" -var="environment=dev"
```

4. Apply the infrastructure:
```bash
terraform apply -var="aws_region=us-east-1" -var="environment=dev"
```

### Infrastructure Components

The Terraform configuration creates:

- **VPC**: Multi-AZ VPC with public and private subnets
- **ECS Cluster**: Fargate cluster for running containers
- **ECR Repository**: Docker image repository
- **Application Load Balancer**: Public-facing load balancer
- **Security Groups**: Network security configurations
- **IAM Roles**: Permissions for ECS tasks
- **CloudWatch**: Log groups for application logs

### Deploy Application to ECS

1. Build and tag the Docker image:
```bash
aws ecr get-login-password --region us-east-1 | docker login --username AWS --password-stdin <account-id>.dkr.ecr.us-east-1.amazonaws.com
docker build -f IeltsPlatform.ApiService/Dockerfile -t <account-id>.dkr.ecr.us-east-1.amazonaws.com/ielts-platform-dev-api:latest .
```

2. Push to ECR:
```bash
docker push <account-id>.dkr.ecr.us-east-1.amazonaws.com/ielts-platform-dev-api:latest
```

3. Update ECS service:
```bash
aws ecs update-service --cluster ielts-platform-dev-cluster --service ielts-platform-dev-api --force-new-deployment --region us-east-1
```

## 🔄 CI/CD with Jenkins

### Jenkins Setup

1. Install required Jenkins plugins:
   - AWS Steps Plugin
   - Docker Pipeline Plugin
   - Pipeline Plugin

2. Configure credentials in Jenkins:
   - `aws-credentials`: AWS access key and secret key
   - `aws-account-id`: AWS account ID

3. Create a new Pipeline job and point it to the repository

### Pipeline Stages

The Jenkins pipeline includes:

1. **Checkout**: Clone the repository
2. **Install Dependencies**: Restore NuGet packages
3. **Build**: Compile the solution
4. **Test**: Run unit tests
5. **Build Docker Image**: Create container image
6. **Push to ECR**: Upload to AWS ECR
7. **Terraform Plan**: Review infrastructure changes
8. **Terraform Apply**: Apply infrastructure changes (main branch only)
9. **Deploy to ECS**: Update ECS service (main branch only)

### Triggering Deployment

- Automatic builds trigger on code push
- Deployment to AWS requires manual approval via the `DEPLOY_TO_AWS` parameter

## 🔍 API Endpoints

### Health Check
```
GET /health
```

### Weather Forecast (Example)
```
GET /weatherforecast
```

### OpenAPI Documentation (Development)
```
GET /openapi/v1.json
```

## 🌐 AWS Services Integration

The API is configured to work with:

- **Amazon S3**: Object storage
- **Amazon DynamoDB**: NoSQL database
- **Amazon CloudWatch**: Logging and monitoring
- **AWS Secrets Manager**: Secure secret storage (recommended for production)

## 📊 Monitoring and Logging

- **CloudWatch Logs**: Application logs are sent to `/ecs/ielts-platform-dev-api`
- **Container Insights**: ECS cluster and service metrics
- **Health Checks**: ALB health checks on `/health` endpoint

## 🔐 Security Considerations

- ECS tasks run in private subnets
- AWS IAM roles for task execution and application access
- Security groups restrict network access
- ECR image scanning enabled
- Secrets should be managed via AWS Secrets Manager or Parameter Store

## 🚀 Scaling

The infrastructure supports horizontal scaling:

1. Adjust `desired_count` in Terraform variables
2. Configure auto-scaling policies (add to Terraform as needed)

## 📝 Environment Variables

Key environment variables for the API:

- `ASPNETCORE_ENVIRONMENT`: Development, Staging, or Production
- `AWS_REGION`: AWS region for service calls
- `ASPNETCORE_URLS`: HTTP binding URL

## 🧪 Testing

Run all tests:
```bash
dotnet test
```

Run specific test project:
```bash
dotnet test path/to/test/project
```

## 📦 Package Management

Update dependencies:
```bash
dotnet list package --outdated
dotnet add package <PackageName>
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Run tests and ensure they pass
5. Submit a pull request

## 📄 License

See LICENSE file for details.

## 🆘 Support

For issues and questions:
- Open an issue on GitHub
- Check existing documentation
- Review CloudWatch logs for runtime errors

## 🔄 Cleanup

To destroy all AWS resources:
```bash
cd terraform
terraform destroy -var="aws_region=us-east-1" -var="environment=dev"
```

**Note**: This will delete all resources including data in DynamoDB and S3. Use with caution!
