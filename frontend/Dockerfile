FROM node:slim@sha256:bbfe63937f1938e0f61f6e1cbc811d772ba6559babb41820b441bd70e57628b2 AS src
WORKDIR /src

# Install dependencies and build
COPY package*.json ./
RUN npm ci
COPY . .

FROM src AS debug
WORKDIR /src

EXPOSE 5173
ENTRYPOINT [ "npm", "run", "dev", "--", "--host", "--port", "8080" ]

FROM src AS publish
WORKDIR /src

RUN npm run build

# Stage 2: Serve with NGINX
FROM nginx:alpine@sha256:65645c7bb6a0661892a8b03b89d0743208a18dd2f3f17a54ef4b76fb8e2f2a10 AS production
# Remove default NGINX website
RUN rm -rf /usr/share/nginx/html/*

# Copy built assets from builder
COPY --from=publish /src/dist /usr/share/nginx/html

# Copy custom NGINX config
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Expose port
EXPOSE 80

# Start NGINX
CMD ["nginx", "-g", "daemon off;"]