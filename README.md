# Scoutly

Учебный API вакансий.

## Стек

- .NET 9, ASP.NET Core
- Entity Framework Core и PostgreSQL
- DTO

## Что есть

Модель компаний, вакансий, навыков и профилей специалистов. Реализован CRUD вакансий и DTO: `GET`, `POST`, `PUT` и `DELETE` для `/api/vacancies`.

## Запуск

Нужны .NET 9 SDK и PostgreSQL. Приложение читает строку подключения из `ConnectionStrings:ApplicationDbContext`; для локальной разработки используется `appsettings.Development.json`, а в других окружениях задайте переменную `ConnectionStrings__ApplicationDbContext`.

Примените миграции к базе:

```bash
dotnet ef database update --project scoutly
```

Затем запустите API:

```bash
dotnet run --project scoutly
```

HTTP: `http://localhost:5202`. Swagger доступен в Development: `/swagger`.
