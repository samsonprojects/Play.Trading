FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5006
ENV ASPNETCORE_URLS=http://+:5006

# Create a single non-root user with a specific UID and assign ownership of /app for security
RUN adduser --uid 5678 --disabled-password --gecos "" appuser && chown -R appuser /app && chmod -R 700 /app
# Switch to the non-root user
  
USER appuser

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
COPY ["src/Play.Trading.Service/Play.Trading.Service.csproj", "src/Play.Trading.Service/"]

# Use secrets for GitHub package authentication
RUN --mount=type=secret,id=GH_OWNER,dst=/GH_OWNER --mount=type=secret,id=GH_PAT,dst=/GH_PAT \
 dotnet nuget add source --username USERNAME --password `cat /GH_PAT` --store-password-in-clear-text --name github "https://nuget.pkg.github.com/`cat /GH_OWNER`/index.json"

RUN dotnet restore "src/Play.Trading.Service/Play.Trading.Service.csproj"
COPY ./src ./src
WORKDIR "/src/Play.Trading.Service"
ARG configuration=Release
RUN dotnet publish "Play.Trading.Service.csproj" -c $configuration --no-restore -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Set the entry point
ENTRYPOINT ["dotnet", "Play.Trading.Service.dll"]
