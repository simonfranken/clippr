FROM mcr.microsoft.com/dotnet/sdk:8.0@sha256:363f595704ae9f9be51db003e49b75f65197b0a44d39cba6f7e2d9b020458604 AS src
WORKDIR /src

COPY clippr.IdentityService.API/clippr.IdentityService.API.csproj clippr.IdentityService.API/

RUN dotnet restore ./clippr.IdentityService.API/.

COPY . .


FROM mcr.microsoft.com/dotnet/sdk:8.0@sha256:363f595704ae9f9be51db003e49b75f65197b0a44d39cba6f7e2d9b020458604 AS debug
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


FROM mcr.microsoft.com/dotnet/aspnet:8.0@sha256:2d386e5e5099095e7dd8f0a884e60a2ac50d42005a8783e842b3af67a2320f72 AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

EXPOSE 8080
ENTRYPOINT ["dotnet", "clippr.IdentityService.API.dll"]