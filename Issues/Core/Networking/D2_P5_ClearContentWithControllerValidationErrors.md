# Понятный текст ошибки при не успешной валидации с помощью DataAnnotations

## Описание

Сейчас возвращается громоздкий контент в ответе от сервера, если валидация с помощью атрибутов DataAnnotations не
прошла. Хочется сделать контент с ошибкой более красивым и понятным, чтобы его можно было выводить пользователю

Пример текущего контента в ответе

```json
{
    "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
    "title": "One or more validation errors occurred.",
    "status": 400,
    "errors": {
        "Login": [
            "The Login field is required."
        ],
        "Password": [
            "The Password field is required."
        ],
        "RecipientTimeUtcOffsetHours": [
            "Неправильное смещение времени от всемирного времени UTC"
        ]
    },
    "traceId": "00-51c1ca99d07569b3645940d4adf1421f-f90a680bcc44c240-00"
}
```

Данный ответ возвращается из сервиса RecipientService, метод создания аккаунта `POST api/recipient-account`

