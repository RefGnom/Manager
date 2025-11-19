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

**Fields description**

- **Title:** Название задачи
- **Description:** Описание задачи(опционально)
- **DeadLineUtc:** Крайний срок задачи(опционально)
- **ReminderIntervals:** Интервалы напоминания о задаче(опционально)


- **Responses**

    - **201 Created**

### 2) GetWork

- **Method:** GET
- **URL:** /api/recipients/{recipientId}/works/{workId}
- **Description:** Возвращает задачу по айдишнику

- **Request Body:**

```json
{
    "WorkId": "Id",
    "Title": "string",
    "Description": "string?",
    "WorkStatus": "string",
    "DeadLineUtc": "DateTime?",
    "ReminderIntervals": "TimeSpan[]?"
}
```

**Fields description**

- **WorkId:** Идентификатор пользователя
- **Title:** Название задачи
- **Description:** Описание задачи(опционально)
- **WorkStatus:** статус задачи. Принимает следующие значения:
    - **Unknown:** Неизвестный статус
    - **Actual:** Активная задача
    - **Completed:** Выполненная задача
    - **Expired:** Просроченная задача
    - **Deleted:** Удаленная задача
- **DeadLineUtc:** Крайний срок задачи(опционально)
- **ReminderIntervals:** Интервалы напоминания о задаче(опционально)

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

**Fields description**

- **Title:** Название задачи
- **Description:** Описание задачи(опционально)
- **DeadLineUtc:** Крайний срок задачи(опционально)
- **ReminderIntervals:** Интервалы напоминания о задаче(опционально)


- **Responses**
    - **200 OK**
    - **404 Not Found**

### 4) DeleteWork

- **Method:** DELETE
- **URL:** /api/recipients/{recipientId}/works/{workId}
- **Description:** Добавляет к имени задачи Deleted и переводит статус в Deleted

- **Responses**
    - **200 OK**
    - **404 Not Found**

### 5) GetWorks

- **Method:** GET
- **URL:** /api/recipients/{recipientId}/works
- **Description:** Возвращает все задачи пользователя

- **Response Body:**

```json
[
    {
        "WorkId": "Guid",
        "Title": "string",
        "Description": "string?",
        "WorkStatus": "string",
        "DeadLineUtc": "DateTime?",
        "ReminderIntervals": "TimeSpan[]?"
    }
]
```

**Fields description**

- **WorkId:** Уникальный Идентификатор задачи
- **Title:** Название задачи
- **Description:** Описание задачи(опционально)
- **WorkStatus:** статус задачи. Принимает следующие значения:
    - **Unknown:** Неизвестный статус
    - **Actual:** Активная задача
    - **Completed:** Выполненная задача
    - **Expired:** Просроченная задача
    - **Deleted:** Удаленная задача
- **DeadLineUtc:** Крайний срок задачи(опционально)
- **ReminderIntervals:** Интервалы напоминания о задаче(опционально)

### 6) GetActualWorks

- **Method:** GET
- **URL:** /api/recipients/{recipientId}/works/actual
- **Description:** Возвращает все актуальные задачи пользователя

- **Response Body:**

```json
[
    {
        "WorkId": "Guid",
        "Title": "string",
        "Description": "string?",
        "WorkStatus": "string",
        "DeadLineUtc": "DateTime?",
        "ReminderIntervals": "TimeSpan[]?"
    }
]
```

**Fields description**

- **WorkId:** Уникальный Идентификатор задачи

- **Title:** Название задачи
- **Description:** Описание задачи(опционально)
- **WorkStatus:** статус задачи. Принимает следующие значения:
    - **Unknown:** Неизвестный статус
    - **Actual:** Активная задача
    - **Completed:** Выполненная задача
    - **Expired:** Просроченная задача
    - **Deleted:** Удаленная задача
- **DeadLineUtc:** Крайний срок задачи(опционально)
- **ReminderIntervals:** Интервалы напоминания о задаче(опционально)

### 7) GetWorksForReminder

- **Method:** GET
- **URL:** /api/recipients/{recipientId}/works/ready-for-reminder
- **Description:** Возвращает все задачи, про которые нужно напомнить пользователю

- **Response Body:**

```json
[
    {
        "WorkId": "Guid",
        "Title": "string",
        "Description": "string?",
        "WorkStatus": "string",
        "DeadLineUtc": "DateTime?",
        "ReminderIntervals": "TimeSpan[]?"
    }
]
```

**Fields description**

- **WorkId:** Уникальный Идентификатор задачи
- **Title:** Название задачи
- **Description:** Описание задачи(опционально)
- **WorkStatus:** статус задачи. Принимает следующие значения:
    - **Unknown:** Неизвестный статус
    - **Actual:** Активная задача
    - **Completed:** Выполненная задача
    - **Expired:** Просроченная задача
    - **Deleted:** Удаленная задача
- **DeadLineUtc:** Крайний срок задачи(опционально)
- **ReminderIntervals:** Интервалы напоминания о задаче(опционально)

### 8) GetExpiredWorks

- **Method:** GET
- **URL:** /api/recipients/{recipientId}/works/expired
- **Description:** Возвращает все просроченный задачи для пользователя

- **Response Body:**

```json
[
    {
        "WorkId": "Guid",
        "Title": "string",
        "Description": "string?",
        "WorkStatus": "string",
        "DeadLineUtc": "DateTime?",
        "ReminderIntervals": "TimeSpan[]?"
    }
]
```

**Fields description**

- **WorkId:** Уникальный Идентификатор задачи
- **Title:** Название задачи
- **Description:** Описание задачи(опционально)
- **WorkStatus:** статус задачи. Принимает следующие значения:
    - **Unknown:** Неизвестный статус
    - **Actual:** Активная задача
    - **Completed:** Выполненная задача
    - **Expired:** Просроченная задача
    - **Deleted:** Удаленная задача
- **DeadLineUtc:** Крайний срок задачи(опционально)
- **ReminderIntervals:** Интервалы напоминания о задаче(опционально)

### 9) GetDeletedWorks

- **Method:** GET
- **URL:** /api/recipients/{recipientId}/works/deleted
- **Description:** Возвращает все удаленные задачи (с пометкой "deleted") пользователя

- **Response Body:**

```json
[
    {
        "WorkId": "Guid",
        "Title": "string",
        "Description": "string?",
        "WorkStatus": "string",
        "DeadLineUtc": "DateTime?",
        "ReminderIntervals": "TimeSpan[]?"
    }
]
```

**Fields description**

- **WorkId:** Уникальный Идентификатор задачи
- **Title:** Название задачи
- **Description:** Описание задачи(опционально)
- **WorkStatus:** статус задачи. Принимает следующие значения:
    - **Unknown:** Неизвестный статус
    - **Actual:** Активная задача
    - **Completed:** Выполненная задача
    - **Expired:** Просроченная задача
    - **Deleted:** Удаленная задача
- **DeadLineUtc:** Крайний срок задачи(опционально)
- **ReminderIntervals:** Интервалы напоминания о задаче(опционально)

### 10) GetCompletedWorks

- **Method:** GET
- **URL:** /api/recipients/{recipientId}/works/completed
- **Description:** Возвращает все выполненные задачи пользователя

- **Response Body:**

```json
[
    {
        "WorkId": "Id",
        "Title": "string",
        "Description": "string?",
        "WorkStatus": "string",
        "DeadLineUtc": "DateTime?",
        "ReminderIntervals": "TimeSpan[]?"
    }
]
```

**Fields description**

- **WorkId:** Уникальный Идентификатор задачи
- **Title:** Название задачи
- **Description:** Описание задачи(опционально)
- **WorkStatus:** статус задачи. Принимает следующие значения:
    - **Unknown:** Неизвестный статус
    - **Actual:** Активная задача
    - **Completed:** Выполненная задача
    - **Expired:** Просроченная задача
    - **Deleted:** Удаленная задача
- **DeadLineUtc:** Крайний срок задачи(опционально)
- **ReminderIntervals:** Интервалы напоминания о задаче(опционально)
