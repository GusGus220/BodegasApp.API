FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
# Como ya estamos adentro de la carpeta, solo llamamos al archivo directo
RUN dotnet restore "BodegasApp.API.csproj"
RUN dotnet publish "BodegasApp.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BodegasApp.API.dll"]