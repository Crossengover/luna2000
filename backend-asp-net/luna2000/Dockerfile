# Устанавливаем базовый образ с .NET SDK
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Устанавливаем SDK образ для сборки
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Копируем файл решения и восстанавливаем зависимости
COPY ["luna2000.csproj", "./"]
RUN dotnet restore "./luna2000.csproj"

# Копируем все файлы и собираем приложение
COPY . .
WORKDIR "/src/"
RUN dotnet build "luna2000.csproj" -c Release -o /app/build

# Публикуем приложение
FROM build AS publish
RUN dotnet publish "luna2000.csproj" -c Release -o /app/publish

# Финальный образ для выполнения
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "luna2000.dll"]
