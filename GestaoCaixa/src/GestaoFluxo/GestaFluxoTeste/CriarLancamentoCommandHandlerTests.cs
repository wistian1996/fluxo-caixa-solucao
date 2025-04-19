using AutoMapper;
using GestaoFluxo.Application.Dtos;
using GestaoFluxo.Application.Features.CriarLancamento;
using GestaoFluxo.Application.Interfaces.EventPublisher;
using GestaoFluxo.Application.Interfaces.OutboxService;
using GestaoFluxo.Domain.Entities;
using GestaoFluxo.Domain.Interfaces;
using global::GestaoFluxo.Domain.SeedWork;
using Microsoft.Extensions.Logging;
using Moq;

namespace GestaFluxoTeste
{

    namespace GestaoFluxo.Tests.Application.Features
    {
        public class CriarLancamentoCommandHandlerTests
        {
            private Mock<ILancamentoRepository> _lancamentoRepositoryMock;
            private Mock<IUnitOfWork> _unitOfWorkMock;
            private Mock<IMapper> _mapperMock;
            private Mock<IEventPublisher> _eventPublisherMock;
            private Mock<IOutboxService> _outboxServiceMock;
            private Mock<ILogger<CriarLancamentoCommandHandler>> _loggerMock;
            private CriarLancamentoCommandHandler _handler;

            [SetUp]
            public void Setup()
            {
                _lancamentoRepositoryMock = new Mock<ILancamentoRepository>();
                _unitOfWorkMock = new Mock<IUnitOfWork>();
                _mapperMock = new Mock<IMapper>();
                _eventPublisherMock = new Mock<IEventPublisher>();
                _outboxServiceMock = new Mock<IOutboxService>();
                _loggerMock = new Mock<ILogger<CriarLancamentoCommandHandler>>();

                _lancamentoRepositoryMock.Setup(r => r.UnitOfWork).Returns(_unitOfWorkMock.Object);

                _handler = new CriarLancamentoCommandHandler(
                    _lancamentoRepositoryMock.Object,
                    _mapperMock.Object,
                    _eventPublisherMock.Object,
                    _loggerMock.Object,
                    _outboxServiceMock.Object
                );
            }

            [Test]
            public async Task Handle_DeveCriarLancamento_ComSucesso()
            {
                var command = new CriarLancamentoCommand
                {
                    ComercianteId = Guid.NewGuid(),
                    IsCredito = true,
                    Valor = 500
                };

                var expectedDto = new LancamentoDto();

                _mapperMock.Setup(m => m.Map<LancamentoDto>(It.IsAny<Lancamento>()))
                           .Returns(expectedDto);

                var result = await _handler.Handle(command, CancellationToken.None);

                Assert.IsNotNull(result);
                Assert.AreEqual(expectedDto, result);

                _lancamentoRepositoryMock.Verify(r => r.InserirLancamento(It.IsAny<Lancamento>(), It.IsAny<CancellationToken>()), Times.Once);
                _unitOfWorkMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
                _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
                _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
                _outboxServiceMock.Verify(o => o.AddAsync(It.IsAny<OutboxMessage>(), It.IsAny<CancellationToken>()), Times.Once);
            }

            [Test]
            public void Handle_DeveLancarExcecaoELogarErro_QuandoFalhar()
            {
                var command = new CriarLancamentoCommand
                {
                    ComercianteId = Guid.NewGuid(),
                    IsCredito = true,
                    Valor = 100
                };

                _lancamentoRepositoryMock.Setup(r => r.InserirLancamento(It.IsAny<Lancamento>(), It.IsAny<CancellationToken>()))
                                         .ThrowsAsync(new Exception("Falha simulada"));

                var ex = Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
                Assert.That(ex.Message, Is.EqualTo("Falha simulada"));

                _loggerMock.Verify(
                    x => x.Log(
                        LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Falha ao processar criação do lançamento")),
                        It.IsAny<Exception>(),
                        It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                    Times.Once);
            }
        }
    }
}
