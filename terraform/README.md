# IELTS Platform - AWS Infrastructure with Terraform

This Terraform configuration creates a complete AWS infrastructure for the IELTS Platform, including:

## Infrastructure Components

### Networking
- **VPC**: Isolated network environment (10.0.0.0/16)
- **Public Subnets**: For ECS tasks and Load Balancer (2 AZs)
- **Private Subnets**: For RDS PostgreSQL database (2 AZs)
- **Internet Gateway**: For internet access
- **NAT Gateway**: For private subnets to access internet

### Container Registry
- **ECR Repositories**: 
  - API repository for .NET backend
  - Web repository for frontend
  - Automatic image scanning on push
  - Lifecycle policies to manage image retention

### Compute
- **ECS Cluster**: Fargate-based cluster for running containers
- **ECS Task Definitions**: 
  - API task (configurable CPU/Memory)
  - Web task (configurable CPU/Memory)
- **ECS Services**: Auto-scaling services with health checks

### Load Balancing
- **Application Load Balancer**: 
  - Public-facing ALB
  - HTTP listener on port 80
  - Path-based routing (/api/* → API, / → Web)
  - Target groups for each service

### Database
- **RDS PostgreSQL**: 
  - PostgreSQL 15.4
  - Private subnet deployment
  - Automated backups (7 days retention)
  - CloudWatch logs enabled

### Security
- **Security Groups**:
  - ALB: Allows HTTP/HTTPS from internet
  - ECS API: Allows traffic from ALB
  - ECS Web: Allows traffic from ALB
  - RDS: Allows PostgreSQL from ECS API only

- **IAM Roles**:
  - ECS Task Execution Role: Pull images from ECR, write logs
  - ECS Task Role: Application-level permissions (S3, Secrets Manager)

### Monitoring
- **CloudWatch Log Groups**: Centralized logging for API and Web services

## Prerequisites

1. **Terraform**: Install Terraform >= 1.0
   ```bash
   # Windows (using Chocolatey)
   choco install terraform
   
   # Or download from https://www.terraform.io/downloads
   ```

2. **AWS Account**: You need an AWS account with appropriate permissions

3. **AWS Credentials**: Configure your AWS credentials (replace mock values)

## Getting Started

### 1. Configure Credentials

Edit `terraform.tfvars` and replace the mock credentials:

```hcl
# Replace these with your actual AWS credentials
aws_access_key = "YOUR_ACTUAL_ACCESS_KEY"
aws_secret_key = "YOUR_ACTUAL_SECRET_KEY"

# Replace database credentials
db_username = "your_db_username"
db_password = "your_strong_db_password"
```

**⚠️ SECURITY WARNING**: Never commit `terraform.tfvars` to git! It's already in `.gitignore`.

### 2. Initialize Terraform

```bash
cd terraform
terraform init
```

This will download the required AWS provider plugins.

### 3. Review the Plan

```bash
terraform plan
```

Review the resources that will be created.

### 4. Apply the Configuration

```bash
terraform apply
```

Type `yes` when prompted. This will create all the infrastructure resources.

### 5. Get Output Values

After successful deployment:

```bash
terraform output
```

Important outputs:
- `load_balancer_url`: URL to access your application
- `ecr_api_repository_url`: ECR repository for API images
- `ecr_web_repository_url`: ECR repository for Web images
- `rds_endpoint`: PostgreSQL database endpoint (sensitive)

## Deploying Your Application

### 1. Build and Push Docker Images

**For API:**
```bash
# Login to ECR
aws ecr get-login-password --region us-east-1 | docker login --username AWS --password-stdin $(terraform output -raw ecr_api_repository_url | cut -d'/' -f1)

# Build and tag
docker build -t ielts-platform-api ./path-to-api
docker tag ielts-platform-api:latest $(terraform output -raw ecr_api_repository_url):latest

# Push to ECR
docker push $(terraform output -raw ecr_api_repository_url):latest
```

**For Web:**
```bash
# Build and tag
docker build -t ielts-platform-web ./path-to-web
docker tag ielts-platform-web:latest $(terraform output -raw ecr_web_repository_url):latest

# Push to ECR
docker push $(terraform output -raw ecr_web_repository_url):latest
```

### 2. Update ECS Services

After pushing images, ECS will automatically pull and deploy them. To force a new deployment:

```bash
aws ecs update-service --cluster ielts-platform-dev-cluster --service ielts-platform-dev-api-service --force-new-deployment
aws ecs update-service --cluster ielts-platform-dev-cluster --service ielts-platform-dev-web-service --force-new-deployment
```

## Configuration Variables

Key variables you can customize in `terraform.tfvars`:

| Variable | Description | Default |
|----------|-------------|---------|
| `aws_region` | AWS region | us-east-1 |
| `project_name` | Project name prefix | ielts-platform |
| `environment` | Environment (dev/staging/prod) | dev |
| `vpc_cidr` | VPC CIDR block | 10.0.0.0/16 |
| `db_instance_class` | RDS instance type | db.t3.micro |
| `api_cpu` | API task CPU units | 256 |
| `api_memory` | API task memory (MB) | 512 |
| `web_cpu` | Web task CPU units | 256 |
| `web_memory` | Web task memory (MB) | 512 |

## Cost Optimization

Current configuration is optimized for development. For production:

1. **RDS**: 
   - Enable Multi-AZ (`multi_az = true`)
   - Use larger instance class
   - Enable deletion protection

2. **ECS**: 
   - Increase task count for high availability
   - Consider using Fargate Spot for cost savings

3. **ALB**: 
   - Add HTTPS listener with SSL certificate
   - Enable access logs

## Cleanup

To destroy all resources:

```bash
terraform destroy
```

Type `yes` when prompted. This will delete all created resources.

**⚠️ WARNING**: This action cannot be undone! Make sure you have backups if needed.

## Troubleshooting

### ECS Tasks Not Starting

1. Check CloudWatch logs:
   ```bash
   aws logs tail /ecs/ielts-platform-dev/api --follow
   ```

2. Verify ECR images exist:
   ```bash
   aws ecr describe-images --repository-name ielts-platform-dev-api
   ```

### Database Connection Issues

1. Verify security group rules
2. Check RDS endpoint in outputs:
   ```bash
   terraform output rds_endpoint
   ```

### Load Balancer Not Accessible

1. Wait 5-10 minutes for ALB to become active
2. Check target health:
   ```bash
   aws elbv2 describe-target-health --target-group-arn <target-group-arn>
   ```

## Security Best Practices

1. **Credentials**: Use AWS Secrets Manager or Parameter Store instead of environment variables
2. **HTTPS**: Add SSL certificate and HTTPS listener for production
3. **Network**: Review security group rules regularly
4. **IAM**: Follow principle of least privilege
5. **Encryption**: Enable encryption at rest for all data stores

## Support

For issues or questions, please contact the infrastructure team.

