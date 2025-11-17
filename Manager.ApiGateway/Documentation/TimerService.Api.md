## Timer API
**Description**: Управление таймерами пользователей.

### 1) StartTimer
- **Method:** POST
- **URL:** /api/recipient/{recipientId}/timers
- **Description:** Создаёт новый таймер для пользователя или стартует существующий
- **Request Body:**
```json
{
  "Name": "string",
  "StartTime": "DateTime",
  "PingTimeout": "TimeSpan?"
}
```
- **Responses**

    - **200 OK**

    - **400 Bad Request**
### 2) SelectUserTimers
- **Method:** GET
- **URL:** /api/recipient/{recipientId}/timers
- **Description:** Получает все таймеры для конкретного пользователя
- **Query Parameters:**
  - WithArchived: "bool",
  - WithDeleted: "bool"
- **Response Body:**
```json
[
    {
        "Name": "String",
        "StartTime": "DateTime?",
        "ElapsedTime": "TimeSpan",
        "StopTime": "DateTime?",
        "PingTimeout": "TimeSpan?",
        "TimerStatus": "string"
    }
]
```
### 3) StopTimer
- **Method:** PATCH
- **URL:** /api/recipient/{recipientId}/timers/{name}/stop
- **Description:** Останавливает сессию таймера и переводит таймер в статус остановлен
- **Request Body:**
```json
{
  "StopTime": "DateTime"
}
```
- **Responses**

    - **200 OK**

    - **404 Not Found**

### 4) FindTimer
- **Method:** PATCH
- **URL:** /api/recipient/{recipientId}/timers/{name}
- **Description:** Ищет таймер по его уникальному индексу

- **Response Body:**
```json
{
    "Name": "String",
    "StartTime": "DateTime?",
    "ElapsedTime": "TimeSpan",
    "StopTime": "DateTime?",
    "PingTimeout": "TimeSpan?",
    "TimerStatus": "string"
}

```
### 5) ResetTimer
- **Method:** PATCH
- **URL:** /api/recipient/{recipientId}/timers/{name}/reset
- **Description: Сбрасывает время таймера и архивирует его, добавляет к имени таймера archived**

- **Responses**

    - **200 OK**
    - **400 Bad Request**
    - **404 Not Found**

### 6) DeleteTimer
- **Method:** DELETE
- **URL:** /api/recipient/{recipientId}/timers/{name}/delete
- **Description:** Добавляет к имени таймера Deleted и переводит статус в Deleted

- **Responses**

    - **200 OK**
    - **400 Bad Request**
    - **404 Not Found**