# Hosting clippr with Docker Compose

## Deployment Overview

To deploy clippr using Docker Compose, follow these steps:

1. **[Clone the repository](#1-clone-the-repository)**
2. **[Copy and configure environment files](#2-environment-configuration)**
3. **[Set up SSL certificates](#3-ssl-certificates)**
4. **[Start the services](#4-running-clippr)**

Each step is explained in detail in the sections below.

---

## 1. Clone the repository

Clone the repository to your machine:

```bash
git clone git@github.com:simonfranken/clippr.git
cd clippr
```

---

## 2. Environment Configuration

clippr uses a multi-file environment configuration:

- `.env`: Used by Docker Compose for service orchestration (not injected into containers directly).
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

> **NOTE:**  
> Only uncomment and set the optional authentication variables if you need external providers.

---

## 3. SSL Certificates

For secure (HTTPS) connections, place your SSL certificate files in the `./certs` directory:

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

---

## 4. Running clippr

Launch the application with:

```bash
docker compose up -d
```

Check container logs with:

```bash
docker compose logs
```

---

## Additional Configuration

You can further customize clippr by adjusting your environment files or `docker-compose.yaml` for advanced scenarios such as custom databases, additional authentication, or scaling.

---

## Troubleshooting

- Ensure certificate files are present and readable by containers.
- Make sure all required ports are open and not blocked by firewalls.
- Use `docker compose logs` or `docker compose logs <service>` for debugging.

---

## Further Reading

- [Docker Compose Documentation](https://docs.docker.com/compose/)

---
