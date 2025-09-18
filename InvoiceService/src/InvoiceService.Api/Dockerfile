FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy only csproj files (for restore)
COPY InvoiceService/src/InvoiceService.Api/InvoiceService.Api.csproj InvoiceService/src/InvoiceService.Api/
COPY InvoiceService/src/InvoiceService.Application/InvoiceService.Application.csproj InvoiceService/src/InvoiceService.Application/
COPY InvoiceService/src/InvoiceService.Infrastructure/InvoiceService.Infrastructure.csproj InvoiceService/src/InvoiceService.Infrastructure/
COPY InvoiceService/src/InvoiceService.Domain/InvoiceService.Domain.csproj InvoiceService/src/InvoiceService.Domain/
COPY *.sln .
COPY Shared/ Shared/

# Restore
RUN dotnet restore

# Copy everything (preserves Program.cs and Startup.cs)
COPY . .

WORKDIR "/app/InvoiceService/src/InvoiceService.Api"
RUN dotnet publish -c Release -o /app/out

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT ["dotnet", "InvoiceService.Api.dll"]
