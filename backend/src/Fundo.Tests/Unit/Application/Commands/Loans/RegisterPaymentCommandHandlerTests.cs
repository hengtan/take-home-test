using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Fundo.Application.Commands.Loans.RegisterPayment;
using Fundo.Application.Interfaces;
using Fundo.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Fundo.Services.Tests.Unit.Application.Commands.Loans;

public class RegisterPaymentCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenLoanExistsAndPaymentIsValid()
    {
        // Arrange
        var loan = Loan.Create(1500m, 500m, "Maria Silva");
        var command = new RegisterPaymentCommand(loan.Id, 200m);

        var mockLoanRepo = new Mock<ILoanRepository>();
        mockLoanRepo
            .Setup(r => r.GetByIdAsync(loan.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(loan);

        var mockUow = new Mock<IUnitOfWork>();
        mockUow.Setup(u => u.LoanRepository).Returns(mockLoanRepo.Object);
        mockUow.Setup(u => u.CompleteAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = CreateHandler(mockUow.Object).Handler;

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        loan.CurrentBalance.Should().Be(300m);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenLoanNotFound()
    {
        // Arrange
        var fakeLoanId = Guid.NewGuid();
        var command = new RegisterPaymentCommand(fakeLoanId, 100m);

        var mockRepo = new Mock<ILoanRepository>();
        mockRepo.Setup(r => r.GetByIdAsync(fakeLoanId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Loan?)null);

        var mockUow = new Mock<IUnitOfWork>();
        mockUow.Setup(u => u.LoanRepository).Returns(mockRepo.Object);

        var handler = CreateHandler(mockUow.Object).Handler;

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be("NotFound");
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenCompleteFails()
    {
        // Arrange
        var loan = Loan.Create(1000m, 1000m, "John Doe");
        var command = new RegisterPaymentCommand(loan.Id, 300m);

        var mockRepo = new Mock<ILoanRepository>();
        mockRepo.Setup(r => r.GetByIdAsync(loan.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(loan);

        var mockUow = new Mock<IUnitOfWork>();
        mockUow.Setup(u => u.LoanRepository).Returns(mockRepo.Object);
        mockUow.Setup(u => u.CompleteAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB fail"));

        var handler = CreateHandler(mockUow.Object).Handler;

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be("Internal");
    }

    [Fact]
    public async Task Handle_Should_SetLoanStatusToPaid_WhenBalanceBecomesZero()
    {
        // Arrange
        var loan = Loan.Create(1000m, 100m, "Zero Balance");
        var command = new RegisterPaymentCommand(loan.Id, 100m);

        var mockRepo = new Mock<ILoanRepository>();
        mockRepo.Setup(r => r.GetByIdAsync(loan.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(loan);

        var mockUow = new Mock<IUnitOfWork>();
        mockUow.Setup(u => u.LoanRepository).Returns(mockRepo.Object);
        mockUow.Setup(u => u.CompleteAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = CreateHandler(mockUow.Object).Handler;

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        loan.Status.Should().Be(LoanStatus.Paid);
        loan.CurrentBalance.Should().Be(0m);
    }

    [Fact]
    public async Task Handle_Should_LogError_WhenExceptionIsThrown()
    {
        // Arrange
        var loan = Loan.Create(1000m, 1000m, "Exception Loan");
        var command = new RegisterPaymentCommand(loan.Id, 200m);

        var mockRepo = new Mock<ILoanRepository>();
        mockRepo.Setup(r => r.GetByIdAsync(loan.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(loan);

        var mockUow = new Mock<IUnitOfWork>();
        mockUow.Setup(u => u.LoanRepository).Returns(mockRepo.Object);
        mockUow.Setup(u => u.CompleteAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Any exception"));

        var handlerTuple = CreateHandler(mockUow.Object);

        // Act
        var result = await handlerTuple.Handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be("Internal");

        handlerTuple.Logger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()
            ),
            Times.AtLeastOnce()
        );
    }

    private static (RegisterPaymentCommandHandler Handler, Mock<ILogger<RegisterPaymentCommandHandler>> Logger) CreateHandler(IUnitOfWork uow)
    {
        var logger = new Mock<ILogger<RegisterPaymentCommandHandler>>();
        var handler = new RegisterPaymentCommandHandler(uow, logger.Object);
        return (handler, logger);
    }
}