# clippr IdentityService

This is the authentication and identity provider for clippr. It issues JWTs, manages user authentication using ASP.NET Identity, and supports multiple authentication methods (including email/password and OIDC providers). Other backend clients can validate JWTs via the public JWKS endpoint.

## Technology Overview

- **Framework:** .NET 8
- **Authentication:** ASP.NET Identity for user management and authentication
- **Token Issuance:** Generates JWTs for authenticated clients
- **OIDC Support:** Can be configured as an identity provider and use external OIDC identity providers
- **API:** Endpoints for performing authentication/login, registering and managing users, and issuing JWTs

## Environment Variables

| Environment Variable                                | Default Value         | Description                                                                                      |
| --------------------------------------------------- | --------------------- | ------------------------------------------------------------------------------------------------ |
| `Serilog__Using__0`                                 | Serilog.Sinks.Console | The logging sink used by Serilog (array item 0).                                                 |
| `Serilog__MinimumLevel`                             | Information           | Minimum log level for Serilog.                                                                   |
| `Serilog__WriteTo__0__Name`                         | Console               | Sets Serilog sink destination (array item 0: Console).                                           |
| `Logging__LogLevel__Default`                        | Information           | Default application log level.                                                                   |
| `Logging__LogLevel__Microsoft.AspNetCore`           | Warning               | Log level for Microsoft.AspNetCore namespace.                                                    |
| `AllowedHosts`                                      | \*                    | Allowed hosts for the application.                                                               |
| `JwtSettings__Issuer`                               | clipprIdentity        | JWT token issuer name.                                                                           |
| `JwtSettings__Audience`                             | clippr                | Expected audience for JWT authentication.                                                        |
| `JwtSettings__ExpiresInMinutes`                     | 60                    | JWT token expiration time (in minutes).                                                          |
| `FeatureManagement__InternalAuth`                   | true                  | Enables or disables internal authentication features.                                            |
| `Hosting__Url`                                      | **N/A**               | The URL the web server listens on.                                                               |
| `Authentication__ExternalProviders__0__ProviderKey` | **N/A**               | _(Optional)_ The key identifying the external authentication provider (first provider in array). |
| `Authentication__ExternalProviders__0__Issuer`      | **N/A**               | _(Optional)_ The issuer URL for the external authentication provider.                            |
| `Authentication__ExternalProviders__0__Audience`    | **N/A**               | _(Optional)_ The audience value expected by the external authentication provider.                |
| `ConnectionStrings__Default`                        | **N/A**               | The default database connection string.                                                          |

> **Note:**  
> Variables without default value must be provided by you. All other variables may be customized if needed.

## Further Information

- For complete deployment and configuration examples, see the [hosting/docker README](../hosting/docker/README.md).

---
