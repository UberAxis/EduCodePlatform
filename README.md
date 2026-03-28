# EduCodePlatform

EduCodePlatform — это веб-платформа для управления обучением, предназначенная для создания, доставки и отслеживания образовательного контента. Она включает модули, уроки, задания, тесты, управление пользователями и систему достижений.

## [Презентация для защиты](https://disk.yandex.ru/i/Crsp-mgDT1ulVg)
## [Скринкаст, демонстрирующий работу прототипа](https://disk.yandex.ru/i/gfBnvld60D2lNg)

## Технологический стек

*   **Frontend:** Nuxt.js (Vue.js, TypeScript), Nuxt UI, Tailwind CSS, Pinia
*   **Backend:** .NET Core (C#), Entity Framework Core, ASP.NET Core Web API
*   **База данных:** Реляционная база данных PostgreSQL
*   **Контейнеризация:** Docker, Docker Compose
*   **Аутентификация:** JWT (JSON Web Tokens)

## Предварительные требования

Перед началом убедитесь, что у вас установлено следующее программное обеспечение:

1.  **Docker:** [Загрузить Docker](https://docs.docker.com/get-docker/)
2.  **Docker Compose:** [Загрузить Docker Compose](https://docs.docker.com/compose/install/)

## Настройка и установка

Этот проект использует Docker Compose для упрощения процесса установки и развертывания. Команда `docker-compose up --build` соберет необходимые Docker-образы и запустит все службы (frontend, backend, база данных).

### Шаги:

1.  **Клонируйте репозиторий:**
    Клонируйте репозиторий EduCodePlatform на вашу локальную машину:
    ```bash
    git clone https://github.com/UberAxis/EduCodePlatform
    cd EduCodePlatform
    ```

2.  **Соберите и запустите сервисы:**
    Перейдите в корневую директорию клонированного репозитория (где находится файл `docker-compose.yml`) и выполните следующую команду:
    ```bash
    docker-compose up --build
    ```
    Эта команда выполнит следующие действия:
    *   **Build:** Создаст Docker-образы для frontend и backend приложений, если они еще не существуют или если были обнаружены изменения.
    *   **Start:** Запустит контейнеры для frontend, backend и базы данных.

3.  **Настройка базы данных:**
    Настройка базы данных управляется через Docker Compose. Конкретный тип базы данных и детали подключения (такие как имя пользователя, пароль, имя базы данных) определены в файле `docker-compose.yml`. Миграции Entity Framework Core будут автоматически применены для настройки схемы базы данных при запуске сервиса backend.

## Доступ к приложению

После успешного завершения команды `docker-compose up --build` приложение должно быть доступно по следующим адресам:

*   **Frontend (Клиент):** `http://localhost:3000` (Это порт разработки Nuxt.js по умолчанию. Он может отличаться, если указан в `docker-compose.yml` или `nuxt.config.ts` для production-сборки.)
*   **Backend API:** `http://localhost:8888` (Это порт по умолчанию, указанный в `nuxt.config.ts` для `runtimeConfig.public.apiBase`. Реальный порт backend внутри контейнера может отличаться и быть проброшен через `docker-compose.yml`.)

**Примечание:** Если `docker-compose.yml` использует другие сопоставления портов или имена сервисов, скорректируйте URL соответствующим образом.
