#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["GoodReading.Domain/GoodReading.Domain.csproj", "GoodReading.Domain/"]
COPY ["GoodReading.Application/GoodReading.Application.csproj", "GoodReading.Application/"]
COPY ["GoodReading.Persistence/GoodReading.Persistence.csproj", "GoodReading.Persistence/"]
COPY ["GoodReading.Infrastructure/GoodReading.Infrastructure.csproj", "GoodReading.Infrastructure/"]
COPY ["GoodReading.Web.Api/GoodReading.Web.Api.csproj", "GoodReading.Web.Api/"]
RUN dotnet restore "GoodReading.Web.Api/GoodReading.Web.Api.csproj"
COPY . .
WORKDIR "/src/GoodReading.Web.Api"
RUN dotnet build "GoodReading.Web.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GoodReading.Web.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GoodReading.Web.Api.dll"]