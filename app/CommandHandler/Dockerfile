FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src

COPY CommandHandler/ ./CommandHandler/
COPY OperationContracts/ ./OperationContracts/
COPY Common/ ./Common/
COPY EventsFacade/ ./EventsFacade/

RUN dotnet restore CommandHandler/CommandHandler.csproj &&\
    dotnet build CommandHandler/CommandHandler.csproj -c Release -o /app/build &&\
    dotnet publish CommandHandler/CommandHandler.csproj -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim
WORKDIR /app

COPY --from=build /app/publish .

COPY spark_analysis_scripts/ ./spark_analysis_scripts

RUN apt-get update -y &&\ 
    apt-get install -y python3
RUN python3 -m pip install pyspark findspark

HEALTHCHECK --interval=10s --timeout=10s --retries=3 CMD curl --fail http://localhost:80/healthcheck || exit 1
ENTRYPOINT ["dotnet", "CommandHandler.dll"]