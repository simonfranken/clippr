FROM mcr.microsoft.com/dotnet/sdk:8.0@sha256:8ab06772f296ed5f541350334f15d9e2ce84ad4b3ce70c90f2e43db2752c30f6 AS build
WORKDIR /src

COPY clippr.API/clippr.API.csproj clippr.API/
COPY clippr.Core/clippr.Core.csproj clippr.Core/
COPY clippr.Repository/clippr.Repository.csproj clippr.Repository/

RUN dotnet restore ./clippr.API/.

COPY . .
WORKDIR /src/clippr.API
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0@sha256:6159cf66274cf52730d7a0c7bb05cf0af94b79370176886ac58286ab6cbb7faf AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

EXPOSE 8080
ENTRYPOINT ["dotnet", "clippr.API.dll"]