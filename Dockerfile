# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia todo el contenido del contexto (./Auth) al build
COPY . .

# Ajusta el nombre del proyecto si no es Auth.csproj
# Si tu .csproj tiene otro nombre, reemplaza Auth.csproj por el correcto
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# crear user no-root
RUN useradd --create-home --shell /bin/bash appuser \
    && chown -R appuser:appuser /app

COPY --from=build /app/publish ./

ENV ASPNETCORE_URLS=http://+:80 \
    ASPNETCORE_ENVIRONMENT=Production \
    DOTNET_RUNNING_IN_CONTAINER=true

EXPOSE 80
USER appuser
ENTRYPOINT ["dotnet", "Auth.dll"]
