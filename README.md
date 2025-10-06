# 🎮 FIAP Cloud Games (FCG) - Payment Service

## 📚 Sobre o Projeto

Microsserviço API de simulação de pagamentos para o projeto **FCG (Fiap Cloud Games)** — oferece endpoint para requisições de pagamento que retorna status aleatório com distribuição probabilística, e mapeamento de usuário via JWT.
Desenvolvida dentro do ecossistema educacional da FIAP (Faclidade de Informática e Administração Palista). 

[Documentação](https://www.notion.so/Fiap-Cloud-Games-FCG-1dea50ade75480e78653c05e2cca2193?pvs=4)

## :money_with_wings:  Sobre o Serviço de Pagamentos

O serviço de pagamentos é responsável por gerenciar o carrinho e os pedidos no nosso projeto FCG. Ele oferece funcionalidades para adcionar e remover itens do carrinho(cart), listar carrinhos e fazer o checkout. Além disso, também tem endpoints de pedido(order) como para mostrar um pedido e e realizar pagamento.

:space_invader: Essa API foi feita com Event Sourcing, registrando os eventos no nosso banco MongoDB. 
<img height="30" width="40" src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/mongodb/mongodb-original.svg" />

A lógica de <B>Pagamento</b> dessa API é:
- O usuário se autentifica com seu token JWT, o programa extrai o UserId do token JWT enviado no cabeçalho `Authorization`;
- Usuário executa o método "api/Order/{orderId}/pay";
- A <b>Azure function</b> é chamada e ela gera um status de pagamento com **70% de chance de sucesso** e **30% de chance de falha**, com variações de tipo de falha;  
- Retorna para o usuário o resultado como JSON, contendo `UserId`, `PaymentStatus` e `Timestamp`.

### :computer: Comunicação com a API de Usuarios e com a API de Jogos

Para usar a API de Pagamentos é necessário <b>fazer autenticação via token JWT obtido pelo metodo de login da api de Usuários</b>. A API de Pagamentos se comunica com o API de Jogos para pegar o `GameId` que será adcionado ao carrinho. 

Além disso, depois que o pagamento com a Azure Function é processado se ele for bem sucedido, essa API irá acessar a API de Usuários para cadastrar os novos jogos adquiridos na biblioteca de jogos do usuário e também irá aumentar a popularidade dos jogos comprados, acessando a API de Jogos novamente.

## ⚙️ Tecnologias e Plataformas utilizadas

- [.NET 8](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio](https://visualstudio.microsoft.com/pt-br/)
- [EF Core](https://learn.microsoft.com/pt-br/ef/core/)
- [ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/)
- [XUnit](https://xunit.net/)
- [Swagger](https://swagger.io/)
- [Docker](https://www.docker.com/)
- [MongoDB](https://www.mongodb.com/)

## 🛠️ Como Executar

### Usando Docker

1. Certifique-se de ter o [Docker](https://www.docker.com/get-started/) instalado em sua máquina.
2. No terminal, navegue até a raiz do projeto.
3. Execute o comando abaixo para construir e iniciar os containers:

```bash
docker-compose up -d --build
```

## 🤝 Contribuição

Contribuições são bem-vindas! Sinta-se à vontade para abrir issues ou pull requests.

## 📄 Licença

Este projeto está licenciado sob a licença MIT.

---

Feito com ❤️!
