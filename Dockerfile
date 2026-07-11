
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY *.sln .
COPY API/*.csproj API/
COPY Core/*.csproj Core/
COPY Infrastructure/*.csproj Infrastructure/
COPY Domain/*.csproj Domain/
COPY tests/ECommerce.UnitTests/*.csproj tests/ECommerce.UnitTests/
RUN dotnet restore E-Commerce.sln

COPY . .

RUN dotnet publish "API/API.csproj" -c Release -o publish

RUN dotnet tool install --global dotnet-ef

ENV PATH=${PATH}:/root/.dotnet/tools

RUN dotnet ef migrations bundle \
    --project Infrastructure \
    --startup-project API \
    --configuration Release \
    --output /app/efbundle

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
COPY --from=build /app/efbundle .

RUN chmod +x /app/efbundle
ENTRYPOINT ["dotnet", "API.dll"]