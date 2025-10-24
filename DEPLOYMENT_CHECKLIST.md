# Deployment Checklist

## Pre-Deployment

### Development Environment

- [x] All 16 migration tasks completed
- [x] Code builds successfully without errors
- [x] Database migration files created
- [ ] Unit tests written and passing
- [ ] Integration tests written and passing
- [x] API documentation (Swagger) accessible

### Code Quality

- [x] No critical warnings in build output
- [x] FluentValidation rules for all DTOs
- [x] Error handling implemented
- [x] Rate limiting configured
- [x] CORS configured
- [ ] Code review completed
- [ ] Security audit performed

## Configuration

### Application Settings

- [ ] Update JWT secret (min 32 characters, production-grade)
- [ ] Configure access token expiration (recommended: 15 min)
- [ ] Configure refresh token expiration (recommended: 30 days)
- [ ] Set ASPNETCORE_ENVIRONMENT to "Production"
- [ ] Configure allowed CORS origins (remove AllowAnyOrigin)

### Database

- [ ] PostgreSQL 16 installed/provisioned
- [ ] Database created: `ielts_platform`
- [ ] Connection string configured
- [ ] Database user created with appropriate permissions
- [ ] Migrations applied successfully
- [ ] Database backups configured
- [ ] Connection pooling configured

### AWS Services

#### S3
- [ ] S3 bucket created
- [ ] Bucket name configured in appsettings.json
- [ ] IAM user created with S3 permissions
- [ ] Access key and secret key configured
- [ ] Bucket CORS policy configured
- [ ] Bucket lifecycle policies set

#### SES (Email)
- [ ] SES verified sender email
- [ ] SMTP credentials created
- [ ] SMTP host configured
- [ ] SMTP port configured (587 recommended)
- [ ] Test email sent successfully

### Google OAuth (Optional)

- [ ] Google Cloud project created
- [ ] OAuth 2.0 client ID created
- [ ] Authorized redirect URIs configured
- [ ] Client ID added to appsettings.json

## Security

### Authentication

- [x] Argon2 password hashing implemented
- [x] JWT token generation working
- [x] Token refresh mechanism working
- [x] Email verification required
- [ ] Password complexity requirements defined
- [ ] Account lockout policy configured
- [ ] Session timeout configured

### Authorization

- [x] Role-based access control implemented
- [x] Admin endpoints protected
- [x] Moderator endpoints protected
- [ ] Resource-level authorization verified
- [ ] Cross-user access prevention verified

### Network Security

- [ ] HTTPS/TLS configured
- [ ] SSL certificate installed
- [ ] HTTP to HTTPS redirect enabled
- [x] CORS restricted to specific origins (update for production)
- [ ] Security headers configured (HSTS, CSP, X-Frame-Options)
- [x] Rate limiting enabled (100 req/60s)

### Data Protection

- [ ] Database encryption at rest
- [ ] Backup encryption enabled
- [ ] Sensitive data in environment variables/secrets manager
- [ ] Connection strings secured
- [ ] AWS credentials in secrets manager
- [ ] No secrets in source control

## Infrastructure

### Docker Setup

- [x] Dockerfile created and tested
- [x] docker-compose.yml configured
- [x] .env.example provided
- [ ] .env file created with production values
- [ ] Docker images built successfully
- [ ] Containers start without errors
- [ ] Health checks passing

### Container Registry

- [ ] Container registry chosen (ECR, Docker Hub, etc.)
- [ ] Registry authentication configured
- [ ] Image tagged and pushed
- [ ] Image scan completed (security vulnerabilities)

### Orchestration (Choose One)

#### Docker Compose (Simple)
- [ ] docker-compose.yml configured for production
- [ ] Volumes configured for data persistence
- [ ] Networks configured
- [ ] Resource limits set (CPU, memory)

#### AWS ECS (Recommended)
- [ ] ECS cluster created
- [ ] Task definition created
- [ ] Service configured
- [ ] Application Load Balancer configured
- [ ] Target groups configured
- [ ] Auto-scaling policies set
- [ ] CloudWatch logging enabled

#### Kubernetes (Advanced)
- [ ] Kubernetes manifests created
- [ ] Deployments configured
- [ ] Services configured
- [ ] Ingress configured
- [ ] Persistent volumes configured
- [ ] Secrets configured
- [ ] Resource limits set

## Database Migration

- [x] Initial migration created
- [ ] Migration tested in staging environment
- [ ] Migration applied to production database
- [ ] Database indexes verified
- [ ] Query performance tested
- [ ] Rollback plan prepared

## Monitoring & Logging

### Application Monitoring

- [ ] Health check endpoint responding: `/health`
- [ ] Application Insights configured (or alternative)
- [ ] Custom metrics configured
- [ ] Performance counters enabled
- [ ] Distributed tracing enabled

### Logging

- [ ] Structured logging configured
- [ ] Log levels configured appropriately
- [ ] Log aggregation set up (CloudWatch, ELK, etc.)
- [ ] Error alerts configured
- [ ] Log retention policy set

### Alerting

- [ ] Error rate alerts
- [ ] Response time alerts
- [ ] CPU/Memory alerts
- [ ] Database connection alerts
- [ ] Disk space alerts
- [ ] Alert notification channels configured (email, Slack, etc.)

## Testing

### Unit Tests

- [ ] Service layer tests
- [ ] Repository layer tests
- [ ] Validation tests
- [ ] Test coverage > 70%

### Integration Tests

- [ ] API endpoint tests
- [ ] Database integration tests
- [ ] Authentication flow tests
- [ ] Authorization tests

### End-to-End Tests

- [ ] User registration flow
- [ ] Login flow
- [ ] Test creation flow
- [ ] Test submission flow
- [ ] File upload flow

### Load Testing

- [ ] Load testing tool chosen (k6, JMeter, etc.)
- [ ] Load test scenarios created
- [ ] Baseline performance measured
- [ ] Peak load tested
- [ ] Sustained load tested
- [ ] Performance bottlenecks identified and resolved

## Performance

### Database

- [ ] Indexes on frequently queried columns verified
- [ ] Query execution plans reviewed
- [ ] Connection pooling configured
- [ ] Database statistics updated
- [ ] Slow query logging enabled

### Application

- [ ] Response caching implemented where appropriate
- [ ] AsNoTracking() used for read-only queries
- [ ] Lazy loading vs eager loading optimized
- [ ] API response times < 200ms for simple queries
- [ ] API response times < 1s for complex queries

### Infrastructure

- [ ] CDN configured for static assets
- [ ] Compression enabled (gzip/brotli)
- [ ] HTTP/2 enabled
- [ ] Database read replicas configured (if needed)

## Backup & Recovery

### Database Backups

- [ ] Automated daily backups configured
- [ ] Backup retention policy defined (7 days, 30 days, etc.)
- [ ] Backup verification scheduled
- [ ] Point-in-time recovery tested
- [ ] Backup restoration tested
- [ ] Backup storage location defined
- [ ] Off-site backup configured

### Application Backups

- [ ] Configuration backed up
- [ ] Uploaded files backed up (S3 versioning)
- [ ] Disaster recovery plan documented
- [ ] RTO (Recovery Time Objective) defined
- [ ] RPO (Recovery Point Objective) defined

## Documentation

- [x] README.md updated
- [x] ARCHITECTURE.md provided
- [x] QUICK_START.md provided
- [x] DOCKER_SETUP.md provided
- [x] MIGRATION_SUMMARY.md provided
- [ ] API documentation published
- [ ] Deployment runbook created
- [ ] Troubleshooting guide created
- [ ] Change log maintained

## CI/CD Pipeline

### Source Control

- [ ] Git repository created
- [ ] .gitignore configured (exclude .env, bin/, obj/, etc.)
- [ ] Branch protection rules set
- [ ] Code review process defined

### Continuous Integration

- [ ] CI pipeline created (GitHub Actions, GitLab CI, etc.)
- [ ] Build on every commit
- [ ] Run tests on every commit
- [ ] Code quality checks (linting, formatting)
- [ ] Security scanning (SAST)
- [ ] Dependency vulnerability scanning

### Continuous Deployment

- [ ] Deployment pipeline created
- [ ] Staging environment configured
- [ ] Production environment configured
- [ ] Blue-green or rolling deployment configured
- [ ] Automated rollback on failure
- [ ] Deployment approval process defined

## Post-Deployment

### Verification

- [ ] Health check endpoint responding
- [ ] Swagger documentation accessible
- [ ] User registration working
- [ ] Email verification working
- [ ] Login working
- [ ] JWT token generation working
- [ ] Token refresh working
- [ ] File upload to S3 working
- [ ] Database queries performing well
- [ ] All API endpoints responding

### Monitoring

- [ ] Monitor error rates (first 24 hours)
- [ ] Monitor response times
- [ ] Monitor resource utilization (CPU, memory, disk)
- [ ] Monitor database connections
- [ ] Monitor external service calls (S3, SES)
- [ ] Check logs for errors/warnings

### Performance

- [ ] Measure baseline metrics
- [ ] Compare with pre-deployment metrics
- [ ] Identify any performance regressions
- [ ] Optimize if needed

### User Communication

- [ ] Deployment announcement sent
- [ ] Known issues documented
- [ ] Support channels ready
- [ ] Feedback mechanism in place

## Rollback Plan

### Preparation

- [ ] Previous version tagged in source control
- [ ] Previous container image available
- [ ] Previous database backup available
- [ ] Rollback procedure documented

### Rollback Steps

1. [ ] Stop current deployment
2. [ ] Deploy previous version
3. [ ] Restore database if schema changed
4. [ ] Verify rollback successful
5. [ ] Notify stakeholders

## Compliance & Legal

- [ ] Privacy policy updated
- [ ] Terms of service updated
- [ ] Data retention policy defined
- [ ] GDPR compliance verified (if applicable)
- [ ] Data encryption verified
- [ ] User data deletion procedure implemented
- [ ] Audit logging enabled

## Final Sign-Off

- [ ] Development team approved
- [ ] QA team approved
- [ ] Security team approved
- [ ] Operations team approved
- [ ] Product owner approved
- [ ] Deployment scheduled
- [ ] Stakeholders notified

---

## Deployment Commands

### Local Docker Deployment

```bash
# 1. Configure environment
cp .env.example .env
# Edit .env with production values

# 2. Build and start services
docker compose up -d

# 3. Apply migrations
docker compose exec api dotnet ef database update --project /src/IeltsPlatform.Infrastructure

# 4. Verify
curl http://localhost:8080/health
```

### AWS ECS Deployment

```bash
# 1. Build and tag image
docker build -t ielts-api:latest -f IeltsPlatform.ApiService/Dockerfile .
docker tag ielts-api:latest {account-id}.dkr.ecr.{region}.amazonaws.com/ielts-api:latest

# 2. Push to ECR
aws ecr get-login-password --region {region} | docker login --username AWS --password-stdin {account-id}.dkr.ecr.{region}.amazonaws.com
docker push {account-id}.dkr.ecr.{region}.amazonaws.com/ielts-api:latest

# 3. Update ECS service
aws ecs update-service --cluster ielts-cluster --service ielts-api --force-new-deployment

# 4. Apply migrations (from local or bastion host)
dotnet ef database update --connection "Host=rds-endpoint;Database=ielts_platform;Username=admin;Password=password"
```

---

**Last Updated**: October 24, 2025  
**Next Review**: Before production deployment
