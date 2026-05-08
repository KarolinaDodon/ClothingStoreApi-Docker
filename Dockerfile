FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src


COPY ["ClothingStoreApi.csproj", "./"]
RUN dotnet restore "./ClothingStoreApi.csproj"


COPY . .
WORKDIR "/src/."
RUN dotnet build "./ClothingStoreApi.csproj" -c $BUILD_CONFIGURATION -o /app/build


RUN dotnet publish "./ClothingStoreApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app


COPY --from=build /app/publish .


EXPOSE 8080


ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production


ENTRYPOINT ["dotnet", "ClothingStoreApi.dll"]