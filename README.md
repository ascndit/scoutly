# Scoutly

Веб-API для платформы скаутинга специалистов: компании, вакансии, профили кандидатов, отклики и работа рекрутера.

## Стек

- .NET 9, ASP.NET Core Web API
- Entity Framework Core 9 и Npgsql (PostgreSQL)
- Расширения PostgreSQL: `pg_trgm`, `pgcrypto`
- OpenAPI

## Модель данных

Контекст `ScoutingDbContext` описывает схему в `scoutly/Models`.

| Сущность | Назначение |
| --- | --- |
| User, Role, Permission | Учётные записи и права |
| RefreshToken | Токены обновления сессии |
| Company | Компании работодателей |
| Vacancy, VacancySkill | Вакансии и требуемые навыки |
| SpecialistProfile | Профиль специалиста |
| SpecialistSkill, Skill, SkillCategory | Навыки специалиста |
| Education, WorkExperience | Образование и опыт |
| Application | Отклик на вакансию |
| RecruiterCandidate, InteractionHistory | Кандидаты в работе у рекрутера |
| Notification | Уведомления пользователя |
| ErrorLog, SystemSetting | Журнал ошибок и настройки |

Поиск по заголовкам вакансий и профилей использует триграммные индексы (`gin_trgm_ops`).

## Запуск

Нужны [.NET 9 SDK](https://dotnet.microsoft.com/download) и PostgreSQL с уже созданной схемой.

```bash
dotnet restore
dotnet build
dotnet run --project scoutly
```

Профили запуска: HTTP `http://localhost:5202`, HTTPS `https://localhost:7141`.

## Текущее состояние

Сборка решения проходит без ошибок и предупреждений. Модели и `ScoutingDbContext` готовы. Контекст ещё не подключён в `Program.cs`, строка подключения к PostgreSQL не задана, а единственный контроллер — шаблонный `WeatherForecast`.
