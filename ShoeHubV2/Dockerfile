FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
# TARGETARCH detects the host system (amd64 or arm64) during multi-arch builds
ARG TARGETARCH
WORKDIR /src
COPY ["ShoeHubV2.csproj", "./"]
# Restore packages targeting the correct host architecture to prevent Mac emulation failures
RUN dotnet restore "ShoeHubV2.csproj" -a $TARGETARCH
COPY . .
WORKDIR "/src"
RUN dotnet build "ShoeHubV2.csproj" -c $BUILD_CONFIGURATION -o /app/build -a $TARGETARCH

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
ARG TARGETARCH
RUN dotnet publish "ShoeHubV2.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false -a $TARGETARCH

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShoeHubV2.dll"]