# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/services/order/api/Learnify.Order.API/Learnify.Order.API.csproj", "src/services/order/api/Learnify.Order.API/"]
COPY ["src/shared/Learnify.Shared/Learnify.Shared.csproj", "src/shared/Learnify.Shared/"]
COPY ["src/services/order/infrastructure/Learnify.Order.Persistence/Learnify.Order.Persistence.csproj", "src/services/order/infrastructure/Learnify.Order.Persistence/"]
COPY ["src/services/order/core/Learnify.Order.Application/Learnify.Order.Application.csproj", "src/services/order/core/Learnify.Order.Application/"]
COPY ["src/shared/Learnify.Bus/Learnify.Bus.csproj", "src/shared/Learnify.Bus/"]
COPY ["src/services/order/core/Learnify.Order.Domain/Learnify.Order.Domain.csproj", "src/services/order/core/Learnify.Order.Domain/"]
RUN dotnet restore "./src/services/order/api/Learnify.Order.API/Learnify.Order.API.csproj"
COPY . .
WORKDIR "/src/src/services/order/api/Learnify.Order.API"
RUN dotnet build "./Learnify.Order.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Learnify.Order.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Learnify.Order.API.dll"]