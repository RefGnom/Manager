### Чтобы подключить аутентификацию, нужно сделать следующее:

1. В конфигурацию добавить настройку `AuthenticationSetting:Resource` - имя вашего ресурса, апи ключу доступ выдаётся на
   ресурсы;
2. Сконфигурировать DI IServiceCollection методом расширения `AddApiKeyRequirementExtensions.AddApiKeyRequirement`;
3. Билдеру приложения IApplicationBuilder добавить мидлвару аутентификации с помощью метода расширения
   `AddApiKeyRequirementExtensions.UseAuthenticationMiddleware`.

### Если используется сваггер:

В конфигурацию генератора сваггера нужно добавить место для авторизации с помощью метода расширения
`AddApiKeyRequirementExtensions.AddApiKeyRequirement`.

`serviceCollection.AddSwaggerGen(c => c.AddApiKeyRequirement())`