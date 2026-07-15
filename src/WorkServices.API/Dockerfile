FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

COPY . .

RUN dotnet restore src/WorkServices.API/WorkServices.API.csproj

RUN dotnet publish \
    src/WorkServices.API/WorkServices.API.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "WorkServices.API.dll"]