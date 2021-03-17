FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["QueryService/QueryService.csproj", "QueryService/"]
RUN dotnet restore "QueryService/QueryService.csproj"
COPY . .
WORKDIR "/src/QueryService"
RUN dotnet build "QueryService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "QueryService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
HEALTHCHECK --interval=10s --timeout=10s --retries=3 CMD curl --fail http://localhost:80/healthcheck || exit 1
ENTRYPOINT ["dotnet", "QueryService.dll"]