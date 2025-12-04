# Quick Start Guide

## Prerequisites
- .NET 9 SDK
- Docker (optional, for containerization)
- AWS CLI (optional, for AWS deployment)
- Terraform (optional, for infrastructure)

## Local Development

### Run with .NET Aspire Dashboard
```bash
# Start all services with Aspire orchestration
cd IeltsPlatform.AppHost
dotnet run
```
This opens the Aspire dashboard showing all services and their health.

### Run API Service Only
```bash
cd IeltsPlatform.ApiService
dotnet run
```
API will be available at http://localhost:5000

### Run with Docker Compose
```bash
docker-compose up --build
```
API will be available at http://localhost:8080

## Testing Endpoints

### Health Check
```bash
curl http://localhost:5000/health
# Response: Healthy
```

### Weather Forecast (Sample Endpoint)
```bash
curl http://localhost:5000/weatherforecast
```

### OpenAPI Documentation (Development Mode)
```bash
curl http://localhost:5000/openapi/v1.json
```

## Building

### Build Solution
```bash
dotnet build
```

### Build for Release
```bash
dotnet build -c Release
```

### Run Tests (when available)
```bash
dotnet test
```

## Docker Operations

### Build Image
```bash
docker build -f IeltsPlatform.ApiService/Dockerfile -t ielts-platform-api:latest .
```

### Run Container
```bash
docker run -p 8080:8080 \
  -e ASPNETCORE_ENVIRONMENT=Development \
  -e AWS_REGION=ap-southeast-2 \
  ielts-platform-api:latest
```

## AWS Deployment

### Setup Terraform Backend (One-time)
```bash
# Create S3 bucket for state
aws s3 mb s3://your-terraform-state-bucket --region ap-southeast-2

# Create DynamoDB table for state locking
aws dynamodb create-table \
  --table-name terraform-state-lock \
  --attribute-definitions AttributeName=LockID,AttributeType=S \
  --key-schema AttributeName=LockID,KeyType=HASH \
  --billing-mode PAY_PER_REQUEST \
  --region ap-southeast-2
```

### Deploy Infrastructure
```bash
cd terraform

# Initialize Terraform
terraform init \
  -backend-config="bucket=your-terraform-state-bucket" \
  -backend-config="key=ielts-platform/terraform.tfstate" \
  -backend-config="region=ap-southeast-2"

# Plan changes
terraform plan -var="environment=dev" -out=tfplan

# Apply changes
terraform apply tfplan
```

### Get Infrastructure Outputs
```bash
terraform output alb_url
terraform output ecr_repository_url
```

### Push Image to ECR
```bash
# Login to ECR
aws ecr get-login-password --region ap-southeast-2 | \
  docker login --username AWS --password-stdin <account-id>.dkr.ecr.ap-southeast-2.amazonaws.com

# Tag image
docker tag ielts-platform-api:latest \
  <account-id>.dkr.ecr.ap-southeast-2.amazonaws.com/ielts-platform-dev-api:latest

# Push image
docker push <account-id>.dkr.ecr.ap-southeast-2.amazonaws.com/ielts-platform-dev-api:latest
```

### Update ECS Service
```bash
aws ecs update-service \
  --cluster ielts-platform-dev-cluster \
  --service ielts-platform-dev-api \
  --force-new-deployment \
  --region ap-southeast-2
```

## Common Tasks

### Add New NuGet Package
```bash
cd IeltsPlatform.ApiService
dotnet add package PackageName
```

### Update Packages
```bash
dotnet list package --outdated
dotnet add package PackageName --version x.y.z
```

### Check Service Status in AWS
```bash
# ECS Service
aws ecs describe-services \
  --cluster ielts-platform-dev-cluster \
  --services ielts-platform-dev-api \
  --region ap-southeast-2

# View logs
aws logs tail /ecs/ielts-platform-dev-api --follow
```

### Cleanup AWS Resources
```bash
cd terraform
terraform destroy -var="environment=dev"
```

## Environment Variables

### For Local Development
```bash
export AWS_REGION=ap-southeast-2
export ASPNETCORE_ENVIRONMENT=Development
```

### For Docker
Add to docker-compose.yml or pass via -e flag:
- `ASPNETCORE_ENVIRONMENT`: Development, Staging, or Production
- `AWS_REGION`: AWS region for service calls
- `AWS_ACCESS_KEY_ID`: AWS access key (if not using IAM roles)
- `AWS_SECRET_ACCESS_KEY`: AWS secret key (if not using IAM roles)

## Troubleshooting

### Build Issues
```bash
# Clean solution
dotnet clean

# Restore packages
dotnet restore

# Rebuild
dotnet build
```

### Docker Issues
```bash
# Remove all containers and images
docker-compose down -v
docker system prune -a
```

### AWS Connection Issues
```bash
# Check AWS credentials
aws sts get-caller-identity

# Check AWS region
echo $AWS_REGION
```

## Jenkins Setup

### Required Plugins
- AWS Steps Plugin
- Docker Pipeline Plugin
- Pipeline Plugin

### Required Credentials in Jenkins
- `aws-credentials`: AWS Access Key and Secret Key
- `aws-account-id`: Your AWS Account ID

### Trigger Build
Push to the repository or manually trigger via Jenkins UI with:
- Parameter: `DEPLOY_TO_AWS` (check to deploy to AWS)

## Monitoring

### View Application Logs
```bash
# AWS CloudWatch
aws logs tail /ecs/ielts-platform-dev-api --follow --region ap-southeast-2
```

### Check Container Health
```bash
# ECS
aws ecs describe-tasks \
  --cluster ielts-platform-dev-cluster \
  --tasks $(aws ecs list-tasks --cluster ielts-platform-dev-cluster --service ielts-platform-dev-api --query 'taskArns[0]' --output text) \
  --region ap-southeast-2
```

## Additional Resources
- [.NET 9 Documentation](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-9)
- [.NET Aspire Documentation](https://learn.microsoft.com/en-us/dotnet/aspire/)
- [AWS ECS Documentation](https://docs.aws.amazon.com/ecs/)
- [Terraform AWS Provider](https://registry.terraform.io/providers/hashicorp/aws/latest/docs)
