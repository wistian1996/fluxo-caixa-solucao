using Asp.Versioning;
using GestaoFluxo.Application.Features.CriarLancamento;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace GestaoFluxo.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/lancamentos")]
    [ApiController]
    public class LancamentoController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _contextAccessor;

        public LancamentoController(IMediator mediator, IHttpContextAccessor contextAccessor)
        {
            _mediator = mediator;
            _contextAccessor = contextAccessor;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CriarLancamento([FromBody] CriarLancamentoCommand command)
        {
            var userId = _contextAccessor.HttpContext!.User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value!;

            command.ComercianteId = Guid.Parse(userId);

            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}
