# Quick Start Guide: GitHub Actions CI/CD

This guide will help you get started with the new GitHub Actions CI/CD pipeline.

## For Repository Administrators

### Initial Setup (One-time)

1. **Configure GitHub Secrets**

   Go to repository **Settings** → **Secrets and variables** → **Actions**:

   ```
   AWS_ACCESS_KEY_ID: <your-aws-access-key>
   AWS_SECRET_ACCESS_KEY: <your-aws-secret-key>
   ```

2. **Set Up Production Environment (Optional but Recommended)**

   Go to repository **Settings** → **Environments** → **New environment**:
   - Name: `production`
   - Add required reviewers for approval
   - Save protection rules

3. **Enable Branch Protection (Recommended)**

   Go to repository **Settings** → **Branches** → **Add rule**:
   - Branch name pattern: `main`
   - ✅ Require status checks before merging
   - ✅ Select "Build and Test" as required check
   - ✅ Require branches to be up to date before merging

### Verification

Push a test commit to verify workflows are working:

```bash
git checkout -b test-workflows
echo "# Test" >> test.txt
git add test.txt
git commit -m "Test GitHub Actions workflows"
git push -u origin test-workflows
```

Create a PR and verify the **Pull Request Check** workflow runs successfully.

---

## For Developers

### Daily Workflow

#### 1. Pull Requests (Automatic)

When you create a PR, GitHub Actions automatically:
- ✅ Restores dependencies
- ✅ Builds the solution
- ✅ Runs tests (if available)
- ✅ Verifies Docker image can be built

**No action needed** - just wait for checks to pass!

#### 2. Merging to Main (Automatic Build)

When you merge to `main` or `develop`:
- ✅ All PR checks run again
- ✅ Docker image is built
- ✅ Image is pushed to ECR with tags:
  - `build-<number>` (e.g., `build-42`)
  - `<git-sha>` (e.g., `abc1234`)
  - `latest` (only on main branch)

**No action needed** - builds happen automatically!

#### 3. Manual Deployment (On-demand)

To deploy to AWS ECS:

1. Go to **Actions** tab
2. Click **CI/CD Pipeline** on the left
3. Click **Run workflow** button
4. Select branch (usually `main`)
5. ✅ Check **Deploy to AWS ECS**
6. Click **Run workflow**
7. If you have environment protection, approve when prompted
8. Wait for deployment to complete (~5-10 minutes)

---

## Understanding Workflow Status

### Status Badges

In the PR, you'll see workflow status:
- 🟢 Green checkmark = Success
- 🔴 Red X = Failed
- 🟡 Yellow dot = In progress

### Viewing Logs

1. Click **Details** next to the workflow status
2. Click on a job name to see detailed logs
3. Expand steps to see individual command output

### Common Statuses

| Status | What It Means | What To Do |
|--------|---------------|------------|
| ✅ **Build and Test** passed | Code compiled and tests passed | Continue with review |
| ❌ **Build and Test** failed | Build or test error | Check logs, fix code, push again |
| ✅ **Build Docker** passed | Docker image created | Good to merge |
| ⏭️ **Terraform Plan** skipped | Not on main branch | Normal for feature branches |
| ⏸️ **Deploy** waiting | Waiting for approval | Approve in Environments tab |

---

## Quick Reference

### Workflow Files

```
.github/workflows/
├── ci-cd.yml          # Main pipeline (build, Docker, deploy)
├── pr-check.yml       # PR validation
└── README.md          # Detailed documentation
```

### When Workflows Run

| Workflow | Trigger | Branches |
|----------|---------|----------|
| CI/CD Pipeline | Push | main, develop |
| Pull Request Check | PR opened/updated | all PRs |
| Manual Deploy | Workflow dispatch | main (recommended) |

### Build Artifacts

After a successful build on `main`:
- Docker image in ECR: `<account>.dkr.ecr.us-east-1.amazonaws.com/ielts-platform-dev-api:latest`
- Tagged with build number and git SHA
- Terraform plan available in workflow artifacts (5 day retention)

---

## Troubleshooting

### My PR check is failing

1. **Click "Details"** to see the error
2. Common issues:
   - **Build error**: Fix compilation errors in your code
   - **Test failure**: Fix failing tests or update tests
   - **Docker build error**: Check Dockerfile syntax

3. **Fix and push**: Push a new commit, workflow re-runs automatically

### I can't trigger a deployment

Check:
- ✅ You're on the `main` branch
- ✅ You have write access to the repository
- ✅ GitHub Secrets are configured correctly

### Deployment is waiting for approval

If you configured environment protection:
1. Go to **Settings** → **Environments** → **production**
2. Click **Review deployments**
3. Check the deployment
4. Click **Approve and deploy**

### Deployment failed

1. Check workflow logs in the Actions tab
2. Check CloudWatch logs for the ECS task
3. Common issues:
   - AWS credentials expired or invalid
   - ECS service not healthy
   - Application startup error

---

## Best Practices

### ✅ Do's

- **Create feature branches** for new work
- **Open PRs** for all changes to main
- **Wait for checks** to pass before merging
- **Test deployments** in a dev environment first
- **Monitor deployments** in AWS CloudWatch after deploying

### ❌ Don'ts

- **Don't push directly** to main (use PRs)
- **Don't merge** if checks are failing
- **Don't deploy** without reviewing changes
- **Don't ignore** workflow failures
- **Don't deploy** during high-traffic periods

---

## Getting Help

### Resources

1. **Workflow Documentation**: [.github/workflows/README.md](.github/workflows/README.md)
2. **Migration Guide**: [MIGRATION_GUIDE.md](MIGRATION_GUIDE.md)
3. **GitHub Actions Docs**: https://docs.github.com/en/actions

### Support Channels

1. **Create an issue** in this repository
2. **Check workflow logs** for error messages
3. **Review recent changes** that might have broken the build

---

## What's Different from Jenkins?

| Jenkins | GitHub Actions | Benefit |
|---------|----------------|---------|
| Manual trigger with parameter | Workflow dispatch | More explicit, safer |
| Always builds on commit | PR checks + main builds | Better visibility |
| Single pipeline | Separate PR and deploy | Clearer separation |
| Credentials in Jenkins | GitHub Secrets | More secure |
| External server | GitHub-hosted | No maintenance |

---

## Next Steps

Once comfortable with the basics:

1. ✅ Set up branch protection rules
2. ✅ Configure environment protection with reviewers
3. ✅ Set up notifications (Slack, email)
4. ✅ Review CloudWatch logs regularly
5. ✅ Consider adding more workflows (security scanning, etc.)

Happy deploying! 🚀
