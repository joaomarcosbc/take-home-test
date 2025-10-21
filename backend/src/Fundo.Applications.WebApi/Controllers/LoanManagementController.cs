using Fundo.Applications.Application.Common;
using Fundo.Applications.Application.UseCases.Loans.CreateLoan;
using Fundo.Applications.Application.UseCases.Loans.CreateLoanPayment;
using Fundo.Applications.Application.UseCases.Loans.GetAllLoans;
using Fundo.Applications.Application.UseCases.Loans.GetLoan;
using Fundo.Applications.Packages.ResultsSerialization.Serializer;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fundo.Applications.WebApi.Controllers;

[Route("/loan")]
[ApiController]
public class LoanManagementController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly Serializer _serializer;

    public LoanManagementController(IMediator mediator, Serializer serializer)
    {
        _mediator = mediator;
        _serializer = serializer;
    }

    /// <summary>
    /// Retrieves a paginated list of loans.
    /// </summary>
    /// <param name="request">
    /// The pagination parameters encapsulated in a <see cref="GetAllLoansRequest"/> 
    /// </param>
    /// <returns>
    /// <b>200:</b> List of <see cref="GetAllLoansResponse"/>.  
    /// <b>400:</b> Invalid parameters.
    /// <b>404:</b> No records found.
    /// </returns>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<GetAllLoansResponse>), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Get([FromQuery] GetAllLoansRequest request)
    {
        var result = await _mediator.Send(request);
        return _serializer.Serialize(result);
    }

    /// <summary>
    /// Retrieves the details of a loan by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the loan.</param>
    /// <returns>
    /// <b>200:</b> Returns the loan details as a <see cref="GetLoanResponse"/>.  
    /// <b>400:</b> Invalid request.  
    /// <b>404:</b> Loan not found.
    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(GetLoanResponse), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(int id)
    {
        var getLoanRequest = new GetLoanRequest(id);
        var result = await _mediator.Send(getLoanRequest);
        return _serializer.Serialize(result);
    }

    /// <summary>
    /// Creates a new loan record.
    /// </summary>
    /// <param name="loan">
    /// The <see cref="CreateLoanRequest"/> containing the loan details.
    /// </param>
    /// <returns>
    /// <b>204:</b> Loan created successfully.
    /// <b>400:</b> Invalid loan data.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create(CreateLoanRequest loan)
    {
        var result = await _mediator.Send(loan);
        return _serializer.Serialize(result);
    }

/// <summary>
/// Records a payment for a specific loan.
/// </summary>
/// <param name="id">
/// The unique identifier of the loan to which the payment applies.
/// </param>
/// <param name="request">
/// The <see cref="CreateLoanPaymentRequest"/> containing the payment amount.
/// </param>
/// <returns>
/// <b>204:</b> Payment recorded successfully.
/// <b>400:</b> Invalid payment data (e.g., amount exceeds remaining balance).
/// <b>404:</b> Loan not found.
/// </returns>
[HttpPost("{id}/payment")]
[ProducesResponseType(204)]
[ProducesResponseType(400)]
[ProducesResponseType(404)]
public async Task<IActionResult> CreatePayment(int id, [FromBody] CreateLoanPaymentBody request)
{
    var command = new CreateLoanPaymentRequest(id, request.Amount);
    var result = await _mediator.Send(command);
    return _serializer.Serialize(result);
}
}