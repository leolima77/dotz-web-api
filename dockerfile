
FROM microsoft/dotnet:2.2-sdk-alpine AS build-env
WORKDIR /app

# Copiar csproj e restaurar dependencias
COPY . ./
RUN dotnet restore

# Build da aplicacao
COPY . ./
RUN dotnet publish -c Release -o /app/out

# Build da imagem
FROM microsoft/dotnet:2.2-aspnetcore-runtime-alpine
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Dotz.WebApi.dll"]
#CMD ASPNETCORE_URLS=http://*:$PORT dotnet Dotz.WebApi.dll 