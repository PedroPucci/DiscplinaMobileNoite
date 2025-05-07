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
COPY ["DisciplinaMobileNoite/DisciplinaMobileNoite.csproj", "DisciplinaMobileNoite/"]
COPY ["DisciplinaMobileNoite.Application/DisciplinaMobileNoite.Application.csproj", "DisciplinaMobileNoite.Application/"]
COPY ["DisciplinaMobileNoite.Domain/DisciplinaMobileNoite.Domain.csproj", "DisciplinaMobileNoite.Domain/"]
COPY ["DisciplinaMobileNoite.Infrastructure/DisciplinaMobileNoite.Infrastructure.csproj", "DisciplinaMobileNoite.Infrastructure/"]
COPY ["DisciplinaMobileNoite.Shared/DisciplinaMobileNoite.Shared.csproj", "DisciplinaMobileNoite.Shared/"]

RUN dotnet restore "DisciplinaMobileNoite/DisciplinaMobileNoite.csproj"

# Copy full source and build
COPY . .
WORKDIR "/src/DisciplinaMobileNoite"
RUN dotnet build "DisciplinaMobileNoite.csproj" -c $BUILD_CONFIGURATION --no-restore -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "DisciplinaMobileNoite.csproj" -c $BUILD_CONFIGURATION --no-restore -o /app/publish /p:UseAppHost=false

# Final stage - runtime
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DisciplinaMobileNoite.dll"]
