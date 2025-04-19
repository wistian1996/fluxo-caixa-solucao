using Moq;
using Microsoft.Extensions.Logging;
using SaldoConsolidado.Application.Features.NovoLancamento;
using SaldoConsolidado.Domain.Entities;
using SaldoConsolidado.Domain.Exceptions;
using SaldoConsolidado.Domain.Interfaces;
using SaldoConsolidado.Domain.SeedWork;

namespace SaldoConsolidadeTeste
{
    public class ProcessarNovoLancamentoCommandHandlerTests
    {
        private Mock<ISaldoConsolidadoDiarioRepository> _consolidadoDiarioRepoMock;
        private Mock<IEventoLancamentoRepository> _eventoLancamentoRepoMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<ILogger<ProcessarNovoLancamentoCommandHandler>> _loggerMock;
        private ProcessarNovoLancamentoCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _consolidadoDiarioRepoMock = new Mock<ISaldoConsolidadoDiarioRepository>();
            _eventoLancamentoRepoMock = new Mock<IEventoLancamentoRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<ProcessarNovoLancamentoCommandHandler>>();

            _eventoLancamentoRepoMock.Setup(r => r.UnitOfWork).Returns(_unitOfWorkMock.Object);

            _handler = new ProcessarNovoLancamentoCommandHandler(
                _consolidadoDiarioRepoMock.Object,
                _eventoLancamentoRepoMock.Object,
                _loggerMock.Object
            );
        }

        [Test]
        public async Task Handle_DeveProcessarLancamento_ComSucesso()
        {
            // Arrange
            var command = new ProcessarNovoLancamentoCommand
            {
                Id = Guid.NewGuid(),
                ComercianteId = Guid.NewGuid(),
                DataCriacao = DateTime.UtcNow,
                Valor = 100,
                IsCredito = true
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(result);
            _unitOfWorkMock.Verify(u => u.BeginTransaction(), Times.Once);
            _eventoLancamentoRepoMock.Verify(r => r.AddEvento(It.IsAny<EventoLancamento>()), Times.Once);
            _consolidadoDiarioRepoMock.Verify(r => r.UpsertSaldoDiario(
                command.ComercianteId, command.DataCriacao, command.Valor, command.IsCredito), Times.Once);
            _unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }

        [Test]
        public async Task Handle_DeveTratarEventoJaRegistradoException()
        {
            var command = new ProcessarNovoLancamentoCommand
            {
                Id = Guid.NewGuid(),
                ComercianteId = Guid.NewGuid(),
                DataCriacao = DateTime.UtcNow,
                Valor = 150,
                IsCredito = false
            };

            _eventoLancamentoRepoMock
                .Setup(r => r.AddEvento(It.IsAny<EventoLancamento>()))
                .ThrowsAsync(new EventoJaRegistradoException("Evento já registrado"));

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsTrue(result);
            _unitOfWorkMock.Verify(u => u.Rollback(), Times.Once);
            _loggerMock.Verify(
                l => l.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("já processado")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Test]
        public void Handle_DeveLancarException_QuandoErroInesperado()
        {

            var command = new ProcessarNovoLancamentoCommand
            {
                Id = Guid.NewGuid(),
                ComercianteId = Guid.NewGuid(),
                DataCriacao = DateTime.UtcNow,
                Valor = 200,
                IsCredito = true
            };

            _eventoLancamentoRepoMock
                .Setup(r => r.AddEvento(It.IsAny<EventoLancamento>()))
                .ThrowsAsync(new Exception("Erro inesperado"));

            var ex = Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
            Assert.That(ex.Message, Is.EqualTo("Erro inesperado"));

            _unitOfWorkMock.Verify(u => u.Rollback(), Times.Once);
            _loggerMock.Verify(
                l => l.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("Falha ao processar lançamento")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}
