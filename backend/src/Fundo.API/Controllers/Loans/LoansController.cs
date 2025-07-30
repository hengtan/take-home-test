using Fundo.Application.Commands.Loans.Create;
using Fundo.Application.Commands.Loans.RegisterPayment;
using Fundo.Application.DTOs;
using Fundo.Application.Queries.Loan.GetById;
using Fundo.Application.Queries.Loan.List;
using Fundo.Application.RegisterPaymentRequest;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fundo.API.Controllers.Loans;

[ApiController]
[Authorize]
[Route("loans")]
public class LoansController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateLoanCommand command)
    {
        var result = await mediator.Send(command);

        if (result.IsFailure)
        {
            return result.Error!.Code switch
            {
                "Validation" => BadRequest(result.Error.Message),
                "Conflict" => Conflict(result.Error.Message),
                "NotFound" => NotFound(result.Error.Message),
                _ => StatusCode(500, result.Error.Message)
            };
        }

        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<LoanDetailsDto>> GetById(Guid id)
    {
        var loan = await mediator.Send(new GetLoanByIdQuery(id));

        if (loan is null)
            return NotFound(new { error = $"Loan with ID {id} was not found." });

        return Ok(loan);
    }

    [HttpGet]
    public async Task<ActionResult<List<LoanListItemDto>>> GetAll()
    {
        var loans = await mediator.Send(new GetAllLoansQuery());
        return loans.Any() ? Ok(loans) : NoContent();
    }

    [HttpPost("{id:guid}/payment")]
    public async Task<IActionResult> RegisterPayment(Guid id, [FromBody] RegisterPaymentRequest request)
    {
        var command = new RegisterPaymentCommand(id, request.Amount);

        var result = await mediator.Send(command);

        if (result.IsFailure)
            return BadRequest(new { error = result.Error });

        return NoContent();
    }
}