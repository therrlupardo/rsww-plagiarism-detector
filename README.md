# Project from Distributed Systems of High Availability 

## Running using docker-compose

1. Go to directory `server`
2. Run docker using `docker-compose up --build` 
    - You can skip flag `--build`, if you want to run containers without rebuilding
3. To stop and remove containers use `docker-compose down`. 
    - You can add flag `-v` to remove also all volumes 

## Running locally

1. Go to service you want to run
2. Restore dependencies using `dotnet restore`
3. Run service using `dotnet run`

> Running locally assumes that postgres is running on localhost:5432 (for example as docker container run with `docker-compose up postgres`). If you want to connect to another instance change connection string in `IdentityService/appsettings.Development.json`

## API Documentation
Project is using swagger as API documentation. To access it you go to page `localhost:8080/swagger` (assuming you are running docker version). 

Swagger should be also available for all services running locally at `service_host:service_port/swagger`.

## API authorization

API is secured using JWT tokens. To get one go to IdentityService (also available via swagger) and use `/api/identity/login`. If your credentials are correct in response you will find `accessToken`. To authorize your request add `Authorization: Bearer {token}` to it's headers.
Example response:
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIwMjQ2NWY5OS01ZjM5LTRiZmQtOTM1NS0wZjNmYWYxODM2Y2YiLCJqdGkiOiJmMGEzOWRlNy00NmE3LTQ2MjAtYmY0Yi03NDcxODhiODM4ODMiLCJpYXQiOiIwMy8xNS8yMDIxIDEwOjEyOjQ5IiwibmJmIjoxNjE1ODAzMTY5LCJleHAiOjE2MTU4MDQ5NjksImlzcyI6InRlc3QiLCJhdWQiOiJ0ZXN0In0.kQvY98tAl8MTbWG0SHByPeELYW0ZXZLYK_18wWSIRMg",
  "expiresAt": 1800
}
```

Example header:
```http
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIwMjQ2NWY5OS01ZjM5LTRiZmQtOTM1NS0wZjNmYWYxODM2Y2YiLCJqdGkiOiJmMGEzOWRlNy00NmE3LTQ2MjAtYmY0Yi03NDcxODhiODM4ODMiLCJpYXQiOiIwMy8xNS8yMDIxIDEwOjEyOjQ5IiwibmJmIjoxNjE1ODAzMTY5LCJleHAiOjE2MTU4MDQ5NjksImlzcyI6InRlc3QiLCJhdWQiOiJ0ZXN0In0.kQvY98tAl8MTbWG0SHByPeELYW0ZXZLYK_18wWSIRMg