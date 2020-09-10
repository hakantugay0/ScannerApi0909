#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Scanner.API/Scanner.API.csproj", "Scanner.API/"]
COPY ["Scanner.Data/Scanner.Data.csproj", "Scanner.Data/"]
COPY ["Scanner.Core/Scanner.Core.csproj", "Scanner.Core/"]
COPY ["Scanner.Helper/Scanner.Helper.csproj", "Scanner.Helper/"]
COPY ["Scanner.Resolver/Scanner.Resolver.csproj", "Scanner.Resolver/"]
COPY ["Scanner.Service/Scanner.Service.csproj", "Scanner.Service/"]
RUN dotnet restore "Scanner.API/Scanner.API.csproj"
COPY . .
WORKDIR "/src/Scanner.API"
RUN dotnet build "Scanner.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Scanner.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Scanner.API.dll"]