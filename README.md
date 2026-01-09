# 🎮 FIAP Cloud Games (FCG) - Payment Service

## 📚 Sobre o Projeto

Microsserviço API de simulação de pagamentos para o projeto **FCG (Fiap Cloud Games)** — oferece endpoint para requisições de pagamento que retorna status aleatório com distribuição probabilística, e mapeamento de usuário via JWT.
Desenvolvida dentro do ecossistema educacional da FIAP (Faclidade de Informática e Administração Palista). 

[Documentação](https://www.notion.so/Fiap-Cloud-Games-FCG-1dea50ade75480e78653c05e2cca2193?pvs=4)

## :money_with_wings:  Sobre o Serviço de Pagamentos

O serviço de pagamentos é responsável por gerenciar o carrinho e os pedidos no nosso projeto FCG. Ele oferece funcionalidades para adcionar e remover itens do carrinho(cart), listar carrinhos e fazer o checkout. Além disso, também tem endpoints de pedido(order) como para mostrar um pedido e e realizar pagamento.

:space_invader: Essa API foi feita com Event Sourcing, registrando os eventos no nosso banco MongoDB. 
<img height="30" width="40" src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/mongodb/mongodb-original.svg" />

## :envelope_with_arrow: <img align="center" height="30" width="40" src="https://cdn.jsdelivr.net/gh/devicons/devicon@latest/icons/amazonwebservices/amazonwebservices-plain-wordmark.svg"> Messageria

Quando o usuário faz um POST no endpoint "api/Order/{orderId}/pay", se o pagamento for bem sucedido, essa API irá publicar uma mensagem na fila "user-game-library-added-queue" e outra na fila "game-popularity-increased-queue". Essas mensagens em fila serão consumidas pelo worker da API de Usuários e pelo worker da API de Games.

Nesse projeto fazemos a comunicação entre os microsserviços usando o Amazon SQS. Abaixo está a lista dos arquivos principais envolvidos com a messageria:
- Pasta k8s (nessa pasta se encontram configMaps e arquivo de deployment);
- FCG.Payments.Infra.Messaging.Sqs.AmazonSqsPublisher.

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
