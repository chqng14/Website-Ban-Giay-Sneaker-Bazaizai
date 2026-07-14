FROM mcr.microsoft.com/dotnet/aspnet:8.0.28 AS base
WORKDIR /app
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0.422 AS build
WORKDIR /src
COPY ["App_View/App_View.csproj", "App_View/"]
COPY ["AppData/App_Data.csproj", "AppData/"]
RUN dotnet restore "App_View/App_View.csproj"
COPY . .
WORKDIR "/src/App_View"
RUN dotnet build "App_View.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "App_View.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
RUN mkdir -p /app/storage /app/keys && chown -R $APP_UID:$APP_UID /app
COPY --from=publish --chown=$APP_UID:$APP_UID /app/publish .
USER $APP_UID
ENTRYPOINT ["dotnet", "App_View.dll"]
