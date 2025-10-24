# Docker Deployment Guide

## Prerequisites

- Docker Desktop installed and running
- Docker Compose v2 or higher
- Configured environment variables

## Setup Instructions

### 1. Configure Environment Variables

Copy `.env.example` to `.env` and fill in your credentials:

```bash
cp .env.example .env
```

Required variables:
- `AWS_BUCKET_NAME` - Your S3 bucket name
- `AWS_ACCESS_KEY_ID` - AWS access key
- `AWS_SECRET_ACCESS_KEY` - AWS secret key
- `EMAIL_SMTP_USER` - SMTP username for email service
- `EMAIL_SMTP_PASSWORD` - SMTP password
- `GOOGLE_CLIENT_ID` - Google OAuth client ID (optional)

### 2. Start Services

Start all services (PostgreSQL + API):

```bash
docker compose up -d
```

Or start services individually:

```bash
# Start PostgreSQL only
docker compose up -d postgres

# Start API only (requires PostgreSQL running)
docker compose up -d api
```

### 3. Apply Database Migrations

If starting fresh, apply migrations to create the database schema:

```bash
# From the Infrastructure project directory
cd IeltsPlatform.Infrastructure
dotnet ef database update --startup-project ../IeltsPlatform.ApiService
```

Or execute migrations inside the API container:

```bash
docker compose exec api dotnet ef database update --project /src/IeltsPlatform.Infrastructure
```

### 4. Verify Services

- API: http://localhost:8080
- Health Check: http://localhost:8080/health
- Swagger: http://localhost:8080/swagger

Check logs:

```bash
# All services
docker compose logs -f

# Specific service
docker compose logs -f api
docker compose logs -f postgres
```

## Service Architecture

### PostgreSQL (`postgres`)
- **Image**: `postgres:16-alpine`
- **Port**: 5432
- **Database**: `ielts_platform`
- **Credentials**: postgres/postgres (change for production)
- **Volume**: `postgres-data` for data persistence

### API Service (`api`)
- **Port**: 8080
- **Dependencies**: PostgreSQL (waits for health check)
- **Health Check**: `curl http://localhost:8080/health`

## Production Deployment

### Security Considerations

1. **Change default credentials**:
   - Update PostgreSQL password in docker-compose.yml
   - Generate a strong JWT secret (min 32 characters)

2. **Use secrets management**:
   - Use Docker secrets or environment-specific configurations
   - Never commit `.env` file to version control

3. **Enable HTTPS**:
   - Configure reverse proxy (nginx/traefik)
   - Use Let's Encrypt for SSL certificates

4. **Database backups**:
   ```bash
   docker compose exec postgres pg_dump -U postgres ielts_platform > backup.sql
   ```

### Environment-Specific Configurations

Update `ASPNETCORE_ENVIRONMENT` in docker-compose.yml:
- Development: `Development`
- Staging: `Staging`
- Production: `Production`

### Scaling

Scale the API service:

```bash
docker compose up -d --scale api=3
```

Add a load balancer (nginx/traefik) in front of multiple API instances.

## Troubleshooting

### Container won't start

Check logs:
```bash
docker compose logs api
```

### Database connection failed

Verify PostgreSQL is running:
```bash
docker compose ps postgres
```

Check connection string in environment variables.

### Migration errors

Ensure PostgreSQL is healthy before running migrations:
```bash
docker compose exec postgres pg_isready -U postgres
```

## Local Development Without Docker

If you prefer running PostgreSQL locally:

1. Install PostgreSQL 16
2. Create database: `CREATE DATABASE ielts_platform;`
3. Update connection string in `appsettings.Development.json`
4. Run migrations:
   ```bash
   cd IeltsPlatform.Infrastructure
   dotnet ef database update --startup-project ../IeltsPlatform.ApiService
   ```
5. Run API:
   ```bash
   cd IeltsPlatform.ApiService
   dotnet run
   ```

## Optional: LocalStack for Local AWS

Uncomment the `localstack` service in docker-compose.yml for local AWS emulation:

```yaml
localstack:
  image: localstack/localstack:latest
  ports:
    - "4566:4566"
  environment:
    - SERVICES=s3,ses
```

Update AWS configuration to point to LocalStack:
```
AWS__ServiceURL=http://localhost:4566
```

## Commands Reference

```bash
# Start all services
docker compose up -d

# Stop all services
docker compose down

# Stop and remove volumes (deletes database data)
docker compose down -v

# Rebuild API image
docker compose build api

# View logs
docker compose logs -f

# Execute command in container
docker compose exec api bash
docker compose exec postgres psql -U postgres -d ielts_platform

# Check service health
docker compose ps

# Restart a service
docker compose restart api
```
