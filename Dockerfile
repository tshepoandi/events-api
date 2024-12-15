# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj first and restore
COPY ["backends.csproj", "./"]
RUN dotnet restore "backends.csproj"

# Copy everything else and build
COPY . .
RUN dotnet publish "backends.csproj" -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Configure environment variables
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true

EXPOSE 5000
EXPOSE 8080
ENTRYPOINT ["dotnet", "backends.dll"]