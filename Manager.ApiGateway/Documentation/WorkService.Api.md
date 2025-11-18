## Work API

**Description**: Управление задачами пользователя

### 1) CreateWork

- **Method:** POST
- **URL:** /api/recipients/{recipientId}/works
- **Description:** Создаёт новую задачу для пользователя
- **Request Body:**

```json
{
    "Title": "string",
    "Description": "string?",
    "DeadLineUtc": "DateTime?",
    "ReminderIntervals": "TimeSpan[]?"
}
```

- **Responses**

    - **201 Created**

### 2) GetWork

- **Method:** GET
- **URL:** /api/recipients/{recipientId}/works/{workId}
- **Description:** Получает задачу по айдишнику

- **Request Body:**

```json
{
    "Title": "string",
    "Description": "string?",
    "WorkStatus": "string",
    "DeadLineUtc": "DateTime?",
    "ReminderIntervals": "TimeSpan[]?"
}
```

### 3) PatchWork

- **Method:** PATCH
- **URL:** /api/recipients/{recipientId}/works/{workId}
- **Description:** Изменяет задачу

- **Request Body:**

```json
{
    "Title": "string",
    "Description": "string?",
    "DeadLineUtc": "DateTime?",
    "ReminderIntervals": "TimeSpan[]?"
}
```

- **Responses**
    - **200 OK**
    - **404 Not Found**

### 4) DeleteWork

- **Method:** DELETE
- **URL:** /api/recipients/{recipientId}/works/{workId}
- **Description:** Удаляет задачу

- **Responses**
    - **200 OK**
    - **404 Not Found**

### 5) GetWorks

- **Method:** GET
- **URL:** /api/recipients/{recipientId}/works
- **Description:** Получает все задачи пользователя

- **Response Body:**

```json
{
    "Title": "string",
    "Description": "string?",
    "WorkStatus": "string",
    "DeadLineUtc": "DateTime?",
    "ReminderIntervals": "TimeSpan[]?"
}
```

### 6) GetActualWorks

- **Method:** GET
- **URL:** /api/recipients/{recipientId}/works/actual
- **Description:** Получает все актуальные задачи пользователя

- **Response Body:**

```json
[
    {
        "Title": "string",
        "Description": "string?",
        "WorkStatus": "string",
        "DeadLineUtc": "DateTime?",
        "ReminderIntervals": "TimeSpan[]?"
    }
]
```

### 7) GetWorksForReminder

- **Method:** GET
- **URL:** /api/recipients/{recipientId}/works/ready-for-reminder
- **Description:** Получает все задачи, про которые нужно напомнить пользователю

- **Response Body:**

```json
[
    {
        "Title": "string",
        "Description": "string?",
        "WorkStatus": "string",
        "DeadLineUtc": "DateTime?",
        "ReminderIntervals": "TimeSpan[]?"
    }
]
```

### 8) GetExpiredWorks

- **Method:** GET
- **URL:** /api/recipients/{recipientId}/works/expired
- **Description:** Получает все задачи, про которые нужно напомнить пользователю

- **Response Body:**

```json
[
    {
        "Title": "string",
        "Description": "string?",
        "WorkStatus": "string",
        "DeadLineUtc": "DateTime?",
        "ReminderIntervals": "TimeSpan[]?"
    }
]
```

### 8) GetDeletedWorks

- **Method:** GET
- **URL:** /api/recipients/{recipientId}/works/deleted
- **Description:** Получает все удаленные задачи (с пометкой "deleted") пользователя

- **Response Body:**

```json
[
    {
        "Title": "string",
        "Description": "string?",
        "WorkStatus": "string",
        "DeadLineUtc": "DateTime?",
        "ReminderIntervals": "TimeSpan[]?"
    }
]
```

### 9) GetExpiredWorks

- **Method:** GET
- **URL:** /api/recipients/{recipientId}/works/expired
- **Description:** Получает все истекшие по дедлайну задачи пользователя
- **Response Body:**

```json
[
    {
        "Title": "string",
        "Description": "string?",
        "WorkStatus": "string",
        "DeadLineUtc": "DateTime?",
        "ReminderIntervals": "TimeSpan[]?"
    }
]
```

### 10) GetCompletedWorks

- **Method:** GET
- **URL:** /api/recipients/{recipientId}/works/completed
- **Description:** Получает все выполненные задачи пользователя

- **Response Body:**

```json
[
    {
        "Title": "string",
        "Description": "string?",
        "WorkStatus": "string",
        "DeadLineUtc": "DateTime?",
        "ReminderIntervals": "TimeSpan[]?"
    }
]
```