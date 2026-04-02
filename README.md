# LibApp

Информационная система автоматизации управления библиотекой для повышения эффективности работы библиотечного персонала и улучшения качества обслуживания читателей.
---

## Возможности

- Регистрация, авторизация и смена пароля.
- CRUD по всем таблицам БД Библиотеки.
- Прием с штрафами и без книги.
- Бронирование книги.
- Выдача книги.
- Система состояний книги.
- Отчеты по книгам.

# Роли
```
admin        - admin
librarian1   - lib123
reader1      - reader123
reader2      - reader123
reader3      - reader123
```

# Демонстрация приложения

![Экран приложения 1](/demopng/reg.png)
![Экран приложения 2](/demopng/ctlg.png)
![Экран приложения 3](/demopng/edprof.png)
![Экран приложения 4](/demopng/prfl.png)
![Экран приложения 5](/demopng/accept.png)
![Экран приложения 6](/demopng/accept2.png)
![Экран приложения 7](/demopng/accept3.png)
![Экран приложения 8](/demopng/fines.png)
![Экран приложения 9](/demopng/reader.png)

## Команды 

# Контейнер с БД (Postgres)
```dockerfile
docker compose up -d
```

# Запуск сервера
```commandline
dotnet restore
dotnet build
dotnet run
```

