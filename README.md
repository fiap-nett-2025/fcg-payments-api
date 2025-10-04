# FCG Payments API

API de simulação de pagamentos para o projeto **FCG (Fiap Challenge Games)** — oferece endpoint para requisições de pagamento que retorna status aleatório com distribuição probabilística, e mapeamento de usuário via JWT.

---

## 🧾 Tabela de conteúdo

- Visão geral  
- Funcionalidades  
- Tech stack  
- Pré-requisitos  
- Instalação / execução local  
- Endpoints disponíveis  
- Exemplo de uso  
- Integração com Docker / Mongo  
- Enumeração de status de pagamento  
- Considerações / melhorias futuras  
- Licença  

---

## 📌 Visão geral

Essa API serve como **serviço de pagamento simulado** para integração com sistemas de pedidos.  
A lógica principal:

- Recebe requisição HTTP POST para processar um pagamento.  
- Extrai o **UserId** do token JWT enviado no cabeçalho `Authorization`.  
- Gera um status de pagamento com **70% de chance de sucesso** e **30% de chance de falha**, com variações de tipo de falha.  
- Retorna o resultado como JSON, contendo `UserId`, `PaymentStatus` e `Timestamp`.

É útil para testar fluxos de pedido/pagamento sem depender de gateway real.

---

## ✅ Funcionalidades

- Autenticação via token JWT (extrai `NameIdentifier` ou `sub`).  
- Simulação de pagamento com distribuição probabilística.  
- Vários tipos de falhas (ex: insuficient funds, timeout, cartão inválido).  
- Projeto estruturado para poder evoluir (suporte a Event Sourcing, integração com repositórios, logs).  

---

## 🛠 Tech stack

- .NET / C# — Lógica da API  
- JWT / Claims — Autenticação e identificação de usuário  
- Random — Simulação do status de pagamento  
- Docker + MongoDB (opcional) — Persistência / ambiente local de testes  

---

## 📋 Pré-requisitos

- .NET SDK (versão compatível com o projeto)  
- (Opcional) Docker & Docker Compose  
- (Se estiver persistindo algo) MongoDB acessível  

---

## ▶️ Instalação / execução local

1. Clone o repositório:

   ```bash
   git clone https://github.com/fiap-nett-2025/fcg-payments-api.git
   cd fcg-payments-api
   ```

2. (Opcional) Se houver **Docker Compose** incluído, suba os containers:

   ```bash
   docker compose up -d
   ```

3. Compile e execute localmente o projeto:

   ```bash
   dotnet build
   dotnet run --project FCG.Payments.API
   ```

---

## 🐳 Integração com Docker / Mongo

Se você usa MongoDB para persistência (por exemplo, Event Store ou logs de pagamento):

1. Inclua no `docker-compose.yml` um serviço Mongo.  
2. Configure o `ConnectionString` no `appsettings.json`.  
3. Configure `IMongoDatabase`, `IMongoClient` e repositórios via injeção de dependência.  
4. Eventualmente, você pode gravar os eventos de pagamento no Mongo como histórico.  

---

## 🔧 Melhorias futuras / Considerações

- Validação e verificação do token JWT (validar assinatura, issuer, claims esperadas).  
- Taxas, moedas, métodos reais de pagamento integrados.  
- Recurso de repetição/retentativa em falhas “timeout / redes”.  
- Auditoria / log persistido (salvar cada requisição resposta em banco).  
- Suporte a vários ambientes (desenvolvimento, homolog, produção).  
- Documentação Swagger / OpenAPI para testar o endpoint interativamente.  

---

## 📄 Licença

Este projeto está sob a licença **MIT** — veja o arquivo `LICENSE` para mais detalhes.
