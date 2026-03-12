# Projects CRM
## ➡️ [На русском](./README.ru.md)
## Overview
### Features:
- Create, delete, edit, view projects
- Create, delete, edit, view employees
- Assign or remove employees from projects
- Project creation wizard
- N-layer architecture
- Basic unit and integration testing

- DB is seeded if empty
- Swagger available in the development environment for API documentation
### Tech Stack
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

## Getting Started
### Prerequisites
#### Containerized
- [Docker](https://www.docker.com/get-started/) 
- [Docker Compose](https://docs.docker.com/compose/install)
#### Locally
- [.NET SDK 10.0](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
- [Node](https://nodejs.org/en/download)
- [PostgreSQL](https://www.postgresql.org/download/) 
### Running in Container
1. Clone repository
2. Create a `.env` file in the project root and configure: `DB_USER`, `DB_PASSWORD`, `DB_NAME`
3. Run `docker compose up --build`

***NOTE*** by default backend ports are not exposed outside the Docker network
### Running Locally
***NOTE*** that running locally is intended for development purposes
#### Backend
1. Navigate to `/backend/src/Web`
2. Create a file `appsettings.Development.json` (or rename `appsettings.json.example`)
3. Configure `ConnectionStrings` with your local postgres credentials
4. Run following commands in `/backend/src/Web` 
```
dotnet dev-certs https --trust
dotnet restore
dotnet run
```
By default the backend listens on: 
- HTTPS: `7224`,  `5195`
#### Frontend
Run following commands in `/frontend`:
```
npm install
npm run dev
```
By default frontend listens on `63232`

