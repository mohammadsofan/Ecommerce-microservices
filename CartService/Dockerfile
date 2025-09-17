# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["CartService/*.sln", "./"]
COPY ["CartService/CartService.Api/*.csproj", "CartService.Api/"]
COPY ["CartService/CartService.Application/*.csproj", "CartService.Application/"]
COPY ["CartService/CartService.Domain/*.csproj", "CartService.Domain/"]
COPY ["CartService/CartService.Infrastructure/*.csproj", "CartService.Infrastructure/"]
COPY ["Shared/*.csproj", "Shared/"]

RUN dotnet restore "CartService.Api/CartService.Api.csproj"

# Copy all source files and build
COPY CartService/ ./
COPY Shared/ Shared/
RUN dotnet build "CartService.Api/CartService.Api.csproj" -c Release -o /app/build

# Publish Stage
FROM build AS publish
RUN dotnet publish "CartService.Api/CartService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

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
