FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Same-Photo-Malcolm-Turnbull-Everyday-Discord-Bot.csproj", "src/"]
RUN dotnet restore "src/Same-Photo-Malcolm-Turnbull-Everyday-Discord-Bot.csproj"
COPY . .
WORKDIR "/src/src"
RUN dotnet build "Same-Photo-Malcolm-Turnbull-Everyday-Discord-Bot.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Same-Photo-Malcolm-Turnbull-Everyday-Discord-Bot.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY src/Images/MalcolmTurnbull.jpg ./src/Images/MalcolmTurnbull.jpg
ENTRYPOINT ["dotnet", "Same-Photo-Malcolm-Turnbull-Everyday-Discord-Bot.dll"]
