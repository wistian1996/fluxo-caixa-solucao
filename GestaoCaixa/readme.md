dotnet tool install --global dotnet-ef

cd Infrastructure

$ dotnet ef migrations add UpdateOutboxId -s ../GestaoFluxo.API

dotnet ef migrations remove -s ../GestaoFluxo.API

dotnet ef migrations script -o output_sql.sql -s ../GestaoFluxo.API

dotnet ef database update -s ../GestaoFluxo.API

dotnet ef migrations add AddDtCriacaoPadraoUtcPrecisao3 -s ../GestaoFluxo.API



. FluentValidation
. Mediator + Behavior (Pipeline)
. Outbox pattern
. Serilog + Logs + Seq (ferramenta para visualizaçao de logs)
. BackgroundServices
. RabbitMq
. Polly (resiliencia)
. Semaforo (thread e lock)
. Ocelot api gateway



Futuro
1. Organizar scripts da api de saldo
2. Criação de usuario para usar o rabbit (evitar usar guest)
3. Cofre para salvar credenciais
4. O identity server (responsavel geração de token) está na API Gateway, e normalmente deveria ser um componente separado (keyclock, identityserver4, cognito, ...)

1. rate limit
2. authentcation (api ou backend-direto)
3. configurar as rotas gateway
4. trace do seq

5. script k6 (performance) 

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