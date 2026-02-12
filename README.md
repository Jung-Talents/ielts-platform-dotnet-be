# IELTS Platform - .NET 9 API Backend

A modern, cloud-native IELTS platform backend built with .NET 9, .NET Aspire, AWS services, and infrastructure as code using Terraform.

## 🚀 Features

- **Modern Architecture**: Built with .NET 9 and .NET Aspire for cloud-native development
- **AWS Integration**: Leverages AWS services including ECS Fargate, S3, and DynamoDB
- **Infrastructure as Code**: Complete Terraform configuration for AWS infrastructure
- **CI/CD Pipeline**: GitHub Actions workflows for automated build, test, and deployment
- **Containerization**: Docker support for consistent deployments
- **Health Checks**: Built-in health monitoring endpoints
- **Observability**: CloudWatch logging and ECS Container Insights

## 📋 Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker](https://www.docker.com/get-started)
- [Terraform](https://www.terraform.io/downloads) (>= 1.0)
- [AWS CLI](https://aws.amazon.com/cli/)
- AWS Account with appropriate permissions

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
├── .github/workflows/             # GitHub Actions workflows
│   ├── ci-cd.yml                  # Main CI/CD pipeline
│   ├── pr-check.yml               # Pull request checks
│   └── README.md                  # Workflows documentation
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
terraform plan -var="aws_region=ap-southeast-2" -var="environment=dev"
```

4. Apply the infrastructure:
```bash
terraform apply -var="aws_region=ap-southeast-2" -var="environment=dev"
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
aws ecr get-login-password --region ap-southeast-2 | docker login --username AWS --password-stdin <account-id>.dkr.ecr.ap-southeast-2.amazonaws.com
docker build -f IeltsPlatform.ApiService/Dockerfile -t <account-id>.dkr.ecr.ap-southeast-2.amazonaws.com/ielts-platform-dev-api:latest .
```

2. Push to ECR:
```bash
docker push <account-id>.dkr.ecr.ap-southeast-2.amazonaws.com/ielts-platform-dev-api:latest
```

3. Update ECS service:
```bash
aws ecs update-service --cluster ielts-platform-dev-cluster --service ielts-platform-dev-api --force-new-deployment --region ap-southeast-2
```

## 🔄 CI/CD with GitHub Actions

### GitHub Actions Setup

1. Configure GitHub Secrets (Settings → Secrets and variables → Actions):
   - `AWS_ACCESS_KEY_ID`: AWS access key ID
   - `AWS_SECRET_ACCESS_KEY`: AWS secret access key

2. (Optional) Set up production environment with required reviewers

For detailed configuration, see [.github/workflows/README.md](.github/workflows/README.md)

📘 **New to GitHub Actions?** Check out the [Quick Start Guide](GITHUB_ACTIONS_QUICKSTART.md)

### Workflows

The GitHub Actions pipeline includes:

#### CI/CD Pipeline (`ci-cd.yml`)
1. **build-and-test**: Restore dependencies, build, and test
2. **build-docker**: Build and push Docker image to ECR
3. **terraform-plan**: Plan infrastructure changes
4. **deploy-infrastructure**: Apply infrastructure changes (manual trigger only)
5. **deploy-ecs**: Update ECS service (manual trigger only)

#### Pull Request Check (`pr-check.yml`)
- Runs on all pull requests
- Builds and tests code
- Verifies Docker image can be built

### Triggering Deployment

- **Automatic**: Builds and pushes Docker images on push to `main` or `develop` branches
- **Manual Deployment**: 
  1. Go to Actions tab → CI/CD Pipeline
  2. Click "Run workflow"
  3. Select branch and check "Deploy to AWS ECS"
  4. Click "Run workflow"

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
terraform destroy -var="aws_region=ap-southeast-2" -var="environment=dev"
```

**Note**: This will delete all resources including data in DynamoDB and S3. Use with caution!
