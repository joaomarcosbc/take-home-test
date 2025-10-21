using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fundo.Applications.Application.Common;
using Fundo.Applications.Application.Repositories;
using Fundo.Applications.Application.UseCases.Loans.GetAllLoans;
using Fundo.Applications.Domain.Entities;
using Fundo.Applications.Domain.Enums;
using Fundo.Applications.Packages.ResultsSerialization.Errors;
using Moq;
using Xunit;

namespace Fundo.Services.Tests.Unit.Application.UseCases;

public class GetAllLoansHandlerTest
{
    [Fact]
    public async Task Handle_ShouldReturnOkResult_WhenLoansExist()
    {
        // Arrange
        var mockRepository = new Mock<ILoanRepository>();

        var loans = new List<Loan>
        {
            new() { Id = 1, Amount = 1000, CurrentBalance = 500, ApplicantName = "Alice", Status = LoanStatus.Active },
            new() { Id = 2, Amount = 2000, CurrentBalance = 0, ApplicantName = "Bob", Status = LoanStatus.Paid }
        };

        var pagedResult = new PagedResult<Loan>(
            Items: loans,
            TotalCount: loans.Count,
            PageNumber: 1,
            PageSize: 10
        );

        mockRepository
            .Setup(r => r.GetAllAsync(1, 10))
            .ReturnsAsync(pagedResult);

        var handler = new GetAllLoansHandler(mockRepository.Object);
        var request = new GetAllLoansRequest(1, 10);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(2, result.Value.Items.Count);
        Assert.Equal(1, result.Value.PageNumber);
        Assert.Equal(10, result.Value.PageSize);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFoundError_WhenNoLoansExist()
    {
        // Arrange
        var mockRepository = new Mock<ILoanRepository>();

        var emptyPagedResult = new PagedResult<Loan>(
            Items: new List<Loan>(),
            TotalCount: 0,
            PageNumber: 1,
            PageSize: 10
        );

        mockRepository
            .Setup(r => r.GetAllAsync(1, 10))
            .ReturnsAsync(emptyPagedResult);

        var handler = new GetAllLoansHandler(mockRepository.Object);
        var request = new GetAllLoansRequest(1, 10);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailed);
        Assert.IsType<NotFoundError>(result.Errors[0]);
        Assert.Contains("No loans found", result.Errors[0].Message);
    }

    [Fact]
    public async Task Handle_ShouldMapLoansToResponseCorrectly()
    {
        // Arrange
        var mockRepository = new Mock<ILoanRepository>();

        var loan = new Loan
        {
            Id = 10,
            Amount = 1500,
            CurrentBalance = 500,
            ApplicantName = "Charlie",
            Status = LoanStatus.Active
        };

        var pagedResult = new PagedResult<Loan>(
            Items: new List<Loan> { loan },
            TotalCount: 1,
            PageNumber: 2,
            PageSize: 5
        );

        mockRepository
            .Setup(r => r.GetAllAsync(2, 5))
            .ReturnsAsync(pagedResult);

        var handler = new GetAllLoansHandler(mockRepository.Object);
        var request = new GetAllLoansRequest(2, 5);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        var response = result.Value.Items[0];

        Assert.Equal(loan.Id, response.Id);
        Assert.Equal(loan.Amount, response.Amount);
        Assert.Equal(loan.CurrentBalance, response.CurrentBalance);
        Assert.Equal(loan.ApplicantName, response.ApplicantName);
        Assert.Equal(loan.Status, response.Status);
    }
}