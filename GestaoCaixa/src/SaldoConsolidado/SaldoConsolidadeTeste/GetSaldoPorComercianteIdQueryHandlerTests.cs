using Moq;
using SaldoConsolidado.Application.Features.Saldo;
using SaldoConsolidado.Domain.Entities;
using SaldoConsolidado.Domain.Interfaces;

namespace SaldoConsolidadeTeste
{
    [TestFixture]
    public class GetSaldoPorComercianteIdQueryHandlerTests
    {
        private Mock<ISaldoConsolidadoDiarioRepository> _saldoRepositoryMock;
        private GetSaldoPorComercianteIdQueryHandler _handler;

        [SetUp]
        public void Setup()
        {
            _saldoRepositoryMock = new Mock<ISaldoConsolidadoDiarioRepository>();
            _handler = new GetSaldoPorComercianteIdQueryHandler(_saldoRepositoryMock.Object);
        }

        [Test]
        public async Task Handle_DeveRetornarSaldoConsolidadoDto_QuandoSaldoExistir()
        {
            var comercianteId = Guid.NewGuid();
            var dataRef = new DateTime(2025, 04, 19);

            var saldoEntity = new SaldoConsolidadoDiario(
                comercianteId,
                dataRef,
                10
            );

            _saldoRepositoryMock
                .Setup(repo => repo.GetSaldoDiario(comercianteId, dataRef))
                .ReturnsAsync(saldoEntity);

            var query = new GetSaldoPorComercianteIdQuery(comercianteId, dataRef);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.AreEqual(comercianteId, result!.ComercianteId);
            Assert.AreEqual(10, result.Saldo);
            _saldoRepositoryMock.Verify(repo => repo.GetSaldoDiario(comercianteId, dataRef), Times.Once);
        }

        [Test]
        public async Task Handle_DeveRetornarNull_QuandoSaldoNaoExistir()
        {
            var comercianteId = Guid.NewGuid();
            var dataRef = DateTime.UtcNow.Date;

            _saldoRepositoryMock
                .Setup(repo => repo.GetSaldoDiario(comercianteId, dataRef))
                .ReturnsAsync((SaldoConsolidadoDiario?)null);

            var query = new GetSaldoPorComercianteIdQuery(comercianteId, dataRef);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.IsNull(result);
            _saldoRepositoryMock.Verify(repo => repo.GetSaldoDiario(comercianteId, dataRef), Times.Once);
        }
    }
}
