## Timer API

**Description**: Управление таймерами пользователей.

### 1) StartTimer

- **Method:** POST
- **URL:** /api/recipients/{recipientId}/timers
- **Description:** Создаёт новый таймер для пользователя или стартует существующий
- **Request Body:**

```json
{
    "Name": "string",
    "StartTime": "DateTime",
    "PingTimeout": "TimeSpan?"
}
```

**Fields description**

- **Name:** Название таймера
- **StartTime:** Время запуска таймера
- **PingTimeout:** Через сколько напомнить о таймере(опционально)


- **Responses**

    - **200 OK**

    - **400 Bad Request**

### 2) SelectUserTimers

- **Method:** GET
- **URL:** /api/recipients/{recipientId}/timers
- **Description:** Возвращает все таймеры для конкретного пользователя
- **Query Parameters:**
    - WithArchived: "bool",
    - WithDeleted: "bool"
- **Response Body:**

```json
[
    {
        "TimerId": "Guid",
        "Name": "String",
        "StartTime": "DateTime?",
        "ElapsedTime": "TimeSpan",
        "StopTime": "DateTime?",
        "PingTimeout": "TimeSpan?",
        "TimerStatus": "string",
        "TimerSessions": [
            {
                "StartTime": "DateTime",
                "StopTime": "DateTime?",
                "IsOver": "bool"
            }
        ]
    }
]
```

**Fields description**

- **TimerId:** Уникальный Id таймера
- **Name:** Название таймера
- **StartTime:** Время запуска таймера (опционально)
- **ElapsedTime:** Пройденное время после запуска таймера. 0 если таймер не запущен
- **StopTime:** Время остановки таймера(опционально)
- **PingTimeout:** Через сколько напомнить о таймере(опционально):
- **TimerSessions** Сессии таймера. Запустил таймер, затем остановил его - 1 сессия
- **TimerStatus:** Текущее состояние таймера. Принимает следующие значения:
    - **Created:** Созданный, но не запущенный таймер. Базовый статус.
    - **Started** Запущенный таймер
    - **Stopped** Остановленный таймер
    - **Reset** Сброшенный таймер
    - **Archived** Архивный таймер
    - **Deleted** Удаленный таймер

- **Responses**

    - **200 OK**

    - **401 Bad Request**

### 3) StopTimer

- **Method:** PATCH
- **URL:** /api/recipients/{recipientId}/timers/{timerName}/stop
- **Description:** Останавливает сессию таймера и переводит таймер в статус остановлен
- **Request Body:**

```json
{
    "StopTime": "DateTime"
}
```

**Fields description**

- **StopTime:** Время остановки таймера

- **Responses**

    - **200 OK**

    - **404 Not Found**

### 4) FindTimer

- **Method:** PATCH
- **URL:** /api/recipients/{recipientId}/timers/{timerName}
- **Description:** Ищет таймер по его уникальному имени

- **Response Body:**

```json
{
    "TimerId": "Guid",
    "StartTime": "DateTime?",
    "ElapsedTime": "TimeSpan",
    "StopTime": "DateTime?",
    "PingTimeout": "TimeSpan?",
    
    "TimerSessions": [
        {
            "SessionId": "Guid",
            "StartTime": "DateTime",
            "StopTime": "DateTime?",
            "IsOver": "bool"
        }
    ],
    "TimerStatus": "string"
} 
```

**Fields description**

- **SessionId:** Уникальный Id сессии
- **TimerId:** Уникальный Id таймера
- **StartTime:** Время запуска таймера (опционально)
- **ElapsedTime:** Пройденное время после запуска таймера. 0 если таймер не запущен
- **StopTime:** Время остановки таймера(опционально)
- **PingTimeout:** Через сколько напомнить о таймере(опционально):
- **TimerSessions** Сессии таймера. Запустил таймер, затем остановил его - 1 сессия
- **TimerStatus:** Текущее состояние таймера. Принимает следующие значения:
    - **Created:** Созданный, но не запущенный таймер. Базовый статус.
    - **Started** Запущенный таймер
    - **Stopped** Остановленный таймер
    - **Reset** Сброшенный таймер
    - **Archived** Архивный таймер
    - **Deleted** Удаленный таймер
- **Responses**

    - **200 OK**

    - **401 BadRequest**

### 5) ResetTimer

- **Method:** PATCH
- **URL:** /api/recipients/{recipientId}/timers/{timerName}/reset
- **Description: Сбрасывает время таймера и архивирует его, добавляет к имени таймера archived**

- **Responses**

    - **200 OK**
    - **400 Bad Request**
    - **404 Not Found**

### 6) DeleteTimer

- **Method:** DELETE
- **URL:** /api/recipients/{recipientId}/timers/{timerName}/delete
- **Description:** Добавляет к имени таймера deleted и переводит статус в Deleted

- **Responses**

    - **200 OK**
    - **400 Bad Request**
    - **404 Not Found**