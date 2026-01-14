FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
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
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "App_View.dll"]
