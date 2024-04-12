FROM mcr.microsoft.com/dotnet/sdk:8.0.204 AS build-env

WORKDIR /app

# Copy csproj and restore distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build project
COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0.4

WORKDIR /app
COPY --from=build-env /app/out .

ENTRYPOINT [ "dotnet", "ECommerceStore.Api.dll" ]


