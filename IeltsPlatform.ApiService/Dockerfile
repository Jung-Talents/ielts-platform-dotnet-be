# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["IeltsPlatform.ApiService/IeltsPlatform.ApiService.csproj", "IeltsPlatform.ApiService/"]
COPY ["IeltsPlatform.ServiceDefaults/IeltsPlatform.ServiceDefaults.csproj", "IeltsPlatform.ServiceDefaults/"]
RUN dotnet restore "IeltsPlatform.ApiService/IeltsPlatform.ApiService.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/IeltsPlatform.ApiService"
RUN dotnet build "IeltsPlatform.ApiService.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "IeltsPlatform.ApiService.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8443

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IeltsPlatform.ApiService.dll"]
