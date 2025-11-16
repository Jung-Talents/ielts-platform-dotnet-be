# Migration Guide: Jenkins to GitHub Actions

This guide will help you migrate from Jenkins to GitHub Actions for the IELTS Platform CI/CD pipeline.

## Overview

The new GitHub Actions workflows provide the same functionality as the Jenkins pipeline, with some improvements:

- ✅ Better integration with GitHub (no external server needed)
- ✅ Built-in secrets management
- ✅ Workflow visualization in GitHub UI
- ✅ Environment protection rules
- ✅ Artifact management
- ✅ Docker layer caching with GitHub Actions cache

## Prerequisites

Before migrating, ensure you have:

1. AWS credentials with necessary permissions (see [.github/workflows/README.md](.github/workflows/README.md))
2. Access to GitHub repository settings (admin role)
3. Understanding of your current Jenkins setup

## Migration Steps

### Step 1: Configure GitHub Secrets

1. Navigate to your repository on GitHub
2. Go to **Settings** → **Secrets and variables** → **Actions**
3. Click **New repository secret**
4. Add the following secrets:

   | Secret Name | Value | Source |
   |------------|-------|--------|
   | `AWS_ACCESS_KEY_ID` | Your AWS Access Key ID | From Jenkins credentials `aws-credentials` |
   | `AWS_SECRET_ACCESS_KEY` | Your AWS Secret Access Key | From Jenkins credentials `aws-credentials` |

### Step 2: Configure Production Environment (Optional but Recommended)

1. Go to **Settings** → **Environments**
2. Click **New environment**
3. Name it `production`
4. Configure protection rules:
   - Add required reviewers (recommended for production deployments)
   - Set deployment delay if needed
5. Click **Save protection rules**

### Step 3: Update ECR Repository Name (If Needed)

The Jenkins pipeline uses an environment variable for the ECR repository name. Check if your ECR repository name matches the one in the GitHub Actions workflow:

**Jenkins**:
```groovy
ECR_REPOSITORY = "${AWS_ACCOUNT_ID}.dkr.ecr.${AWS_REGION}.amazonaws.com/ielts-platform-dev-api"
```

**GitHub Actions**:
```yaml
ECR_REPOSITORY: 'ielts-platform-dev-api'
```

If your repository name is different, update it in `.github/workflows/ci-cd.yml`:

```yaml
env:
  ECR_REPOSITORY: 'your-repository-name'  # Update this
```

### Step 4: Test the Workflow

1. Push a commit to a feature branch
2. Create a pull request to `main` or `develop`
3. Verify that the **Pull Request Check** workflow runs successfully
4. Merge the PR
5. Verify that the **CI/CD Pipeline** workflow runs and builds the Docker image

### Step 5: Test Manual Deployment

1. Go to **Actions** tab
2. Select **CI/CD Pipeline** workflow
3. Click **Run workflow**
4. Select `main` branch
5. Check **Deploy to AWS ECS**
6. Click **Run workflow**
7. If you configured environment protection, approve the deployment when prompted
8. Wait for the workflow to complete
9. Verify your application is running on ECS

### Step 6: Disable or Remove Jenkins Pipeline

Once you've verified that GitHub Actions works correctly:

1. **Option A: Keep Jenkins as backup** (Recommended initially)
   - Disable the Jenkins job without deleting it
   - Keep the `Jenkinsfile` in the repository
   - Remove it after a few successful GitHub Actions deployments

2. **Option B: Full migration**
   - Delete the Jenkins job
   - Remove the `Jenkinsfile` from the repository:
     ```bash
     git rm Jenkinsfile
     git commit -m "Remove Jenkins pipeline after migration to GitHub Actions"
     git push
     ```

## Workflow Comparison

### Trigger Differences

| Trigger | Jenkins | GitHub Actions |
|---------|---------|----------------|
| Code push | Automatic | Automatic |
| Pull request | Manual or webhook | Automatic |
| Manual deployment | Parameter `DEPLOY_TO_AWS` | Workflow dispatch input |
| Branch filtering | Pipeline `when` blocks | Workflow `if` conditions |

### Stage Mapping

| Jenkins Stage | GitHub Actions Job | Notes |
|--------------|-------------------|-------|
| Checkout | Automatic | GitHub Actions checks out code automatically |
| Install Dependencies | `build-and-test` job | `dotnet restore` step |
| Build | `build-and-test` job | `dotnet build` step |
| Test | `build-and-test` job | `dotnet test` step |
| Build Docker Image | `build-docker` job | Uses `docker/build-push-action` |
| Push to ECR | `build-docker` job | Integrated with build step |
| Terraform Plan | `terraform-plan` job | Runs on `main` branch only |
| Terraform Apply | `deploy-infrastructure` job | Manual trigger only |
| Deploy to ECS | `deploy-ecs` job | Manual trigger only |

### Credential Management

| Jenkins | GitHub Actions |
|---------|----------------|
| Jenkins credentials store | GitHub Secrets |
| `credentials()` function | `secrets.SECRET_NAME` |
| Credential ID | Secret name in uppercase with underscores |

## Key Differences and Improvements

### 1. **No External Server**
- Jenkins requires a dedicated server
- GitHub Actions runs on GitHub-hosted runners (no maintenance needed)

### 2. **Better Secrets Management**
- GitHub Secrets are encrypted and never exposed in logs
- Can be scoped to specific environments

### 3. **Workflow Visualization**
- GitHub Actions provides a visual representation of workflow runs
- Easy to see which jobs passed or failed

### 4. **Environment Protection**
- Built-in support for deployment approvals
- Can configure required reviewers per environment

### 5. **Artifact Management**
- Terraform plans are automatically uploaded as artifacts
- Can be downloaded for review before deployment

### 6. **Docker Caching**
- GitHub Actions cache significantly speeds up Docker builds
- No need to pull layers from ECR for every build

## Troubleshooting

### Workflow Not Triggering

**Issue**: Workflow doesn't run on push or pull request

**Solution**:
- Check that workflow files are in `.github/workflows/` directory
- Verify YAML syntax is correct
- Check branch name matches the `on.push.branches` filter

### ECR Login Fails

**Issue**: `Error: Cannot perform an interactive login from a non TTY device`

**Solution**:
- Verify `AWS_ACCESS_KEY_ID` and `AWS_SECRET_ACCESS_KEY` secrets are set correctly
- Check that AWS credentials have ECR permissions
- Ensure the secrets don't have extra whitespace or newlines

### Docker Build Fails

**Issue**: Docker build fails with "context" or "Dockerfile" not found

**Solution**:
- Verify the `PROJECT_NAME` environment variable is correct
- Check that the Dockerfile path in the workflow matches the actual location
- Ensure the `context` is set to `.` (repository root)

### Terraform State Lock

**Issue**: `Error acquiring the state lock`

**Solution**:
- Ensure no other process is running Terraform
- If necessary, manually release the state lock in AWS DynamoDB
- Consider using Terraform Cloud or Terraform Enterprise for better state management

### Deployment Timeout

**Issue**: Deployment times out or fails

**Solution**:
- Check CloudWatch logs for the ECS task
- Verify health check configuration in the task definition
- Ensure the container has sufficient CPU and memory
- Check that the application starts up correctly

## Rollback Plan

If you need to rollback to Jenkins:

1. Re-enable the Jenkins job
2. Trigger a build manually
3. The Jenkins pipeline will override the GitHub Actions deployment
4. Investigate the issues with GitHub Actions
5. Fix the problems before attempting migration again

## Best Practices

1. **Start with a test environment**: Test the migration on a development or staging environment first
2. **Keep Jenkins running**: Don't delete Jenkins immediately; keep it as a backup for at least a week
3. **Monitor closely**: Watch the first few GitHub Actions deployments carefully
4. **Use branch protection**: Require status checks from GitHub Actions before merging to `main`
5. **Document changes**: Update team documentation and runbooks
6. **Train team members**: Ensure everyone knows how to trigger deployments with GitHub Actions

## Support

If you encounter issues during migration:

1. Check the [GitHub Actions documentation](.github/workflows/README.md)
2. Review the [GitHub Actions documentation](https://docs.github.com/en/actions)
3. Check workflow run logs in the Actions tab
4. Create an issue in the repository with details about the problem

## Next Steps

After successful migration:

1. ✅ Remove Jenkins credentials (keep them backed up)
2. ✅ Update team documentation
3. ✅ Configure branch protection rules
4. ✅ Set up Slack or email notifications for workflow failures (optional)
5. ✅ Consider implementing additional workflows:
   - Dependency updates (Dependabot)
   - Security scanning (CodeQL)
   - Performance testing
   - Automated releases

## Additional Resources

- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [AWS Actions on GitHub Marketplace](https://github.com/marketplace?type=actions&query=aws)
- [Docker Build Push Action](https://github.com/marketplace/actions/build-and-push-docker-images)
- [Terraform GitHub Actions](https://github.com/hashicorp/setup-terraform)
