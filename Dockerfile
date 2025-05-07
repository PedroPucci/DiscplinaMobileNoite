FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["DiscplinaMobileNoite/DiscplinaMobileNoite.csproj", "DiscplinaMobileNoite/"]
COPY ["DiscplinaMobileNoite.Application/DiscplinaMobileNoite.Application.csproj", "DiscplinaMobileNoite.Application/"]
COPY ["DiscplinaMobileNoite.Domain/DiscplinaMobileNoite.Domain.csproj", "DiscplinaMobileNoite.Domain/"]
COPY ["DiscplinaMobileNoite.Infrastructure/DiscplinaMobileNoite.Infrastructure.csproj", "DiscplinaMobileNoite.Infrastructure/"]
COPY ["DiscplinaMobileNoite.Shared/DiscplinaMobileNoite.Shared.csproj", "DiscplinaMobileNoite.Shared/"]

RUN dotnet restore "DiscplinaMobileNoite/DiscplinaMobileNoite.csproj"
COPY . .
WORKDIR "/src/DiscplinaMobileNoite"
RUN dotnet build "DiscplinaMobileNoite.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "DiscplinaMobileNoite.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DiscplinaMobileNoite.dll"]
