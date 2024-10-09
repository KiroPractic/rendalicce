# Use the official Microsoft .NET SDK image for building the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore any dependencies (as a separate step for caching)
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build the project
COPY . ./
RUN dotnet publish -c Development -o out /p:UseAppHost=false

# Use the official ASP.NET Core runtime image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Expose the port for the app
EXPOSE 5000
ENTRYPOINT ["dotnet", "Rendalicce.dll"]