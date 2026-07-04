# Elite Event Management System - Deployment Guide

## Prerequisites

- Docker & Docker Compose (v2.0+)
- .NET 9 SDK (for local development)
- Node.js 22+ & npm (for local development)
- SQL Server 2022 (or Docker container)

---

## Quick Start with Docker

### 1. Clone and Navigate
```bash
cd EliteEventManager
```

### 2. Start All Services
```bash
docker-compose up -d --build
```

This starts:
- **SQL Server** on port `1433`
- **API** on port `8080`
- **Frontend** on port `80`

### 3. Initialize Database
After SQL Server is healthy, run the database scripts:
```bash
docker exec -it elite-events-db /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P "EliteEvents@2024!" -C \
  -i /docker-entrypoint-initdb.d/01_CreateDatabase.sql

docker exec -it elite-events-db /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P "EliteEvents@2024!" -C -d EliteEventDB \
  -i /docker-entrypoint-initdb.d/02_CreateTables.sql

docker exec -it elite-events-db /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P "EliteEvents@2024!" -C -d EliteEventDB \
  -i /docker-entrypoint-initdb.d/03_CreateIndexes.sql

docker exec -it elite-events-db /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P "EliteEvents@2024!" -C -d EliteEventDB \
  -i /docker-entrypoint-initdb.d/04_CreateViews.sql

docker exec -it elite-events-db /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P "EliteEvents@2024!" -C -d EliteEventDB \
  -i /docker-entrypoint-initdb.d/05_StoredProcedures.sql

docker exec -it elite-events-db /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P "EliteEvents@2024!" -C -d EliteEventDB \
  -i /docker-entrypoint-initdb.d/06_SeedData.sql
```

### 4. Access the Application
- **Frontend**: http://localhost
- **API / Swagger**: http://localhost:8080/swagger
- **Admin Login**: admin@eliteevents.com / Admin@123

---

## Local Development Setup

### Backend (.NET)
```bash
cd src
dotnet restore
dotnet build
dotnet run --project EliteEvents.API
```
API runs at: https://localhost:7001

### Frontend (Angular)
```bash
cd elite-events-frontend
npm install
npm start
```
Frontend runs at: http://localhost:4200

### Database
- Install SQL Server locally or use Docker:
```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=EliteEvents@2024!" \
  -p 1433:1433 --name elite-sqlserver \
  -d mcr.microsoft.com/mssql/server:2022-latest
```
- Run scripts in `database/` folder in order (01 through 06)

---

## Production Deployment

### Azure Deployment

1. **Database**: Azure SQL Database (Standard S2+)
2. **Backend**: Azure App Service (B2+ plan) or Azure Container Apps
3. **Frontend**: Azure Static Web Apps or Azure CDN + Storage
4. **Storage**: Azure Blob Storage for file uploads

### Environment Variables (Production)

| Variable | Description |
|----------|-------------|
| `ConnectionStrings__DefaultConnection` | SQL Server connection string |
| `Jwt__Key` | 32+ character secret key |
| `Jwt__Issuer` | Token issuer (e.g., EliteEventsAPI) |
| `Jwt__Audience` | Token audience (e.g., EliteEventsClient) |
| `App__BaseUrl` | Backend URL |
| `App__FrontendUrl` | Frontend URL |
| `Email__SmtpHost` | SMTP server host |
| `Email__SmtpPort` | SMTP port |
| `Email__Username` | SMTP username |
| `Email__Password` | SMTP password |

---

## CI/CD Pipeline (GitHub Actions)

Create `.github/workflows/deploy.yml`:

```yaml
name: Build and Deploy

on:
  push:
    branches: [main]
  pull_request:
    branches: [main]

jobs:
  build-backend:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '9.0.x'
      - run: dotnet restore src/EliteEvents.sln
      - run: dotnet build src/EliteEvents.sln --no-restore
      - run: dotnet publish src/EliteEvents.API/EliteEvents.API.csproj -c Release -o ./publish

  build-frontend:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with:
          node-version: '22'
      - run: cd elite-events-frontend && npm ci --legacy-peer-deps
      - run: cd elite-events-frontend && npm run build -- --configuration production

  docker:
    needs: [build-backend, build-frontend]
    runs-on: ubuntu-latest
    if: github.ref == 'refs/heads/main'
    steps:
      - uses: actions/checkout@v4
      - run: docker-compose build
      # Add push to registry steps here
```

---

## Health Checks

- API: `GET /swagger/index.html` (200 OK)
- Frontend: `GET /` (200 OK)
- Database: SQL connection test

---

## Monitoring & Logging

- Backend logs: Serilog to console + rolling file at `/app/logs/`
- Docker logs: `docker-compose logs -f api`
- Production: Integrate with Application Insights, ELK Stack, or Datadog

---

## Security Checklist

- [ ] Change default JWT key in production
- [ ] Change default SQL Server password
- [ ] Enable HTTPS with valid SSL certificate
- [ ] Configure CORS to allow only production domains
- [ ] Enable rate limiting for API endpoints
- [ ] Set up database backups
- [ ] Enable audit logging
- [ ] Review and restrict file upload types/sizes
- [ ] Configure Content Security Policy headers
- [ ] Enable SQL Server encryption at rest

---

## Scaling Considerations

- Use Azure SQL Elastic Pool for database scaling
- Deploy API behind a load balancer with multiple instances
- Use Redis for session/cache management
- Implement CDN for static assets
- Consider message queues (Azure Service Bus) for email/SMS notifications
- Implement database read replicas for reporting queries
