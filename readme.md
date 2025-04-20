# Gestor de caixa
Um comerciante precisa controlar o seu fluxo de caixa diário com os
lançamentos (débitos e créditos), também precisa de um relatório que
disponibilize o saldo diário consolidado.

##  System Context Diagram
![System Context Diagram](assets/diagrama1.png) <br><br>

## Container Diagram
![System Context Diagram](assets/diagrama2.png) <br><br>

## Resiliência e integração
Foram construídos 2 serviços (Gestão fluxo de caixa e Consulta Saldo).
Para garantir que o Serviço gestão de fluxo de caixa funcione de forma indepente foram adicionadas as seguintes implementações:

## API GestaoFluxo
Utilizei o RabbitMq para se comunicar com a API ConsultaSaldo e para garantir que mesmo que a API Consulta Saldo e o RabbitMq  esteja fora, a API fluxo de caixa permanece ativa e operando, utilizei o Pattern OutBoxMesage

![System Context Diagram](assets/outboxpattern.png) <br><br>

## API SaldoConsolidado
Consulta informações da fila e insere as informações na sua própria base de dados.

Obs: Para incrementar ou decrementar o saldo do usuário eu poderia consultar, somar e 
depois fazer o update, mas para isso eu precisaria incrementar um mecanismo de lock da tabela
para garantir que o saldo não fosse alterado por outro processo. Então resolvi utilizar o dapper e fazer
o script de consulta e inserção na mesma query.

## API Gateway
Utilizei o Ocelot para criar uma API Gateway simples para centralizar os endpoints, autenticação e RateLimit.
Normalmente se utiliza autenticação nos dois lados, Gateway e API, mas neste caso optei por adicionar uma chave pública no 
Gateway e quando alguma requisição chega em uma das API, esta chave pública é consultada no Gateway
para verificar a Origem do Token. Assim garantimos que o Token gerado tem a origem API gateway.

![System Context Diagram](assets/gateway.png) <br><br>

## Testes de performance

## Logs

## Front End

## Banco de dados 
Banco de dados relacional MySql.

## gestao-fluxo
![System Context Diagram](assets/gestao_fluxo.png) <br><br>

## saldo-consolidado
![System Context Diagram](assets/saldo_consolidado.png) <br><br>

# Tecnologias utilizadas
1. FluentValidation
2. Mediator + Behavior (Pipeline)
3. Outbox pattern
4. Entity Framework
5. BackgroundServices
6. Dapper
7. Serilog + Logs + Seq
8. Ocelot api gateway
9. Jwt
10. MySql
11. Net 8
12. Docker
13. Docker Compose


Futuro
1. Organizar scripts da api de saldo
2. Criação de usuario para usar o rabbit (evitar usar guest)
3. Cofre para salvar credenciais
4. O identity server (responsavel geração de token) está na API Gateway, e normalmente deveria ser um componente separado (keyclock, identityserver4, cognito, ...)
5 . Orquestrador de containers
1. rate limit
2. authentcation (api ou backend-direto)


5. script k6 (performance) 


.net

#. grafana
#. orquestraçao


options.TokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidIssuer = JwtTokenParams.Issuer,

    ValidateAudience = true,
    ValidAudience = JwtTokenParams.Audience,
<=============================>
    ValidateLifetime = false, <=============================>
<=============================>
    ValidateIssuerSigningKey = true,
    ClockSkew = TimeSpan.Zero,
    IssuerSigningKeyResolver = (token, securityToken, kid, parameters) =>
    {
        var key = IdentityServerJwksService.GetKeyJwksById(kid);

        if (key != null)
        {
            return [key];
        }

        return [];
    },
};

opentelemetry seq .net core

https://github.com/karlospn/opentelemetry-metrics-demo

https://grafana.com/grafana/dashboards/17706-asp-net-otel-metrics/

performance tests
K6_WEB_DASHBOARD=true k6 run criar-lancamento-test.js