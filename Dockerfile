# === STAGE 1: Build ===
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY Manager.sln ./

COPY TimerService/TimerService.Server/TimerService.Server.csproj TimerService/TimerService.Server/
COPY TimerService/TimerService.Client/TimerService.Client.csproj TimerService/TimerService.Client/
COPY AuthenticationService/AuthenticationService.Client/AuthenticationService.Client.csproj AuthenticationService/AuthenticationService.Client/
COPY Core/Manager.Core/Manager.Core.csproj Core/Manager.Core/
COPY Tool/Manager.Tool/Manager.Tool.csproj Tool/Manager.Tool/

RUN dotnet restore TimerService/TimerService.Server/TimerService.Server.csproj

COPY . .

RUN dotnet publish TimerService/TimerService.Server/TimerService.Server.csproj -c Release -o /app/publish

# === STAGE 2: Runtime ===
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Manager.TimerService.Server.dll"]
