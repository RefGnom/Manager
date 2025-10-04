## Задача 2. Динамическое определение выводимого описания

### Описание
Команда help выводит одно и тоже независимо от пространства, в котором была использована.
Хочется сделать вывод этой команды динамическим.

При вызове из базового пространства manager `manager help`:
выводятся все команды, которые есть у тулзы и их описание.

При вызове из пространства manager timers `manager timers help`:
выводятся все команды в пространстве timers. `delete, get, reset, my, start, stop`

При вызове из пространства конкретной команды `manager timers start help`:
выводится информация только об указанной команде

### Ожидаемый результат
Пока что ожидается, что уточнение пространства просто уменьшает
количество выводимой информации пользователю, чтобы можно было меньше читать.

Но стоит подумать, что указание конкретного пространства побуждает указать и больше
информации об этом пространстве и его командах. Также, при вызове help для конкретной
команды может быть выведено максимально подробное описание по этой команде.

Рассмотри это как точку расширения, чтобы в будущем не пришлось
всё переделывать для этой фичи.

Привожу список кейсов, как ожидаю что будет работать:
```
> manager help

Common
manager help                            Get description for spaces and commands.
manager auth                            Authentication user for use manager

For your time management
manager timers delete                   Mark timer as deleted.
manager timers get                      Get timer by name.
manager timers reset                    Reset timer and make previous timer archived.
manager timers my                       Get your timers.
manager timers start                    Start new or stopped timer.
Options:
-t      --start_time        Time when need start timer
--timeout           After how long time notify about ending


manager timers stop                     Stop started timer.
Options:
-t      --stop_time         Time when need stop timer
```
```
> manager timers help

For your time management
manager timers delete                   Mark timer as deleted.
manager timers get                      Get timer by name.
manager timers reset                    Reset timer and make previous timer archived.
manager timers my                       Get your timers.
manager timers start                    Start new or stopped timer.
Options:
-t      --start_time        Time when need start timer
--timeout           After how long time notify about ending


manager timers stop                     Stop started timer.
Options:
-t      --stop_time         Time when need stop timer
```
```
> manager timers start help

manager timers start                    Start new or stopped timer.
Options:
-t      --start_time        Time when need start timer
--timeout           After how long time notify about ending
```
```
> manager timers delete help

manager timers delete                   Mark timer as deleted.
```
```
> manager auth help

manager auth                            Authentication user for use manager
```
```
> manager help help

???????
```