FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy only csproj files (for restore)
COPY IdentityService/src/IdentityService.Api/IdentityService.Api.csproj IdentityService/src/IdentityService.Api/
COPY IdentityService/src/IdentityService.Application/IdentityService.Application.csproj IdentityService/src/IdentityService.Application/
COPY IdentityService/src/IdentityService.Infrastructure/IdentityService.Infrastructure.csproj IdentityService/src/IdentityService.Infrastructure/
COPY IdentityService/src/IdentityService.Domain/IdentityService.Domain.csproj IdentityService/src/IdentityService.Domain/
COPY *.sln .
COPY Shared/ Shared/

# Restore
RUN dotnet restore

# Copy everything (preserves Program.cs and Startup.cs)
COPY . .

WORKDIR "/app/IdentityService/src/IdentityService.Api"
RUN dotnet publish -c Release -o /app/out

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT ["dotnet", "IdentityService.Api.dll"]
