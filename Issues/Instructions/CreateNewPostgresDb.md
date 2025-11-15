### Создание новой базы данных Postgresql

1. Заходим на удалённый сервер командой
   > ssh username@your_server_ip

   В случае с сервером ManagerPostgreSqlServer на TimeWeb это
   > ssh root@147.45.150.159

2. Переключаемся на пользователя постгрес
   > sudo -u postgres psql

3. Создаём нового пользователя для нашей бд.
   > CREATE USER myuser WITH PASSWORD 'your_strong_password_here';

   Под логином и паролем этого пользователя будут ходить сервисы, их нужно обязательно запомнить и добавить в секреты
   нужных сервисов

4. Создаём базу с этим пользователем
   > CREATE DATABASE mydatabase OWNER myuser;

5. Даём нашему пользователю все права на базу. Тут можно управлять правами для сервисных учёток и пользовательских
    > GRANT ALL PRIVILEGES ON DATABASE mydatabase TO myuser;

6. Готово, можем подкладывать настройки сервису и локально подключаться к бд