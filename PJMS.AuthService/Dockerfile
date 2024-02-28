FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PJMS.AuthService/PJMS.AuthService.csproj", "PJMS.AuthService/"]
COPY ["PJMS.AuthService.Data.IdentityServer/PJMS.AuthService.Data.IdentityServer.csproj", "PJMS.AuthService.Data.IdentityServer/"]
COPY ["PJMS.AuthService.Abstractions/PJMS.AuthService.Abstractions.csproj", "PJMS.AuthService.Abstractions/"]
COPY ["PJMS.AuthService.Data/PJMS.AuthService.Data.csproj", "PJMS.AuthService.Data/"]
COPY ["PJMS.AuthService.Services/PJMS.AuthService.Services.csproj", "PJMS.AuthService.Services/"]
COPY ["PJMS.AuthService.Web/PJMS.AuthService.Web.csproj", "PJMS.AuthService.Web/"]
COPY ["PJMS.AuthService.VkId/PJMS.AuthService.VkId.csproj", "PJMS.AuthService.VkId/"]
COPY ["PJMS.Logging/PJMS.Logging.csproj", "PJMS.Logging/"]
COPY ["PJMS.AuthService.Mail/PJMS.AuthService.Mail.csproj", "PJMS.AuthService.Mail/"]
RUN dotnet restore "PJMS.AuthService/PJMS.AuthService.csproj"
COPY . .

FROM node:21-alpine as npm-build
WORKDIR /src/PJMS.AuthService/AuthService
COPY --from=build /src/PJMS.AuthService/AuthService/package*.json ./
RUN npm install
COPY --from=build /src/PJMS.AuthService/AuthService/. ./
RUN npm run build-prod

FROM build AS publish
WORKDIR "/src/PJMS.AuthService"
COPY --from=npm-build /src/PJMS.AuthService/wwwroot ./wwwroot
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PJMS.AuthService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PJMS.AuthService.dll"]
