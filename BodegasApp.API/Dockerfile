# Cambiamos 8.0 por 10.0 aquí:
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
# AQUI LE DECIMOS QUE ENTRE A LA CARPETA
RUN dotnet restore "BodegasApp.API/BodegasApp.API.csproj"
RUN dotnet publish "BodegasApp.API/BodegasApp.API.csproj" -c Release -o /app/publish

# Cambiamos 8.0 por 10.0 aquí también:
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BodegasApp.API.dll"]