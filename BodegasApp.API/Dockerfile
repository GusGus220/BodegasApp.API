FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
# Ya no ponemos la ruta doble, solo el nombre del archivo
RUN dotnet restore "BodegasApp.API.csproj"
RUN dotnet publish "BodegasApp.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BodegasApp.API.dll"]