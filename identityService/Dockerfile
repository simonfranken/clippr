FROM mcr.microsoft.com/dotnet/sdk:8.0@sha256:69e1e4217e359e7dcb1d6f5fe1fc6e6c26a486ebbf51b696ee02d274e47ffe56 AS src
WORKDIR /src

COPY clippr.IdentityService.API/clippr.IdentityService.API.csproj clippr.IdentityService.API/
COPY clippr.IdentityService.Core/clippr.IdentityService.Core.csproj clippr.IdentityService.Core/

RUN dotnet restore ./clippr.IdentityService.API/.

COPY . .


FROM mcr.microsoft.com/dotnet/sdk:8.0@sha256:69e1e4217e359e7dcb1d6f5fe1fc6e6c26a486ebbf51b696ee02d274e47ffe56 AS debug
RUN apt-get update && apt-get install -y unzip curl \
    && curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l /vsdbg

COPY --from=src /src /src

WORKDIR /app
RUN dotnet build /src/clippr.IdentityService.API/clippr.IdentityService.API.csproj -c Debug -o /app

ENV DOTNET_USE_POLLING_FILE_WATCHER=true \
    DOTNET_HOSTBUILDER__RELOADCONFIGONCHANGE=false \
    DOTNET_EnableDiagnostics=1 \
    DOTNET_DbgEnable=1

EXPOSE 8080
    
ENTRYPOINT ["dotnet", "clippr.IdentityService.API.dll", "--environment", "Development"]


FROM src AS publish

WORKDIR /src/clippr.IdentityService.API
RUN dotnet publish -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:8.0@sha256:c149fe7e2be3baccf3cc91e9e6cdcca0ce70f7ca30d5f90796d983ff4f27bd9a AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

EXPOSE 8080
ENTRYPOINT ["dotnet", "clippr.IdentityService.API.dll"]