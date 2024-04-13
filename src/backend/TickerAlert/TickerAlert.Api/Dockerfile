FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TickerAlert.Api/TickerAlert.Api.csproj", "TickerAlert.Api/"]
COPY ["TickerAlert.Application/TickerAlert.Application.csproj", "TickerAlert.Application/"]
COPY ["TickerAlert.Domain/TickerAlert.Domain.csproj", "TickerAlert.Domain/"]
COPY ["TickerAlert.Infrastructure/TickerAlert.Infrastructure.csproj", "TickerAlert.Infrastructure/"]
RUN dotnet restore "TickerAlert.Api/TickerAlert.Api.csproj"
COPY . .
WORKDIR "/src/TickerAlert.Api"
RUN dotnet build "TickerAlert.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TickerAlert.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TickerAlert.Api.dll"]
