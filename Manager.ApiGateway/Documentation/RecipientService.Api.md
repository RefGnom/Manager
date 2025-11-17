# RecipientService Api

**Description**: Сервис для управления пользователем

## RecipientAccountController

**Description**: Управление аккаунтом пользователя

### 1) CreateRecipientAccount

- **Method:** POST
- **URL:** /api/recipient/
- **Description:** Создает новый аккаунт для пользователя
- **Request Body:**

```json
{
    "Login": "string",
    "Password": "string",
    "RecipientTimeUtcOffsetHours": "int"
}
```

- **Responses**

    - **201 Created**
    - **400 Bad Request**
    - **409 Conflict**

### 2) GetRecipientAccount

- **Method:** GET
- **URL:** /api/recipient/{recipientId}
- **Description:** Получает данные для аккаунта

- **Responses**

    - **200 OK**
    - **404 Not Found**

- **Response Body:**

```json
{
    "Login": "string",
    "AccountState": "string",
    "StateReason": "string",
    "RecipientTimeUtcOffset": "TimeSpan"
}

```

### 3) PatchRecipientAccount

- **Method:** PATCH
- **URL:** /api/recipient/{recipientId}
- **Description:** Изменяет данные аккаунта
- **Request Body:**

```json
{
    "NewLogin": "string",
    "NewPassword": "string",
    "NewRecipientTimeUtcOffsetHours": "int?"
}
```

- **Responses**

    - **200 OK**
    - **401 Bad Request**
    - **404 Not Found**

### 4) DeleteRecipientAccount

- **Method:** DELETE
- **URL:** /api/recipient/{recipientId}
- **Description:** Удаляет пользователя

- **Responses:**

    - **200 OK**
    - **401 Bad Request**
    - **404 Not Found**

## RecipientAuthorizationController

**Description**: Управление авторизацией пользователя

### 1) GetAuthorization

- **Method:** GET
- **URL:** /api/recipient/{recipientId}/authorization
- **Description:** Получает авторизацию пользователя


- **Responses**

    - **200 OK**
    - **404 Not Found**

- **Response Body:**

```json
{
    "RequestService": "string",
    "RequestedResource": "string",
    "RecipientAuthorizationStatus": "string"
}

```