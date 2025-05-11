FROM mcr.microsoft.com/dotnet/sdk:8.0@sha256:8ab06772f296ed5f541350334f15d9e2ce84ad4b3ce70c90f2e43db2752c30f6
RUN apt-get update && apt-get install -y unzip curl \
    && curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l /vsdbg

WORKDIR /src

COPY ./clippr.API/*.csproj ./clippr.API/.
COPY ./clippr.Repository/*.csproj ./clippr.Repository/.
COPY ./clippr.Core/*.csproj ./clippr.Core/.

RUN dotnet restore ./clippr.API/.

COPY . .

WORKDIR /app
RUN dotnet build /src/clippr.API/clippr.API.csproj -c Debug -o /app

ENV DOTNET_USE_POLLING_FILE_WATCHER=true \
    DOTNET_HOSTBUILDER__RELOADCONFIGONCHANGE=false \
    DOTNET_EnableDiagnostics=1 \
    DOTNET_DbgEnable=1

EXPOSE 8080
    
# Enable debugging
ENTRYPOINT ["dotnet", "clippr.API.dll", "--environment", "Development"]