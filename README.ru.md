# Projects CRM
## ➡️ [English version](./README.md)
## Обзор
### Возможности:
- Создание, удаление, редактирование и просмотр проектов 
- Создание, удаление, редактирование и просмотр сотрудников
- Назначение или снятие сотрудников с проекта
- Визард для создания проектов
- N-layer архитектура
- Базовое покрытие юнит и интеграционными тестами
- БД наполняется тестовыми данными если пустая
- Swagger доступен в development environment для автоматической документации
### Стек
#### Backend
- **.NET 10**
- **ASP.NET Core**
- **EF Core**
- **PostgreSQL**
#### Frontend
- **Node.js**
- **Vue 3**
- **Vue Router**
- **Axios**

## Запуск
### Зависимости для запуска
#### Для работы в контейнере
- [Docker](https://www.docker.com/get-started/)
- [Docker Compose](https://docs.docker.com/compose/install)
#### Для локального запуска
- [.NET SDK 10.0](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
- [Node](https://nodejs.org/en/download)
- [PostgreSQL](https://www.postgresql.org/download/)
### Запуск в контейнере
1. Склонируйте репозиторий 
2. Создайте `.env` файл в корне проекта и настройте следующие переменные: `DB_USER`, `DB_PASSWORD`, `DB_NAME`
3. Выполните команду `docker compose up --build`

***Обратите внимание*** по умолчанию порты бэкенда недоступны вне внутренней сети Docker 
### Локальный запуск
***Обратите внимание*** данный способ запуска предназначен в основном для дебага
#### Backend
1. Перейдите в `/backend/src/Web`
2. Создайте файл `appsettings.Development.json` (или переименуйте `appsettings.json.example`)
3. Заполните `ConnectionStrings` вашими данными postgres
4. Запустите следующие команды в  `/backend/src/Web`
```
dotnet dev-certs https --trust
dotnet restore
dotnet run
```
По умолчанию бэкенд доступен на следующих портах:
- HTTPS: `7224`,  `5195`
#### Frontend
Запустите следующие команды в `/frontend`:
```
npm install
npm run dev
```
По умолчанию фронтенд доступен на порту `63232`

