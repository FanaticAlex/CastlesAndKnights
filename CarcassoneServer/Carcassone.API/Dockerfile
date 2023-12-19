#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 8080
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Carcassone.API/Carcassone.API.csproj", "Carcassone.API/"]
COPY ["Carcassone.Core/Carcassone.Core.csproj", "Carcassone.Core/"]
COPY ["Carcassone.DAL/Carcassone.DAL.csproj", "Carcassone.DAL/"]
RUN dotnet restore "Carcassone.API/Carcassone.API.csproj"
COPY . .
WORKDIR "/src/Carcassone.API"
RUN dotnet build "Carcassone.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Carcassone.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Carcassone.API.dll"]