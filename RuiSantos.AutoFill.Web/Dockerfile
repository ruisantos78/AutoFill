FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RuiSantos.AutoFill.Web/RuiSantos.AutoFill.Web.csproj", "RuiSantos.AutoFill.Web/"]
COPY ["RuiSantos.AutoFill.Application/RuiSantos.AutoFill.Application.csproj", "RuiSantos.AutoFill.Application/"]
COPY ["RuiSantos.AutoFill.Domain/RuiSantos.AutoFill.Domain.csproj", "RuiSantos.AutoFill.Domain/"]
COPY ["RuiSantos.AutoFill.Infrastructure.Engines/RuiSantos.AutoFill.Infrastructure.Engines.csproj", "RuiSantos.AutoFill.Infrastructure.Engines/"]
COPY ["RuiSantos.AutoFill.Infrastructure.Repositories/RuiSantos.AutoFill.Infrastructure.Repositories.csproj", "RuiSantos.AutoFill.Infrastructure.Repositories/"]
COPY ["RuiSantos.AutoFill.Infrastructure.Engines.Gemini/RuiSantos.AutoFill.Infrastructure.Engines.Gemini.csproj", "RuiSantos.AutoFill.Infrastructure.Engines.Gemini/"]
COPY ["RuiSantos.AutoFill.Infrastructure.Repositories.Mongo/RuiSantos.AutoFill.Infrastructure.Repositories.Mongo.csproj", "RuiSantos.AutoFill.Infrastructure.Repositories.Mongo/"]
RUN dotnet restore "RuiSantos.AutoFill.Web/RuiSantos.AutoFill.Web.csproj"
COPY . .
WORKDIR "/src/RuiSantos.AutoFill.Web"
RUN dotnet build "RuiSantos.AutoFill.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "RuiSantos.AutoFill.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RuiSantos.AutoFill.Web.dll"]
