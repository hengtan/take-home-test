using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Fundo.Application.Commands.Loans.Create;
using Fundo.Application.Interfaces;
using Fundo.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Fundo.Services.Tests.Unit.Application.Commands.Loans;

public class CreateLoanCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenLoanIsValid()
    {
        // Arrange
        var command = new CreateLoanCommand(1500m, 1500m, "Maria Silva");

        var mockLoanRepo = new Mock<ILoanRepository>();
        mockLoanRepo
            .Setup(repo => repo.AddAsync(It.IsAny<Loan>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var mockUow = new Mock<IUnitOfWork>();
        mockUow.Setup(u => u.LoanRepository).Returns(mockLoanRepo.Object);
        mockUow.Setup(u => u.CompleteAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = CreateHandler(mockUow.Object);

        // Act
        var handlerTuple = CreateHandler(mockUow.Object);
        var result = await handlerTuple.Handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenAmountIsNegative()
    {
        // Arrange
        var command = new CreateLoanCommand(-1000m, -1000m, "Negative");
        var handler = CreateHandler(Mock.Of<IUnitOfWork>()).Handler;

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be("Validation");
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenCompleteFails()
    {
        // Arrange
        var command = new CreateLoanCommand(2000m, 2000m, "Joao");

        var mockRepo = new Mock<ILoanRepository>();
        mockRepo.Setup(r => r.AddAsync(It.IsAny<Loan>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var mockUow = new Mock<IUnitOfWork>();
        mockUow.Setup(u => u.LoanRepository).Returns(mockRepo.Object);
        mockUow.Setup(u => u.CompleteAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Erro saving loan"));

        var handler = CreateHandler(mockUow.Object);

        // Act
        var result = await handler.Handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be("Internal");
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WithDifferentValues()
    {
        // Arrange
        var command = new CreateLoanCommand(5000m, 5000m, "Carlos Mendes");

        var mockRepo = new Mock<ILoanRepository>();
        var mockUow = new Mock<IUnitOfWork>();

        mockRepo.Setup(r => r.AddAsync(It.IsAny<Loan>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        mockUow.Setup(u => u.LoanRepository).Returns(mockRepo.Object);
        mockUow.Setup(u => u.CompleteAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = CreateHandler(mockUow.Object);

        // Act
        var result = await handler.Handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_Work_WhenRepositoryIsDelayed()
    {
        // Arrange
        var command = new CreateLoanCommand(3000m, 3000m, "Delay Test");

        var mockRepo = new Mock<ILoanRepository>();
        mockRepo.Setup(r => r.AddAsync(It.IsAny<Loan>(), It.IsAny<CancellationToken>()))
            .Returns(async () => await Task.Delay(300));

        var mockUow = new Mock<IUnitOfWork>();
        mockUow.Setup(u => u.LoanRepository).Returns(mockRepo.Object);
        mockUow.Setup(u => u.CompleteAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = CreateHandler(mockUow.Object);

        // Act
        var result = await handler.Handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Handle_Should_Fail_WhenLoanRepositoryIsNull()
    {
        // Arrange
        var command = new CreateLoanCommand(1000m, 1000m, "Null Repo Test");

        var mockUow = new Mock<IUnitOfWork>();
        mockUow.Setup(u => u.LoanRepository).Returns((ILoanRepository)null!);

        var handler = CreateHandler(mockUow.Object);

        // Act
        var result = await handler.Handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be("Internal");
    }

    [Fact]
    public async Task Handle_Should_Respect_Capitalization_WhenCreatingLoan()
    {
        // Arrange
        var command = new CreateLoanCommand(1200m, 1200m, "JOANA SILVA");

        var mockRepo = new Mock<ILoanRepository>();
        var mockUow = new Mock<IUnitOfWork>();

        Loan? capturedLoan = null;

        mockRepo.Setup(r => r.AddAsync(It.IsAny<Loan>(), It.IsAny<CancellationToken>()))
            .Callback<Loan, CancellationToken>((loan, _) => capturedLoan = loan)
            .Returns(Task.CompletedTask);

        mockUow.Setup(u => u.LoanRepository).Returns(mockRepo.Object);
        mockUow.Setup(u => u.CompleteAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = CreateHandler(mockUow.Object);

        // Act
        var result = await handler.Handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        capturedLoan!.ApplicantName.Should().Be("JOANA SILVA");
    }

    private static (CreateLoanCommandHandler Handler, Mock<ILogger<CreateLoanCommandHandler>> Logger) CreateHandler(IUnitOfWork uow)
    {
        var logger = new Mock<ILogger<CreateLoanCommandHandler>>();
        var handler = new CreateLoanCommandHandler(uow, logger.Object);
        return (handler, logger);
    }
}