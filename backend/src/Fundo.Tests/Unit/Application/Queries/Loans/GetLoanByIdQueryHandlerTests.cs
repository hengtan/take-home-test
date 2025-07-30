using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Fundo.API.Controllers.Loans;
using Fundo.Application.Queries.Loan.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Fundo.Services.Tests.Unit.Application.Queries.Loans;

public class GetLoanByIdQueryHandlerTests
{
    [Fact]
    public async Task GetById_ShouldReturnLoan_WhenIdIsValid()
    {
        var loanId = Guid.NewGuid();
        var expectedLoan = new LoanDetailsDto { Id = loanId, ApplicantName = "Test User" };

        var mockMediator = new Mock<IMediator>();
        mockMediator.Setup(m => m.Send(It.Is<GetLoanByIdQuery>(
                q => q.Id == loanId), default))
            .ReturnsAsync(expectedLoan);

        var controller = new LoansController(mockMediator.Object);

        var result = await controller.GetById(loanId);

        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult?.StatusCode.Should().Be(200);
        okResult?.Value.Should().BeEquivalentTo(expectedLoan);
    }

    [Fact]
    public async Task GetById_Should_Return_NotFound_When_Loan_Does_Not_Exist()
    {
        // Arrange
        var loanId = Guid.NewGuid();
        var mockMediator = new Mock<IMediator>();

        mockMediator
            .Setup(m =>
                m.Send(It.Is<GetLoanByIdQuery>(q => q.Id == loanId),
                    It.IsAny<CancellationToken>()))!
            .ReturnsAsync((LoanDetailsDto?)null);

        var controller = new LoansController(mockMediator.Object);

        // Act
        var result = await controller.GetById(loanId);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task GetById_ShouldReturnInternalServerError_WhenHandlerThrows()
    {
        var loanId = Guid.NewGuid();

        var mockMediator = new Mock<IMediator>();
        mockMediator.Setup(m => m.Send(It.IsAny<GetLoanByIdQuery>(), default))
            .ThrowsAsync(new Exception("Unexpected error"));

        var controller = new LoansController(mockMediator.Object);

        Func<Task> act = async () => await controller.GetById(loanId);
        await act.Should().ThrowAsync<Exception>().WithMessage("Unexpected error");
    }

    [Fact]
    public async Task GetById_Should_Return_NotFound_When_Loan_Is_Inactive()
    {
        // Arrange
        var loanId = Guid.NewGuid();
        var mockMediator = new Mock<IMediator>();

        mockMediator
            .Setup(m => m.Send(It.IsAny<GetLoanByIdQuery>(),
                It.IsAny<CancellationToken>()))!
            .ReturnsAsync((LoanDetailsDto?)null);

        var controller = new LoansController(mockMediator.Object);

        // Act
        var result = await controller.GetById(loanId);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }
}