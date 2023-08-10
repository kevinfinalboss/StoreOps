
![Logo](https://github.com/kevinfinalboss/StoreOps/blob/master/screenshots/Logo.jpg?raw=true)


# StoreOps

StoreOps é um sistema de gerenciamento de lojas que oferece uma interface de usuário eficiente para gerenciar diversos aspectos do negócio. Ele integra uma variedade de funcionalidades como o controle de produtos, clientes e configurações.


## Funcionalidades

- Confirmação de criação de usuário e compra de produtos por e-mail.
- Criação de produtos e categorias.
- Filtrar usuários e criação de relátorios excel.
- Armazenamento MongoDB.

## Rodando localmente

Clone o projeto

```bash
  git clone https://github.com/kevinfinalboss/StoreOps
```

Entre no diretório do projeto

```bash
  cd StoreOps
  cd src
```

Instale as dependências

```bash
  dotnet restore
```

Inicie o programa

```bash
  dotnet run .\Program.cs
```


## Variáveis de Ambiente

Para rodar esse projeto, você vai precisar adicionar as seguintes variáveis de ambiente no seu .env

`MONGODB_CONNECTION_STRING`

`MONGODB_DATABASE_NAME`

`SMTP_USERNAME`

`SMTP_PASSWORD`

`SMTP_HOST`

`SMTP_PORT`



## Suporte

Para suporte, mande um email para help.kevin@kevindev.com.br ou entre em nosso canal do Discord.


## Autores

- [@kevinfinalboss](https://www.github.com/kevinfinalboss)


## Feedback

Se você tiver algum feedback, por favor nos deixe saber por meio de atendimento@kevindev.com.br


## Contribuindo

Contribuições são sempre bem-vindas!

Crie uma branch a partir da master e crie um PR com a atualização.


## Licença

[MIT](https://choosealicense.com/licenses/mit/)


## Screenshots

![App Screenshot](https://github.com/kevinfinalboss/StoreOps/blob/master/screenshots/MenuPrincipal.png?raw=true)
![App Screenshot](https://github.com/kevinfinalboss/StoreOps/blob/master/screenshots/MenuConfigura%C3%A7%C3%A3o.jpg?raw=true)
![App Screenshot](https://github.com/kevinfinalboss/StoreOps/blob/master/screenshots/MenuClientes.jpg?raw=true)


