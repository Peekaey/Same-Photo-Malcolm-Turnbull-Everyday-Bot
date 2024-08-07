FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Malcolm-Turnbull-Bot.csproj", "./"]
RUN dotnet restore "Malcolm-Turnbull-Bot.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "Malcolm-Turnbull-Bot.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Malcolm-Turnbull-Bot.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Malcolm-Turnbull-Bot.dll"]
