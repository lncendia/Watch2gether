FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 10001
EXPOSE 10000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

RUN curl -sL https://deb.nodesource.com/setup_20.x | bash -
RUN apt-get install -y nodejs

COPY ["PJMS.AuthService/PJMS.AuthService.csproj", "PJMS.AuthService/"]
COPY ["PJMS.AuthService.Data.IdentityServer/PJMS.AuthService.Data.IdentityServer.csproj", "PJMS.AuthService.Data.IdentityServer/"]
COPY ["PJMS.AuthService.Abstractions/PJMS.AuthService.Abstractions.csproj", "PJMS.AuthService.Abstractions/"]
COPY ["PJMS.AuthService.Data/PJMS.AuthService.Data.csproj", "PJMS.AuthService.Data/"]
COPY ["PJMS.AuthService.Mail/PJMS.AuthService.Mail.csproj", "PJMS.AuthService.Mail/"]
COPY ["PJMS.Logging/PJMS.Logging.csproj", "PJMS.Logging/"]
COPY ["PJMS.AuthService.Services/PJMS.AuthService.Services.csproj", "PJMS.AuthService.Services/"]
COPY ["PJMS.AuthService.Web/PJMS.AuthService.Web.csproj", "PJMS.AuthService.Web/"]
COPY ["PJMS.AuthService.VkId/PJMS.AuthService.VkId.csproj", "PJMS.AuthService.VkId/"]
RUN dotnet restore "PJMS.AuthService/PJMS.AuthService.csproj"
COPY . .

WORKDIR /src/PJMS.AuthService/AuthService
RUN npm install
RUN npm run build-prod

WORKDIR /src/PJMS.AuthService
RUN dotnet build "PJMS.AuthService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PJMS.AuthService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PJMS.AuthService.dll"]