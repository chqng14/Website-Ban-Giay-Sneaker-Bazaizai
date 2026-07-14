FROM mcr.microsoft.com/dotnet/aspnet:8.0.28 AS base
WORKDIR /app
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0.422 AS build
WORKDIR /src
COPY ["App_Api/App_Api.csproj", "App_Api/"]
COPY ["AppData/App_Data.csproj", "AppData/"]
RUN dotnet restore "App_Api/App_Api.csproj"
COPY . .
WORKDIR "/src/App_Api"
RUN dotnet build "App_Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "App_Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
RUN mkdir -p /app/storage && chown -R $APP_UID:$APP_UID /app
COPY --from=publish --chown=$APP_UID:$APP_UID /app/publish .
USER $APP_UID
ENTRYPOINT ["dotnet", "App_Api.dll"]
