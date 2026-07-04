# ============================================================
# Elite Event Management System - Backend Dockerfile
# Multi-stage build for ASP.NET Core 9 Web API
# ============================================================

# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files and restore dependencies
COPY EliteEvents.Domain/EliteEvents.Domain.csproj EliteEvents.Domain/
COPY EliteEvents.Application/EliteEvents.Application.csproj EliteEvents.Application/
COPY EliteEvents.Infrastructure/EliteEvents.Infrastructure.csproj EliteEvents.Infrastructure/
COPY EliteEvents.API/EliteEvents.API.csproj EliteEvents.API/

RUN dotnet restore EliteEvents.API/EliteEvents.API.csproj

# Copy all source code and build
COPY . .
RUN dotnet publish EliteEvents.API/EliteEvents.API.csproj -c Release -o /app/publish --no-restore

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Create non-root user for security
RUN addgroup --system appgroup && adduser --system --ingroup appgroup appuser

# Copy published output
COPY --from=build /app/publish .

# Create directories for uploads and logs
RUN mkdir -p /app/wwwroot/uploads /app/logs && \
    chown -R appuser:appgroup /app

# Switch to non-root user
USER appuser

# Expose ports
EXPOSE 8080

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:8080/swagger/index.html || exit 1

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_RUNNING_IN_CONTAINER=true

ENTRYPOINT ["dotnet", "EliteEvents.API.dll"]
