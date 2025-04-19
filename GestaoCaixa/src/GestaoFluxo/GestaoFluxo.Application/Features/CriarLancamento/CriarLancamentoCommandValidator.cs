using FluentValidation;

namespace GestaoFluxo.Application.Features.CriarLancamento
{
    public class CriarLancamentoCommandValidator : AbstractValidator<CriarLancamentoCommand>
    {
        public CriarLancamentoCommandValidator()
        {
            RuleFor(s => s.ComercianteId)
                .NotEmpty();

            RuleFor(s => s.IsCredito)
                .NotEmpty();

            RuleFor(s => s.Valor)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
