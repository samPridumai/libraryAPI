📚 Library Management API
Описание
REST API для управления корпоративной библиотекой. Поддерживает регистрацию пользователей, учёт книг, выдачу/возврат и работу с PDF-файлами книг.

🚀 Технологии
ASP.NET Core
PostgreSQL + Dapper
FluentValidation
Middleware (глобальная обработка ошибок)
Unit-тесты (xUnit + SQLite InMemory)

🔧 Функциональность
CRUD для книг и пользователей
Выдача/возврат книг
Загрузка и скачивание PDF книг
Фильтрация и сортировка книг
Валидация данных (FluentValidation)
Глобальная обработка исключений (Middleware)
Слои: Controller → Service → Repository (с DI)
