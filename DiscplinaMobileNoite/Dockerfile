# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files and restore dependencies
COPY ["DiscplinaMobileNoite/DisciplinaMobileNoite.csproj", "DisciplinaMobileNoite/"]
COPY ["DiscplinaMobileNoite.Application/DisciplinaMobileNoite.Application.csproj", "DisciplinaMobileNoite.Application/"]
COPY ["DiscplinaMobileNoite.Domain/DisciplinaMobileNoite.Domain.csproj", "DisciplinaMobileNoite.Domain/"]
COPY ["DiscplinaMobileNoite.Infrastructure/DisciplinaMobileNoite.Infrastructure.csproj", "DisciplinaMobileNoite.Infrastructure/"]
COPY ["DiscplinaMobileNoite.Shared/DisciplinaMobileNoite.Shared.csproj", "DisciplinaMobileNoite.Shared/"]

RUN dotnet restore "DisciplinaMobileNoite/DisciplinaMobileNoite.csproj"

# Copy full source and build
COPY . .
WORKDIR "/src/DsciplinaMobileNoite"
RUN dotnet build "DisciplinaMobileNoite.csproj" -c $BUILD_CONFIGURATION --no-restore -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "DiscplinaMobileNoite.csproj" -c $BUILD_CONFIGURATION --no-restore -o /app/publish /p:UseAppHost=false

# Final stage - runtime
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DiscplinaMobileNoite.dll"]
