#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime-deps:5.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
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
RUN dotnet publish "GoodReading.Web.Api.csproj" --runtime alpine-x64 --self-contained true -c Release -o /app/publish /p:PublishTrimmed=true

FROM base AS final
RUN apk update && apk add --no-cache openssl
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["./GoodReading.Web.Api"]