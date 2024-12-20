# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

EXPOSE 5083
EXPOSE 8002

COPY ["Services/Article", "Services/Article"]
COPY ["Shared", "Shared"]


WORKDIR /src/Services/Article/Article.API
RUN dotnet restore

RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build


FROM build AS publish
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Article.API.dll"]
