using System.Threading;
using System.Threading.Tasks;
using Fundo.Applications.Application.Repositories;
using Fundo.Applications.Application.UseCases.Loans.CreateLoan;
using Fundo.Applications.Domain.Entities;
using Fundo.Applications.Domain.Enums;
using Moq;
using Xunit;
namespace Fundo.Services.Tests.Unit.Application.UseCases;

public class CreateLoanHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnOkResult_WhenLoanIsCreated()
    {
        // Arrange
        var mockRepository = new Mock<ILoanRepository>();

        mockRepository
            .Setup(r => r.CreateAsync(It.IsAny<Loan>()))
            .Returns(Task.CompletedTask);

        var handler = new CreateLoanHandler(mockRepository.Object);

        var request = new CreateLoanRequest(
            Amount: 1000m,
            CurrentBalance: 500m,
            ApplicantName: "John Doe"
        );

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<Loan>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldSetLoanStatusToPaid_WhenCurrentBalanceIsZero()
    {
        // Arrange
        var mockRepository = new Mock<ILoanRepository>();
        Loan capturedLoan = null;

        mockRepository
            .Setup(r => r.CreateAsync(It.IsAny<Loan>()))
            .Callback<Loan>(loan => capturedLoan = loan)
            .Returns(Task.CompletedTask);

        var handler = new CreateLoanHandler(mockRepository.Object);

        var request = new CreateLoanRequest(
            Amount: 1000m,
            CurrentBalance: 0m,
            ApplicantName: "Jane Doe"
        );

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(capturedLoan);
        Assert.Equal(LoanStatus.Paid, capturedLoan!.Status);
    }

    [Fact]
    public async Task Handle_ShouldSetLoanStatusToActive_WhenCurrentBalanceIsGreaterThanZero()
    {
        // Arrange
        var mockRepository = new Mock<ILoanRepository>();
        Loan capturedLoan = null;

        mockRepository
            .Setup(r => r.CreateAsync(It.IsAny<Loan>()))
            .Callback<Loan>(loan => capturedLoan = loan)
            .Returns(Task.CompletedTask);

        var handler = new CreateLoanHandler(mockRepository.Object);

        var request = new CreateLoanRequest(
            Amount: 2000m,
            CurrentBalance: 1000m,
            ApplicantName: "Alice"
        );

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(capturedLoan);
        Assert.Equal(LoanStatus.Active, capturedLoan!.Status);
    }
}
