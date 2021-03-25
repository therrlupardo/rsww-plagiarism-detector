FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src

COPY QueryService/ ./QueryService/
COPY Common/ ./Common/
COPY Queries/ ./Queries/

RUN dotnet restore QueryService/QueryService.csproj &&\
    dotnet build QueryService/QueryService.csproj -c Release -o /app/build &&\
    dotnet publish QueryService/QueryService.csproj -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim
WORKDIR /app

COPY --from=build /app/publish .

HEALTHCHECK --interval=10s --timeout=10s --retries=3 CMD curl --fail http://localhost:80/healthcheck || exit 1
ENTRYPOINT ["dotnet", "QueryService.dll"]