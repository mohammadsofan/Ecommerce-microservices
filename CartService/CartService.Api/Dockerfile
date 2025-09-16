# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["Ecommerce.sln", "./"]
COPY ["CartService/CartService.Api/CartService.Api.csproj", "CartService/CartService.Api/"]
COPY ["CartService/CartService.Application/CartService.Application.csproj", "CartService/CartService.Application/"]
COPY ["CartService/CartService.Domain/CartService.Domain.csproj", "CartService/CartService.Domain/"]
COPY ["CartService/CartService.Infrastructure/CartService.Infrastructure.csproj", "CartService/CartService.Infrastructure/"]
COPY ["Shared/Shared.csproj", "Shared/"]

# Restore NuGet packages
RUN dotnet restore "CartService/CartService.Api/CartService.Api.csproj"

# Copy source code
COPY ["CartService/CartService.Api/", "CartService/CartService.Api/"]
COPY ["CartService/CartService.Application/", "CartService/CartService.Application/"]
COPY ["CartService/CartService.Domain/", "CartService/CartService.Domain/"]
COPY ["CartService/CartService.Infrastructure/", "CartService/CartService.Infrastructure/"]
COPY ["Shared/", "Shared/"]

# Build the application
RUN dotnet build "CartService/CartService.Api/CartService.Api.csproj" -c Release -o /app/build

# Publish Stage
FROM build AS publish
RUN dotnet publish "CartService/CartService.Api/CartService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Copy published files from publish stage
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "CartService.Api.dll"]
RUN dotnet build "CartService.Api/CartService.Api.csproj" -c Release -o /app/build

# Publish Stage
FROM build AS publish
RUN dotnet publish "CartService/CartService.Api/CartService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Copy published files from publish stage
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "CartService.Api.dll"]
