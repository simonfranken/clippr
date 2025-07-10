# clippr Backend

This is the backend API service for clippr. It manages clipboard content, user accounts, authentication tokens, and communicates with the identity service for authentication tasks.

## Technology Overview

- **Framework:** .NET 8
- **Database:** Entity Framework (EF Core) for managing storage and migrations
- **Authentication:** Validates JWTs issued by the identityService using ASP.NET Identity
- **API:** RESTful endpoints for clipboard management and content retrieval

## Environment Variables

| Environment Variable                                 | Default Value         | Description                                                  |
| ---------------------------------------------------- | --------------------- | ------------------------------------------------------------ |
| `Serilog__Using__0`                                  | Serilog.Sinks.Console | The logging sink used by Serilog (array item 0).             |
| `Serilog__MinimumLevel`                              | Information           | Minimum log level for Serilog.                               |
| `Serilog__WriteTo__0__Name`                          | Console               | Sets Serilog sink destination (array item 0: Console).       |
| `Logging__LogLevel__Default`                         | Information           | Default application log level.                               |
| `Logging__LogLevel__Microsoft.AspNetCore`            | Warning               | Log level for Microsoft.AspNetCore namespace.                |
| `CleanUp__CronExpression`                            | 0 \* \* \* \*         | Cron expression for scheduled cleanup job.                   |
| `CleanUp__Enabled`                                   | true                  | Whether the cleanup job is enabled.                          |
| `CleanUp__MaxClipAgeHours`                           | 3                     | Maximum age of clips (in hours) before cleanup runs.         |
| `Authentication__IdentityService__IssuerPublicUrl`   | **N/A**               | Public URL of the identity service issuer.                   |
| `Authentication__IdentityService__IssuerInternalUrl` | **N/A**               | Internal URL of the identity service issuer.                 |
| `Authentication__IdentityService__Audience`          | clippr                | Expected audience value for identity service authentication. |
| `Hosting__Url`                                       | **N/A**               | The URL the web server listens on.                           |
| `ConnectionStrings__Default`                         | **N/A**               | Default database connection string.                          |

> **Note:**  
> Variables without default value must be provided by you. All other variables may be customized if needed.

## Further Information

- For complete deployment and configuration examples, see the [hosting/docker README](../hosting/docker/README.md).

---
