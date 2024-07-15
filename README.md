# Avaliação Prática para Desenvolvedor(a) Backend .NET

## Descrição do Projeto

Este projeto consiste em duas aplicações desenvolvidas para processar um grande volume de dados de posições financeiras de clientes, obtidos de uma API externa, e disponibilizá-los para consulta através de uma API REST. Ambas as aplicações são dockerizadas para facilitar a implantação e execução.

## Estrutura do Projeto

O projeto é composto por duas partes principais:

1. **Aplicação Console**:
    - Responsável por consumir os dados da API externa.
    - Processar e armazenar os dados em um banco de dados PostgreSQL.

2. **API REST**:
    - Expor endpoints para consulta das posições financeiras armazenadas no banco de dados PostgreSQL.

## Aplicação Console

### Funcionalidades

- **Consumo da API**:
    - Consumir dados da API externa: `https://api.andbank.com.br/candidate/positions`.
    - Utilizar o header `X-Test-Key` com valor `a2mpznLX6F8rD` para autenticação.
    - Processar eficientemente grandes listas em memória.
    - Exibir informações de progresso no console.

- **Processamento e Armazenamento**:
    - Processar dados recebidos e aplicar transformações necessárias.
    - Armazenar dados em um banco de dados PostgreSQL usando Entity Framework Core e Npgsql.
    - Criar modelo de dados e migrations necessárias.

### Dockerização

- Criar um Dockerfile para a aplicação Console.
- Configurar para executar a aplicação automaticamente ao iniciar o container.

## API REST (ASP.NET Core 8)

### Endpoints

- **GET /api/positions/client/{clientId}**: Retorna as últimas posições para um determinado clientId.
- **GET /api/positions/client/{clientId}/summary**: Retorna as últimas posições e soma os valores para cada productId.
- **GET /api/positions/top10**: Retorna as 10 últimas posições com os maiores valores.

## Requisitos Atendidos

- Utilização das últimas novidades e melhores práticas do .NET 8.
- Utilização de técnicas de clean code e DDD
- Considerar o uso de bibliotecas para resiliência na comunicação com a API externa.

## Avaliação

### Critérios Avaliados

- **Funcionalidade**: A solução atende aos requisitos do cenário?
- **Qualidade do Código**: O código é limpo, legível e bem estruturado?
- **Eficiência e Escalabilidade**: A solução lida com o grande volume de dados de forma eficiente e escalável?
- **Conhecimento Técnico**: Domínio de .NET 8, ASP.NET Core, Entity Framework Core, PostgreSQL, Npgsql, Docker e melhores práticas de desenvolvimento.
- **Resolução de Problemas**: Capacidade de identificar e resolver problemas de forma eficiente.
- **Autenticação**: Autenticação correta na API externa.
- **Dockerização**: Aplicações corretamente dockerizadas e executáveis em containers Docker.

## Executando o Projeto

### Pré-requisitos

- Docker instalado.
- PostgreSQL configurado e em execução.

### Instruções

1. **Clonar o repositório**:
    ```bash
    git clone <url-do-repositorio>
    ```

2. **Construir e executar a Aplicação Console**:
    ```bash
    cd console-app
    docker build -t console-app .
    docker run -e CONNECTION_STRING="Host=localhost;Database=finance;Username=user;Password=pass" console-app
    ```

3. **Construir e executar a API REST**:
    ```bash
    cd api-rest
    docker build -t api-rest .
    docker run -p 8080:80 -e CONNECTION_STRING="Host=localhost;Database=finance;Username=user;Password=pass" api-rest
    ```

### Estrutura do Banco de Dados

O modelo de dados utilizado na aplicação é o seguinte:

- **Position**:
    - `PositionId`: string
    - `ProductId`: string
    - `ClientId`: string
    - `Date`: DateTime
    - `Value`: decimal
    - `Quantity`: decimal

## Autor

- Nome: Pablo R. Borges
- Email: pablorborges@gmail.com 

