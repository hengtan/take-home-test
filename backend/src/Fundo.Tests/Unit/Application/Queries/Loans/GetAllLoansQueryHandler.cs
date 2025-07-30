using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Fundo.API.Controllers.Loans;
using Fundo.Application.Queries.Loan.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Fundo.Services.Tests.Unit.Application.Queries.Loans;

public class GetAllLoansQueryHandler
{
    [Fact]
    public async Task GetAll_ShouldReturnLoans_WhenLoansExist()
    {
        var expectedLoans = new List<LoanListItemDto>
        {
            new() { Id = Guid.NewGuid(), ApplicantName = "User A" },
            new() { Id = Guid.NewGuid(), ApplicantName = "User B" }
        };

        var mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<GetAllLoansQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedLoans);

        var controller = new LoansController(mediator.Object);

        var result = await controller.GetAll();
        var okResult = result.Result as OkObjectResult;

        okResult.Should().NotBeNull();
        okResult?.StatusCode.Should().Be(200);
        okResult?.Value.Should().BeEquivalentTo(expectedLoans);
    }

    [Fact]
    public async Task GetAll_ShouldReturnNoContent_WhenNoLoansExist()
    {
        var mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<GetAllLoansQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var controller = new LoansController(mediator.Object);

        var result = await controller.GetAll();

        var noContent = result.Result as NoContentResult;
        noContent.Should().NotBeNull();
        noContent?.StatusCode.Should().Be(204);
    }
}