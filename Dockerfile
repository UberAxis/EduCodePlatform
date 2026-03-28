FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["EduCodePlatform.sln", "./"]
COPY ["EduCodePlatform.WebApi/EduCodePlatform.WebApi.csproj", "EduCodePlatform.WebApi/"]
COPY ["EduCodePlatform.Application/EduCodePlatform.Application.csproj", "EduCodePlatform.Application/"]
COPY ["EduCodePlatform.Domain/EduCodePlatform.Domain.csproj", "EduCodePlatform.Domain/"]
COPY ["EduCodePlatform.Infrastructure/EduCodePlatform.Infrastructure.csproj", "EduCodePlatform.Infrastructure/"]

RUN dotnet restore "EduCodePlatform.sln"

COPY . .

WORKDIR "/src/EduCodePlatform.WebApi"
RUN dotnet build "EduCodePlatform.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "EduCodePlatform.WebApi.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "EduCodePlatform.WebApi.dll"]