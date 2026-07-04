# ============================================================
# Elite Event Management System - Frontend Dockerfile
# Multi-stage build for Angular 20+ application
# ============================================================

# Stage 1: Build
FROM node:22-alpine AS build
WORKDIR /app

# Copy package files and install dependencies
COPY package.json package-lock.json* ./
RUN npm ci --legacy-peer-deps

# Copy source and build for production
COPY . .
RUN npm run build -- --configuration production

# Stage 2: Serve with Nginx
FROM nginx:alpine AS runtime

# Copy custom nginx config
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Copy built Angular app
COPY --from=build /app/dist/elite-events-frontend/browser /usr/share/nginx/html

# Expose port
EXPOSE 80

# Health check
HEALTHCHECK --interval=30s --timeout=5s --start-period=10s --retries=3 \
  CMD wget --no-verbose --tries=1 --spider http://localhost/ || exit 1

CMD ["nginx", "-g", "daemon off;"]
