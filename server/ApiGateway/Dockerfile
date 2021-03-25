FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src

COPY ApiGateway/ ./ApiGateway
COPY Common/ ./Common

RUN dotnet restore ApiGateway/ApiGateway.csproj &&\
    dotnet build ApiGateway/ApiGateway.csproj -c Release -o /app/build &&\
    dotnet publish ApiGateway/ApiGateway.csproj -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim
WORKDIR /app
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/app/aspnetapp.pfx

COPY --from=build /app/publish .
COPY ./ssl/aspnetapp.pfx .

HEALTHCHECK --interval=10s --timeout=10s --retries=3 CMD curl --fail http://localhost:80/healthcheck || exit 1
EXPOSE 443
EXPOSE 80
ENTRYPOINT ["dotnet", "ApiGateway.dll"]