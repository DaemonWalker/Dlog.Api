#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 as base
MAINTAINER Daemon Walker
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 as build
WORKDIR /src
COPY ["Dlog.Api/Dlog.Api.csproj", "Dlog.Api/"]
RUN dotnet restore "Dlog.Api/Dlog.Api.csproj"
COPY . .
WORKDIR "/src/Dlog.Api"
RUN dotnet build "Dlog.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dlog.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dlog.Api.dll"]