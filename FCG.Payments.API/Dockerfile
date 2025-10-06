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

# FINAL STAGE - Aqui fazemos tudo do zero
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

# Mantém como root para instalação
USER root

WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# ============================================
# INSTALAÇÃO DO NEW RELIC
# ============================================
RUN apt-get update && \
    apt-get install -y wget ca-certificates gnupg && \
    mkdir -p /usr/share/keyrings && \
    wget -O- https://download.newrelic.com/548C16BF.gpg | gpg --dearmor -o /usr/share/keyrings/newrelic-archive-keyring.gpg && \
    echo "deb [signed-by=/usr/share/keyrings/newrelic-archive-keyring.gpg] http://apt.newrelic.com/debian/ newrelic non-free" > /etc/apt/sources.list.d/newrelic.list && \
    apt-get update && \
    apt-get install -y newrelic-dotnet-agent && \
    rm -rf /var/lib/apt/lists/*

# Verificar instalação (importante para debug)
RUN echo "Verificando instalação do New Relic..." && \
    ls -la /usr/local/newrelic-dotnet-agent/ && \
    test -f /usr/local/newrelic-dotnet-agent/libNewRelicProfiler.so && \
    echo "✅ New Relic instalado com sucesso!" || \
    (echo "❌ Falha na instalação do New Relic" && exit 1)

# Ajustar permissões
RUN chmod -R 755 /usr/local/newrelic-dotnet-agent

# ============================================
# CONFIGURAÇÃO DAS VARIÁVEIS DE AMBIENTE
# ============================================
ENV CORECLR_ENABLE_PROFILING=1 \
    CORECLR_PROFILER={36032161-FFC0-4B61-B559-F6C5D41BAE5A} \
    CORECLR_NEWRELIC_HOME=/usr/local/newrelic-dotnet-agent \
    CORECLR_PROFILER_PATH=/usr/local/newrelic-dotnet-agent/libNewRelicProfiler.so \
    NEW_RELIC_LICENSE_KEY=49fea977ae8cb8915399b2c027e24687FFFFNRAL \
    NEW_RELIC_APP_NAME="fcg-payments-api" \
    NEW_RELIC_LOG_LEVEL=debug

# Copiar aplicação publicada
COPY --from=publish /app/publish .

# Opcional: Adicionar script de verificação ao iniciar
RUN echo '#!/bin/bash\n\
echo "=== New Relic Status ==="\n\
echo "Profiler exists: $(test -f /usr/local/newrelic-dotnet-agent/libNewRelicProfiler.so && echo YES || echo NO)"\n\
echo "CORECLR_ENABLE_PROFILING: $CORECLR_ENABLE_PROFILING"\n\
echo "CORECLR_PROFILER_PATH: $CORECLR_PROFILER_PATH"\n\
echo "NEW_RELIC_APP_NAME: $NEW_RELIC_APP_NAME"\n\
echo "========================"\n\
exec dotnet FCG.Payments.API.dll' > /app/start.sh && \
    chmod +x /app/start.sh

ENTRYPOINT ["/app/start.sh"]