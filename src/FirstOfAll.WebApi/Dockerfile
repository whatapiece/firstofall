FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY src/FirstOfAll.WebApi/FirstOfAll.WebApi.csproj src/FirstOfAll.WebApi/
COPY src/FirstOfAll.Infra.CrossCutting.IoC/FirstOfAll.Infra.CrossCutting.IoC.csproj src/FirstOfAll.Infra.CrossCutting.IoC/
COPY src/FirstOfAll.Domain/FirstOfAll.Domain.csproj src/FirstOfAll.Domain/
COPY src/FirstOfAll.Application/FirstOfAll.Application.csproj src/FirstOfAll.Application/
COPY src/FirstOfAll.Infra.Data/FirstOfAll.Infra.Data.csproj src/FirstOfAll.Infra.Data/
COPY src/FirstOfAll.Infra.CrossCutting.Identity/FirstOfAll.Infra.CrossCutting.Identity.csproj src/FirstOfAll.Infra.CrossCutting.Identity/
RUN dotnet restore src/FirstOfAll.WebApi/FirstOfAll.WebApi.csproj
COPY . .
WORKDIR /src/src/FirstOfAll.WebApi
RUN dotnet build FirstOfAll.WebApi.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish FirstOfAll.WebApi.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "FirstOfAll.WebApi.dll"]
