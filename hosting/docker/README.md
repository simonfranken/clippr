# Hosting clippr with Docker Compose

This guide describes how to deploy the clippr web tool using Docker Compose. These instructions cover the minimal steps needed for setup, including SSL configuration and environment variables.

## Prerequisites

- [Docker](https://www.docker.com/get-started) and [Docker Compose](https://docs.docker.com/compose/) installed on your system.
- Optional: A domain name if you intend to access the application over the internet. Localhost usage is fully supported.
- Optional: Familiarity with environment variables for advanced configuration.

---

## 1. SSL Certificates

For secure HTTPS connections, place your SSL certificate files in the `./certs` directory:

- `./certs/certificate.crt`
- `./certs/certificate.key`

If you do not have a trusted SSL certificate, you can generate a self-signed certificate for testing or local usage. Use `localhost` as the Common Name if not using a custom domain:

```bash
openssl req -x509 -nodes -days 365 -newkey rsa:2048 \
  -keyout ./certs/certificate.key \
  -out ./certs/certificate.crt \
  -subj "/CN=localhost"
```

Replace `localhost` with your domain name if applicable.

---

## 2. Environment Variables (`.env.template`)

The `.env` file configures the clippr backend and authentication. Copy `.env.template` to `.env` and update the values as needed.

Below are the available configuration options, their defaults, status, and a brief description:

| Variable                                            | Default Value           | Required | Description                                                        |
| --------------------------------------------------- | ----------------------- | -------- | ------------------------------------------------------------------ |
| `CLIPPR_URL`                                        | `https://localhost:443` | Yes      | The base URL at which the clippr application is served.            |
| `Authentication__ExternalProviders__0__ProviderKey` | _EMPTY_                 | No       | (Optional) Key/name for an external authentication provider.       |
| `Authentication__ExternalProviders__0__Issuer`      | _EMPTY_                 | No       | (Optional) Issuer URL for the external authentication provider.    |
| `Authentication__ExternalProviders__0__Audience`    | _EMPTY_                 | No       | (Optional) Audience (client ID) for validating the JWTs or tokens. |

**Note:**

- To enable any of the optional authentication fields, remove the leading `#` and fill in the appropriate values.

---

## 3. Starting the Services

Launch the application stack in detached mode with the following command:

```bash
docker compose up -d
```

Monitor the containers and logs to ensure the services are running as expected.

---

## Additional Configuration

clippr can be further customized by extending the `.env` file or adjusting `docker-compose.yaml`.
