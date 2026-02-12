# GitHub Actions Configuration Guide

This document describes the GitHub Actions workflows and required secrets for the IELTS Platform CI/CD pipeline.

## Workflows

### 1. CI/CD Pipeline (`ci-cd.yml`)

The main CI/CD pipeline that runs on push to `main` and `develop` branches, as well as on workflow dispatch.

**Triggers:**
- Push to `main` or `develop` branches
- Manual workflow dispatch with deployment option

**Jobs:**
1. **build-and-test**: Restores dependencies, builds the solution, and runs tests
2. **build-docker**: Builds Docker image and pushes to Amazon ECR
3. **terraform-plan**: Plans infrastructure changes using Terraform
4. **deploy-infrastructure**: Applies Terraform changes (manual trigger only)
5. **deploy-ecs**: Updates ECS service with new Docker image (manual trigger only)

**Manual Deployment:**
To deploy to AWS:
1. Go to Actions tab in GitHub
2. Select "CI/CD Pipeline" workflow
3. Click "Run workflow"
4. Check "Deploy to AWS ECS" checkbox
5. Click "Run workflow" button

### 2. Pull Request Check (`pr-check.yml`)

Runs on pull requests to ensure code quality before merging.

**Triggers:**
- Pull request opened, synchronized, or reopened

**Jobs:**
1. **build-and-test**: Builds and tests the code, verifies Docker image can be built

## Required GitHub Secrets

Configure the following secrets in your GitHub repository settings (Settings → Secrets and variables → Actions):

### AWS Credentials

| Secret Name | Description | Example |
|------------|-------------|---------|
| `AWS_ACCESS_KEY_ID` | AWS Access Key ID with permissions for ECR and ECS | `AKIAIOSFODNN7EXAMPLE` |
| `AWS_SECRET_ACCESS_KEY` | AWS Secret Access Key | `wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY` |

### IAM Permissions Required

The AWS credentials must have the following permissions:

#### ECR (Elastic Container Registry)
- `ecr:GetAuthorizationToken`
- `ecr:BatchCheckLayerAvailability`
- `ecr:GetDownloadUrlForLayer`
- `ecr:BatchGetImage`
- `ecr:PutImage`
- `ecr:InitiateLayerUpload`
- `ecr:UploadLayerPart`
- `ecr:CompleteLayerUpload`

#### ECS (Elastic Container Service)
- `ecs:UpdateService`
- `ecs:DescribeServices`
- `ecs:DescribeClusters`

#### Terraform (for infrastructure deployment)
- Full permissions for VPC, ECS, ECR, ALB, IAM, CloudWatch
- Or use the `PowerUserAccess` managed policy for non-production environments

## Environment Configuration

### Environment Variables in Workflows

The following environment variables are configured in the workflows:

| Variable | Default Value | Description |
|----------|---------------|-------------|
| `DOTNET_VERSION` | `9.0.x` | .NET SDK version |
| `AWS_REGION` | `us-east-1` | AWS region for deployment |
| `ECR_REPOSITORY` | `ielts-platform-dev-api` | ECR repository name |
| `PROJECT_NAME` | `IeltsPlatform.ApiService` | Project name for Docker build |

To change these values, edit the `env` section in `.github/workflows/ci-cd.yml`.

## GitHub Environments

The workflows use a `production` environment for deployment jobs. This allows you to:
- Add approval requirements before deployment
- Add environment-specific secrets
- Track deployment history

To configure the production environment:
1. Go to repository Settings → Environments
2. Create a new environment named `production`
3. (Optional) Add required reviewers
4. (Optional) Add deployment protection rules

## Workflow Outputs

### Artifacts

The Terraform plan is uploaded as an artifact and retained for 5 days. You can download it from the workflow run page.

### Build Numbers

Each workflow run is assigned a unique build number (`github.run_number`) which is used as a Docker image tag.

### Git SHA

The short Git SHA is also used as a Docker image tag for easier tracking.

## Troubleshooting

### ECR Login Fails

**Error:** `Error: Cannot perform an interactive login from a non TTY device`

**Solution:** Ensure `AWS_ACCESS_KEY_ID` and `AWS_SECRET_ACCESS_KEY` secrets are correctly configured.

### Terraform Plan Fails

**Error:** `Error acquiring the state lock`

**Solution:** Ensure no other Terraform process is running. If necessary, manually release the state lock.

### ECS Deployment Timeout

**Error:** `Error: waiter state transitioned to Failure`

**Solution:** Check CloudWatch logs for the ECS task. Common issues:
- Insufficient CPU/memory
- Application startup errors
- Health check failures

## Migrating from Jenkins

### Key Differences

| Jenkins | GitHub Actions |
|---------|----------------|
| Jenkinsfile | `.github/workflows/*.yml` |
| Jenkins credentials | GitHub Secrets |
| Manual approval with parameter | Workflow dispatch with input |
| Build number | `github.run_number` |
| Git commit | `github.sha` |

### Credentials Migration

1. Export Jenkins credentials:
   - AWS credentials
   - AWS account ID

2. Add them as GitHub Secrets:
   ```bash
   # Via GitHub CLI
   gh secret set AWS_ACCESS_KEY_ID
   gh secret set AWS_SECRET_ACCESS_KEY
   ```

3. Update ECR repository URL in workflows if using account ID in the name

## Best Practices

1. **Branch Protection**: Enable branch protection rules for `main` branch
2. **Required Status Checks**: Make PR check workflow required before merging
3. **Environment Protection**: Add required reviewers for production environment
4. **Secrets Rotation**: Rotate AWS credentials regularly
5. **Least Privilege**: Use IAM roles with minimal required permissions

## Additional Resources

- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [AWS Actions on GitHub Marketplace](https://github.com/marketplace?type=actions&query=aws)
- [Terraform GitHub Actions](https://github.com/hashicorp/setup-terraform)
