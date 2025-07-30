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

namespace Fundo.Services.Tests.Unit.Application.Queries.Loans
{
    public class RegisterPaymentCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenPaymentIsValid()
        {
            // Arrange
            var loanId = Guid.NewGuid();
            var command = new RegisterPaymentCommand(loanId, 500m);

            var loan = Loan.Create( 1000m, 1000m, "Test User");

            var mockRepo = new Mock<ILoanRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(loanId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(loan);

            var mockUnit = new Mock<IUnitOfWork>();
            mockUnit.SetupGet(u => u.LoanRepository).Returns(mockRepo.Object);
            mockUnit.Setup(u => u.CompleteAsync(It.IsAny<CancellationToken>()))
                .Callback(() => Console.WriteLine("Saving..."))
                .ReturnsAsync(1);

            var logger = Mock.Of<ILogger<RegisterPaymentCommandHandler>>();
            var handler = new RegisterPaymentCommandHandler(mockUnit.Object, logger);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ShouldReturnNotFound_WhenLoanDoesNotExist()
        {
            // Arrange
            var loanId = Guid.NewGuid();
            var command = new RegisterPaymentCommand(loanId, 100m);

            var mockRepo = new Mock<ILoanRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(loanId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Loan?)null);

            var mockUnit = new Mock<IUnitOfWork>();
            mockUnit.SetupGet(u => u.LoanRepository).Returns(mockRepo.Object);

            var logger = Mock.Of<ILogger<RegisterPaymentCommandHandler>>();
            var handler = new RegisterPaymentCommandHandler(mockUnit.Object, logger);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error?.Code.Should().Be("NotFound");
        }
    }
}