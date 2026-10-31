# Scoutly

Учебный API вакансий.

## Стек

- .NET 9, ASP.NET Core
- Entity Framework Core и PostgreSQL
- DTO

## Что есть

Модель компаний, вакансий, навыков и профилей специалистов. Реализован CRUD вакансий и DTO: `GET`, `POST`, `PUT` и `DELETE` для `/api/vacancies`.

## Запуск

Нужны .NET 9 SDK и PostgreSQL. Строка подключения задаётся локально в `ConnectionStrings:DefaultConnection`, в репозитории её нет.

```bash
dotnet run --project scoutly
```

HTTP: `http://localhost:5202`. Swagger: `/swagger`.
