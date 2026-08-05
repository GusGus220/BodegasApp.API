# 1. Usar el SDK de .NET para compilar el código (Ojo: Cambia 8.0 por 9.0 si usas .NET 9)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "BodegasApp.API.csproj"
RUN dotnet publish "BodegasApp.API.csproj" -c Release -o /app/publish

# 2. Usar la imagen ligera para correr la API en la nube
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BodegasApp.API.dll"]