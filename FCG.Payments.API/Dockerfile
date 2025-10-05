# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# ✅ Troca para root para conseguir instalar pacotes
USER root

RUN apt-get update && \
    apt-get install -y wget ca-certificates gnupg && \
    mkdir -p /usr/share/keyrings && \
    wget -O- https://download.newrelic.com/548C16BF.gpg | gpg --dearmor -o /usr/share/keyrings/newrelic-archive-keyring.gpg && \
    echo "deb [signed-by=/usr/share/keyrings/newrelic-archive-keyring.gpg] http://apt.newrelic.com/debian/ newrelic non-free" > /etc/apt/sources.list.d/newrelic.list && \
    apt-get update && \
    apt-get install -y newrelic-dotnet-agent && \
    rm -rf /var/lib/apt/lists/*

# Ajustar permissões do New Relic para o usuário da aplicação
RUN chmod -R 755 /usr/local/newrelic-dotnet-agent && \
    mkdir -p /usr/local/newrelic-dotnet-agent/logs && \
    chmod -R 777 /usr/local/newrelic-dotnet-agent/logs

# ✅ Volta para o usuário padrão
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Enable the agent
ENV CORECLR_ENABLE_PROFILING=1 \
    CORECLR_PROFILER={36032161-FFC0-4B61-B559-F6C5D41BAE5A} \
    CORECLR_NEWRELIC_HOME=/usr/local/newrelic-dotnet-agent \
    CORECLR_PROFILER_PATH=/usr/local/newrelic-dotnet-agent/libNewRelicProfiler.so \
    NEW_RELIC_LICENSE_KEY=49fea977ae8cb8915399b2c027e24687FFFFNRAL \
    NEW_RELIC_APP_NAME="fcg-payments-api" \
    NEW_RELIC_LOG_LEVEL=debug \
    NEW_RELIC_LOG_CONSOLE=true

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FCG.Payments.API/FCG.Payments.API.csproj", "FCG.Payments.API/"]
COPY ["FCG.Payments.Application/FCG.Payments.Application.csproj", "FCG.Payments.Application/"]
COPY ["FCG.Payments.Domain/FCG.Payments.Domain.csproj", "FCG.Payments.Domain/"]
COPY ["FCG.Payments.Infra.Data/FCG.Payments.Infra.Data.csproj", "FCG.Payments.Infra.Data/"]
RUN dotnet restore "./FCG.Payments.API/FCG.Payments.API.csproj"
COPY . .
WORKDIR "/src/FCG.Payments.API"
RUN dotnet build "./FCG.Payments.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FCG.Payments.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FCG.Payments.API.dll"]