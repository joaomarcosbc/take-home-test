using System;
using System.Threading.Tasks;
using Fundo.Applications.Application.UseCases.Loans.CreateLoan;
using Fundo.Applications.Application.UseCases.Loans.CreateLoanPayment;
using Fundo.Applications.Application.UseCases.Loans.GetLoan;
using Fundo.Applications.Domain.Entities;
using Fundo.Applications.Domain.Enums;
using Fundo.Applications.Infrastructure.DatabaseContext;
using Fundo.Applications.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Fundo.Services.Tests.Integration.Fundo.Applications.WebApi.Controllers;

public class LoanManagementControllerIntegrationTests
{
    private readonly FundoDatabaseContext _context;
    private readonly LoanManagementControllerStub _controller;

    public LoanManagementControllerIntegrationTests()
    {
        var options = new DbContextOptionsBuilder<FundoDatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new FundoDatabaseContext(options);

        var loanRepo = new LoanRepository(_context);
        var paymentRepo = new LoanPaymentRepository(_context);

        _controller = new LoanManagementControllerStub(loanRepo, paymentRepo);
    }

    [Fact]
    public async Task POST_CreateLoan_ShouldReturnNoContent()
    {
        var request = new CreateLoanRequest(1000, 1000, "Maria");

        var result = await _controller.Create(request);

        Assert.IsType<NoContentResult>(result);
        Assert.Single(_context.Loans);
    }

    [Fact]
    public async Task GET_LoanById_ShouldReturnOk_WhenLoanExists()
    {
        var loan = new Loan { Amount = 1000, CurrentBalance = 1000, ApplicantName = "Maria", Status = LoanStatus.Active };
        _context.Loans.Add(loan);
        await _context.SaveChangesAsync();

        var result = await _controller.GetById(loan.Id);

        var ok = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<GetLoanResponse>(ok.Value);
        Assert.Equal("Maria", response.ApplicantName);
    }
}