#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["FoodInfoAPI/FoodInfoAPI.csproj", "FoodInfoAPI/"]
RUN dotnet restore "FoodInfoAPI/FoodInfoAPI.csproj"
COPY . .
WORKDIR "/src/FoodInfoAPI"
RUN dotnet build "FoodInfoAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FoodInfoAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FoodInfoAPI.dll"]