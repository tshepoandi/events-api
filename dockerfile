# Use an official .NET runtime as a parent image
FROM mcr.microsoft.com/dotnet/core/sdk:3.1

# Set the working directory in the container
WORKDIR /app

# Copy the .NET project file
COPY *.csproj ./

# Restore NuGet packages
RUN dotnet restore

# Copy the rest of the application code
COPY . .

# Build the .NET application
RUN dotnet build -c Release

# Expose the port the application will run on
EXPOSE 80

# Run the command to start the .NET application when the container launches
CMD ["dotnet", "run"]