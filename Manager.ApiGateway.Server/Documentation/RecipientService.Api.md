# RecipientService Api

**Description**: Сервис для управления пользователем

## RecipientAccountController

**Description**: Управление аккаунтом пользователя

### 1) CreateRecipientAccount

- **Method:** POST
- **URL:** /api/recipients/
- **Description:** Создает новый аккаунт для пользователя
- **Request Body:**

```json
{
    "Login": "string",
    "Password": "string",
    "RecipientTimeUtcOffsetHours": "int"
}
```

**Fields description**

- **Login:** Уникальный логин пользователя
- **Password:** Пароль пользователя
- **RecipientTimeUtcOffsetHours:** Часы смещения времени получателя относительно UTC. Принимает значение от -12 до 14

- **Responses**

    - **201 Created**
    - **400 Bad Request**
    - **409 Conflict**

### 2) GetRecipientAccount

- **Method:** GET
- **URL:** /api/recipients/{recipientId}
- **Description:** Возвращает данные для аккаунта

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

**Fields description**

- **Login:** Уникальный логин пользователя
- **AccountState:** Текущее состояние аккаунта. Принимает следующие значения:
    - "Unknown"(Неизвестно),
    - Inactive(Неактивный),
    - Active(Активный),
    - Deleted(Удаленный),
    - Banned(Заблокированный)
- **StateReason:** Почему аккаунт находится в том или ином состоянии
- **RecipientTimeUtcOffsetHours:** Часы смещения времени получателя относительно UTC. Принимает значение от -12 до 14

### 3) PatchRecipientAccount

- **Method:** PATCH
- **URL:** /api/recipients/{recipientId}
- **Description:** Изменяет данные аккаунта
- **Request Body:**

```json
{
    "NewLogin": "string",
    "NewPassword": "string",
    "NewRecipientTimeUtcOffsetHours": "int?"
}
```

**Fields description**

- **NewLogin:** Новый логин для пользователя
- **NewPassword:** Новый пароль для пользователя
- **NewRecipientTimeUtcOffsetHours:** Новые часы смещения времени получателя относительно UTC. Принимает значение от -12
  до 14

- **Responses**

    - **200 OK**
    - **401 Bad Request**
    - **404 Not Found**

### 4) DeleteRecipientAccount

- **Method:** DELETE
- **URL:** /api/recipients/{recipientId}
- **Description:** Добавляет к имени пользователя Deleted и переводит статус в Deleted

- **Responses:**

    - **200 OK**
    - **401 Bad Request**
    - **404 Not Found**

## RecipientAuthorizationController

**Description**: Управление авторизацией пользователя

### 1) GetAuthorization

- **Method:** GET
- **URL:** /api/recipients/{recipientId}/authorization
- **Description:** Возвращает авторизацию пользователя


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
**Fields description**

- **RequestService:** Запрашиваемый сервис
- **RequestedResource** Запрашиваемый ресурс
- **RecipientAuthorizationStatus** Статус авторизации. Принимает следующие значения:
    - "Success"(Доступ разрешен),
    - AccountIsNotActive(Доступ запрещён, т.к. аккаунт не в активном состоянии),
    - AccessToServiceDenied(Доступ к сервису запрещён),
    - AccessToResourceDenied(Доступ к ресурсу запрещён)