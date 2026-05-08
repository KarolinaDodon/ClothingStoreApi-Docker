# === ЭТАП 1: СБОРКА (Используем полный SDK) ===
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Копируем только файл проекта (для кэширования зависимостей)
COPY ["ClothingStoreApi.csproj", "./"]
RUN dotnet restore "./ClothingStoreApi.csproj"

# Копируем весь исходный код и собираем
COPY . .
WORKDIR "/src/."
RUN dotnet build "./ClothingStoreApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Публикуем приложение (получаем готовые файлы для запуска)
RUN dotnet publish "./ClothingStoreApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# === ЭТАП 2: ЗАПУСК (Используем легкую среду) ===
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Копируем готовые файлы из этапа сборки
COPY --from=build /app/publish .

# Открываем порт 8080 для доступа извне
EXPOSE 8080

# Указываем, что приложение должно слушать этот порт
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Команда запуска
ENTRYPOINT ["dotnet", "ClothingStoreApi.dll"]