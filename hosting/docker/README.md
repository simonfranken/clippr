# Hosting clippr with Docker Compose

## Docker Compose Files Overview

clippr provides several Docker Compose configurations to cover different deployment and development use cases:

- **docker-compose.yaml:**  
  Standard production or staging deployment. Uses SSL (HTTPS) via Traefik and prebuilt images.  
  **Use this file when you want secure hosting with HTTPS.**

- **docker-compose-insecure.yaml:**  
  Insecure deployment using plain HTTP.  
  **Recommended for local development, testing, or when deploying behind another reverse proxy that handles SSL offloading.**

- **docker-compose-dev.yaml:**  
  Development setup. Builds images from source using the debug stage. Also uses SSL (HTTPS) via Traefik.  
  **Use this file for active development on clippr with live code rebuilding and debugging.**

---

## Deployment Overview

To deploy clippr using Docker Compose, follow these steps:

1. **[Clone the repository](#1-clone-the-repository)**
2. **[Copy and configure environment files](#2-environment-configuration)**
3. **[Optional: Set up SSL certificates](#3-ssl-certificates)**
4. **[Start the services](#4-running-clippr)**

Each step is explained in detail below.

---

## 1. Clone the Repository

Clone the repository to your machine:

```bash
git clone git@github.com:simonfranken/clippr.git
cd clippr
```

---

## 2. Environment Configuration

clippr uses multiple environment files:

- `.env`: Used by Docker Compose for service orchestration (not injected directly into containers).
- `backend.env`: Passed to the backend service container.
- `identityService.env`: Passed to the identity service container.

Copy the template files and adjust as needed:

```bash
cp .env.template .env
cp backend.env.template backend.env
cp identityService.env.template identityService.env
```

### `.env` (for Docker Compose orchestration)

| Variable        | Default Value            | Required | Description                                                                              |
| --------------- | ------------------------ | -------- | ---------------------------------------------------------------------------------------- |
| `CLIPPR_URL`    | `https://localhost:8081` | Yes      | The external base URL for the clippr instance. Used by backend and identity services.    |
| `CLIPPR_DOMAIN` | `localhost`              | Yes      | The domain name Traefik uses for routing. Set this to your actual domain or `localhost`. |

> **Note:**  
> `.env` is processed by Docker Compose for variable substitution.

### `backend.env` (passed to backend container)

| Variable                                             | Default Value                                 | Required | Description                              |
| ---------------------------------------------------- | --------------------------------------------- | -------- | ---------------------------------------- |
| `ConnectionStrings__Default`                         | `Server=database;Database=clipprDb;Uid=root;` | Yes      | Backend database connection string.      |
| `Hosting__Url`                                       | `${CLIPPR_URL}/api`                           | Yes      | URL where the backend listens.           |
| `Authentication__IdentityService__IssuerPublicUrl`   | `${CLIPPR_URL}/identity`                      | Yes      | Public URL to identity service issuer.   |
| `Authentication__IdentityService__IssuerInternalUrl` | `http://identity_service:8080`                | Yes      | Internal URL to identity service issuer. |

### `identityService.env` (passed to identity service container)

| Variable                                            | Default Value                                         | Required | Description                                          |
| --------------------------------------------------- | ----------------------------------------------------- | -------- | ---------------------------------------------------- |
| `ConnectionStrings__Default`                        | `Server=database;Database=clipprIdentityDb;Uid=root;` | Yes      | Connection string for identity database.             |
| `Hosting__Url`                                      | `${CLIPPR_URL}/identity`                              | Yes      | URL where the identity service listens.              |
| `Authentication__ExternalProviders__0__ProviderKey` | _N/A_                                                 | No       | (Optional) Key for external authentication provider. |
| `Authentication__ExternalProviders__0__Issuer`      | _N/A_                                                 | No       | (Optional) Issuer URL for external provider.         |
| `Authentication__ExternalProviders__0__Audience`    | _N/A_                                                 | No       | (Optional) Audience/client ID for JWTs/tokens.       |

> **Note:**  
> Only uncomment and set the optional authentication variables if you need external providers.

---

## 3. SSL Certificates (Optional)

**Required only for `docker-compose.yaml` and `docker-compose-dev.yaml` (HTTPS setups).**  
If you use `docker-compose-insecure.yaml`, you can skip this step.

By default, HTTPS is managed by Traefik using user-provided SSL certificate files stored in the `./certs` directory:

- `./certs/certificate.crt`
- `./certs/certificate.key`

To generate a self-signed certificate for development:

```bash
openssl req -x509 -nodes -days 365 -newkey rsa:2048 \
  -keyout ./certs/certificate.key \
  -out ./certs/certificate.crt \
  -subj "/CN=localhost"
```

Replace `localhost` with your actual domain if needed.

### Traefik TLS Configuration

Traefik is configured to use these certificates via the `tls.yaml` file.

- By default, `tls.yaml` is set up for user-provided certificates as above.
- **You are free to adapt `tls.yaml`** for Let’s Encrypt (automatic certificate provisioning) or for any other custom Traefik TLS configuration that fits your deployment.

For further details, see the [Traefik documentation on TLS and Let’s Encrypt](https://doc.traefik.io/traefik/https/tls/) and adapt `tls.yaml` according to your needs.

---

## 4. Running clippr

Choose the appropriate deployment scenario and use the corresponding command:

### Standard (HTTPS) Deployment

```bash
docker compose -f docker-compose.yaml up -d
```

### Insecure (HTTP, No SSL) Deployment

```bash
docker compose -f docker-compose-insecure.yaml up -d
```

### Local Development (HTTPS, Live Code)

```bash
docker compose -f docker-compose-dev.yaml up -d
```

To check container logs:

```bash
docker compose -f <compose-file> logs
```

Replace `<compose-file>` with the compose file you are using (e.g., `docker-compose.yaml`).

---

## Additional Configuration

You can further customize clippr by adjusting your environment files or Docker Compose files for advanced scenarios, such as using a custom database, adding authentication providers, or horizontal scaling.

---

## Troubleshooting

- Ensure certificate files are present and readable by containers (when using SSL).
- Make sure all required ports are open and not blocked by firewalls.
- Use `docker compose -f <compose-file> logs` or `docker compose -f <compose-file> logs <service>` for debugging.

---

## Further Reading

- [Docker Compose Documentation](https://docs.docker.com/compose/)
- [Traefik Documentation](https://doc.traefik.io/traefik/)
- [Traefik TLS & Let’s Encrypt](https://doc.traefik.io/traefik/https/tls/)

---
