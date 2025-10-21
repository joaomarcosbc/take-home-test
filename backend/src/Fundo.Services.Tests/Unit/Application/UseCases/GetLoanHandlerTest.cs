using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using Fundo.Applications.Application.Repositories;
using Fundo.Applications.Application.UseCases.Loans.GetLoan;
using Fundo.Applications.Domain.Entities;
using Fundo.Applications.Domain.Enums;
using Fundo.Applications.Packages.ResultsSerialization.Errors;
using Moq;
using Xunit;

namespace Fundo.Services.Tests.Unit.Application.UseCases;

public class GetLoanHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnOkResult_WhenLoanExists()
    {
        // Arrange
        var mockRepository = new Mock<ILoanRepository>();

        var loan = new Loan
        {
            Id = 1,
            Amount = 1000m,
            CurrentBalance = 500m,
            ApplicantName = "Alice",
            Status = LoanStatus.Active,
            DateCreated = DateTime.UtcNow,
            DateModified = null,
            Payments = new List<LoanPayment>
            {
                new() { Amount = 500m, DateCreated = DateTime.UtcNow.AddDays(-1) }
            }
        };

        mockRepository
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(loan);

        var handler = new GetLoanHandler(mockRepository.Object);
        var request = new GetLoanRequest(1);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(loan.Amount, result.Value.Amount);
        Assert.Equal(loan.CurrentBalance, result.Value.CurrentBalance);
        Assert.Equal(loan.ApplicantName, result.Value.ApplicantName);
        Assert.Equal(loan.Status, result.Value.Status);
        Assert.Equal(loan.Payments.Count, result.Value.Payments.Count());
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFoundError_WhenLoanDoesNotExist()
    {
        // Arrange
        var mockRepository = new Mock<ILoanRepository>();
        mockRepository
            .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Loan)null);

        var handler = new GetLoanHandler(mockRepository.Object);
        var request = new GetLoanRequest(99);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailed);
        Assert.IsType<NotFoundError>(result.Errors[0]);
        Assert.Contains("No loan found", result.Errors[0].Message);
    }

    [Fact]
    public async Task Handle_ShouldMapLoanToResponseCorrectly()
    {
        // Arrange
        var mockRepository = new Mock<ILoanRepository>();

        var creationDate = DateTime.UtcNow.AddDays(-10);
        var updateDate = DateTime.UtcNow;

        var loan = new Loan
        {
            Id = 10,
            Amount = 2000m,
            CurrentBalance = 1000m,
            ApplicantName = "Bob",
            Status = LoanStatus.Active,
            DateCreated = creationDate,
            DateModified = updateDate,
            Payments = new List<LoanPayment>
            {
                new() { Amount = 500m, DateCreated = DateTime.UtcNow.AddDays(-7) },
                new() { Amount = 500m, DateCreated = DateTime.UtcNow.AddDays(-3) }
            }
        };

        mockRepository
            .Setup(r => r.GetByIdAsync(10))
            .ReturnsAsync(loan);

        var handler = new GetLoanHandler(mockRepository.Object);
        var request = new GetLoanRequest(10);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        var response = result.Value;

        Assert.Equal(loan.Amount, response.Amount);
        Assert.Equal(loan.CurrentBalance, response.CurrentBalance);
        Assert.Equal(loan.ApplicantName, response.ApplicantName);
        Assert.Equal(loan.Status, response.Status);
        Assert.Equal(loan.DateCreated, response.DateCreated);
        Assert.Equal(loan.DateModified, response.DateUpdated);
        Assert.Equal(loan.Payments.Count, response.Payments.Count());
    }
}
