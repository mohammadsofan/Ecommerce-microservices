FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy csproj and restore dependencies
# Copy solution and project files
COPY *.sln .
COPY EmailService/EmailService.Api/*.csproj EmailService/EmailService.Api/
COPY Shared/Shared.csproj Shared/

# Copy Shared project files
COPY Shared/ Shared/

# Copy EmailService.Api project files
COPY EmailService/EmailService.Api/ EmailService/EmailService.Api/

WORKDIR /app/EmailService/EmailService.Api

RUN dotnet restore

RUN dotnet publish -c Release -o /app/out
# Use the official ASP.NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
# Entry point
ENTRYPOINT ["dotnet", "EmailService.Api.dll"]