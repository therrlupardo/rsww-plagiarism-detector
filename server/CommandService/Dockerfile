FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["CommandService/CommandService.csproj", "CommandService/"]
RUN dotnet restore "CommandService/CommandService.csproj"
COPY . .
WORKDIR "/src/CommandService"
RUN dotnet build "CommandService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CommandService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
HEALTHCHECK --interval=10s --timeout=10s --retries=3 CMD curl --fail http://localhost:80/healthcheck || exit 1
ENTRYPOINT ["dotnet", "CommandService.dll"]