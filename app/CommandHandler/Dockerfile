FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src

COPY CommandHandler/ ./CommandHandler/
COPY OperationContracts/ ./OperationContracts/
COPY Common/ ./Common/
COPY EventsFacade/ ./EventsFacade/

RUN dotnet restore CommandHandler/CommandHandler.csproj &&\
    dotnet build CommandHandler/CommandHandler.csproj -c Release -o /app/build &&\
    dotnet publish CommandHandler/CommandHandler.csproj -c Release -o /app/publish


FROM ubuntu:focal

ENV DEBIAN_FRONTEND noninteractive

WORKDIR /app

COPY --from=build /app/publish .
COPY spark_analysis_scripts/ ./spark_analysis_scripts

RUN apt-get update -y &&\ 
    apt-get install -y python3.8 python3-pip &&\
    mkdir -p /usr/share/man/man1 &&\
    apt-get install --no-install-recommends -y openjdk-11-jre-headless

RUN apt-get install -y gpg wget &&\
    wget -O - https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor -o microsoft.asc.gpg &&\
    mv microsoft.asc.gpg /etc/apt/trusted.gpg.d/ &&\
    wget https://packages.microsoft.com/config/ubuntu/20.10/prod.list &&\
    mv prod.list /etc/apt/sources.list.d/microsoft-prod.list &&\
    chown root:root /etc/apt/trusted.gpg.d/microsoft.asc.gpg &&\
    chown root:root /etc/apt/sources.list.d/microsoft-prod.list &&\
    apt-get update &&\
    apt-get install -y apt-transport-https &&\
    apt-get update &&\
    apt-get install -y aspnetcore-runtime-5.0

RUN pip3 install pyspark==3.1.1

ENV SPARK_DRIVER_PORT=5001 \
    SPARK_UI_PORT=5002 \
    SPARK_BLOCKMANAGER_PORT=5003
EXPOSE $SPARK_DRIVER_PORT $SPARK_UI_PORT $SPARK_BLOCKMANAGER_PORT

HEALTHCHECK --interval=10s --timeout=10s --retries=3 CMD curl --fail http://localhost:80/healthcheck || exit 1
ENTRYPOINT ["dotnet", "CommandHandler.dll"]
