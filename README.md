# CRM

Monorepo containing the CRM application.

## Structure

- [`Backend/`](Backend/) — .NET solution `TopSys.TopConWeb.sln` (API, Application, Domain, Infra, Tests).
- [`Frontend/`](Frontend/) — Angular 8 client.

## Getting started

### Backend (.NET)

```bash
cd Backend
dotnet restore TopSys.TopConWeb.sln
dotnet build TopSys.TopConWeb.sln
```

### Frontend (Angular)

```bash
cd Frontend
npm install
npm start
```
