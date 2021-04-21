#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /app
COPY . .
RUN dotnet restore "Common/Common.csproj"
RUN dotnet build "Common/Common.csproj" -c Release -o /app/build
RUN dotnet restore "PetMotel/PetMotelWeb.csproj"
RUN dotnet build "PetMotel/PetMotelWeb.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PetMotel/PetMotelWeb.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PetMotelWeb.dll"]