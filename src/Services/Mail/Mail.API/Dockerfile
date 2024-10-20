FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Mail/Mail.API/Mail.API.csproj", "Services/Mail/Mail.API/"]
COPY ["Shared/BuildingBlocks/BuildingBlocks/BuildingBlocks.csproj", "Shared/BuildingBlocks/BuildingBlocks/"]
COPY ["Shared/ShredKernel/ShredKernel/ShredKernel.csproj", "Shared/ShredKernel/ShredKernel/"]
COPY ["Shared/BuildingBlocks/BuildingBlocks.Messaging/BuildingBlocks.Messaging.csproj", "Shared/BuildingBlocks/BuildingBlocks.Messaging/"]
RUN dotnet restore "Services/Mail/Mail.API/Mail.API.csproj"
COPY . .
WORKDIR "/src/Services/Mail/Mail.API"
RUN dotnet build "Mail.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Mail.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mail.API.dll"]
