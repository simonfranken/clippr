FROM mcr.microsoft.com/dotnet/sdk:8.0@sha256:e6a5a8d884609907fa8d468b927df765967f6b22f890ce92bd3ae614ca4ae87e AS src
WORKDIR /src

COPY clippr.API/clippr.API.csproj clippr.API/
COPY clippr.Core/clippr.Core.csproj clippr.Core/
COPY clippr.Repository/clippr.Repository.csproj clippr.Repository/

RUN dotnet restore ./clippr.API/.

COPY . .


FROM mcr.microsoft.com/dotnet/sdk:8.0@sha256:e6a5a8d884609907fa8d468b927df765967f6b22f890ce92bd3ae614ca4ae87e AS debug
RUN apt-get update && apt-get install -y unzip curl \
    && curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l /vsdbg

COPY selfsigned.crt /usr/local/share/ca-certificates/
RUN update-ca-certificates

COPY --from=src /root/.dotnet /root/.dotnet
COPY --from=src /root/.nuget /root/.nuget
COPY --from=src /root/.local /root/.local
COPY --from=src /src /src

WORKDIR /app
RUN dotnet build /src/clippr.API/clippr.API.csproj -c Debug -o /app

ENV DOTNET_USE_POLLING_FILE_WATCHER=true \
    DOTNET_HOSTBUILDER__RELOADCONFIGONCHANGE=false \
    DOTNET_EnableDiagnostics=1 \
    DOTNET_DbgEnable=1

EXPOSE 8080
    
ENTRYPOINT ["dotnet", "clippr.API.dll", "--environment", "Development"]


FROM src AS publish

WORKDIR /src/clippr.API
RUN dotnet publish -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:8.0@sha256:d5c0d91bc8fe887684b97d5409056270ed78cd23a5123436e842a8114a64d584 AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

EXPOSE 8080
ENTRYPOINT ["dotnet", "clippr.API.dll"]