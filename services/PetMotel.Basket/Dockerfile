FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /app
COPY . .
RUN dotnet restore "PetMotel.Common/PetMotel.Common.csproj"
RUN dotnet build "PetMotel.Common/PetMotel.Common.csproj" -c Release -o /app/build
RUN dotnet restore "services/PetMotel.Basket/PetMotel.Basket.csproj"
RUN dotnet build "services/PetMotel.Basket/PetMotel.Basket.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "services/PetMotel.Basket/PetMotel.Basket.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PetMotel.Basket.dll"]