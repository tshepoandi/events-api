# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy everything, restore dependencies, and publish
COPY . . 
RUN dotnet restore backends.csproj && \
    dotnet publish backends.csproj -c Release -o out

# Copy the rest of the application code
COPY . .

# Publish the application
RUN dotnet publish backends.csproj -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy the published output
COPY --from=build /app/out .

# Configure ASP.NET Core for proxied HTTPS
ENV ASPNETCORE_URLS="http://+:80"
ENV ASPNETCORE_ENVIRONMENT="Production"
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true

EXPOSE 80
EXPOSE 5000

ENTRYPOINT ["dotnet", "backends.dll"]