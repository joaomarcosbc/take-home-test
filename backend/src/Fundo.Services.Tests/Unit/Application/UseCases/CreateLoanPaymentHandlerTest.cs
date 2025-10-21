using System.Threading;
using System.Threading.Tasks;
using Fundo.Applications.Application.Repositories;
using Fundo.Applications.Application.UseCases.Loans.CreateLoanPayment;
using Fundo.Applications.Domain.Entities;
using Fundo.Applications.Domain.Enums;
using Fundo.Applications.Packages.ResultsSerialization.Errors;
using Moq;
using Xunit;

namespace Fundo.Services.Tests.Unit.Application.UseCases;

public class CreateLoanPaymentHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenLoanDoesNotExist()
    {
        // Arrange
        var loanRepoMock = new Mock<ILoanRepository>();
        var paymentRepoMock = new Mock<ILoanPaymentRepository>();

        loanRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync((Loan)null);

        var handler = new CreateLoanPaymentHandler(loanRepoMock.Object, paymentRepoMock.Object);

        var request = new CreateLoanPaymentRequest(LoanId: 1, Amount: 100);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailed);
        Assert.IsType<NotFoundError>(result.Errors[0]);
    }

    [Fact]
    public async Task Handle_ShouldReturnBadRequest_WhenLoanIsAlreadyPaid()
    {
        // Arrange
        var loanRepoMock = new Mock<ILoanRepository>();
        var paymentRepoMock = new Mock<ILoanPaymentRepository>();

        var loan = new Loan
        {
            Id = 1,
            Amount = 1000,
            CurrentBalance = 0,
            Status = LoanStatus.Paid
        };

        loanRepoMock.Setup(r => r.GetByIdAsync(loan.Id))
                    .ReturnsAsync(loan);

        var handler = new CreateLoanPaymentHandler(loanRepoMock.Object, paymentRepoMock.Object);
        var request = new CreateLoanPaymentRequest(LoanId: loan.Id, Amount: 100);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailed);
        Assert.IsType<BadRequestError>(result.Errors[0]);
    }

    [Fact]
    public async Task Handle_ShouldReturnBadRequest_WhenPaymentExceedsRemainingAmount()
    {
        // Arrange
        var loanRepoMock = new Mock<ILoanRepository>();
        var paymentRepoMock = new Mock<ILoanPaymentRepository>();

        var loan = new Loan
        {
            Id = 1,
            Amount = 1000,
            CurrentBalance = 200,
            Status = LoanStatus.Active
        };

        loanRepoMock.Setup(r => r.GetByIdAsync(loan.Id))
                    .ReturnsAsync(loan);

        var handler = new CreateLoanPaymentHandler(loanRepoMock.Object, paymentRepoMock.Object);
        var request = new CreateLoanPaymentRequest(LoanId: loan.Id, Amount: 500);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailed);
        Assert.IsType<BadRequestError>(result.Errors[0]);
    }

    [Fact]
    public async Task Handle_ShouldCreatePaymentAndUpdateLoan_WhenValidPayment()
    {
        // Arrange
        var loanRepoMock = new Mock<ILoanRepository>();
        var paymentRepoMock = new Mock<ILoanPaymentRepository>();

        var loan = new Loan
        {
            Id = 1,
            Amount = 1000,
            CurrentBalance = 400,
            Status = LoanStatus.Active
        };

        loanRepoMock.Setup(r => r.GetByIdAsync(loan.Id)).ReturnsAsync(loan);
        loanRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Loan>())).Returns(Task.CompletedTask);
        paymentRepoMock.Setup(r => r.CreateAsync(It.IsAny<LoanPayment>())).Returns(Task.CompletedTask);

        var handler = new CreateLoanPaymentHandler(loanRepoMock.Object, paymentRepoMock.Object);
        var request = new CreateLoanPaymentRequest(LoanId: loan.Id, Amount: 200);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(200, loan.CurrentBalance);
        Assert.Equal(LoanStatus.Active, loan.Status);

        loanRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Loan>()), Times.Once);
        paymentRepoMock.Verify(r => r.CreateAsync(It.IsAny<LoanPayment>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldMarkLoanAsPaid_WhenPaymentClearsBalance()
    {
        // Arrange
        var loanRepoMock = new Mock<ILoanRepository>();
        var paymentRepoMock = new Mock<ILoanPaymentRepository>();

        var loan = new Loan
        {
            Id = 1,
            Amount = 1000,
            CurrentBalance = 100,
            Status = LoanStatus.Active
        };

        loanRepoMock.Setup(r => r.GetByIdAsync(loan.Id)).ReturnsAsync(loan);
        loanRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Loan>())).Returns(Task.CompletedTask);
        paymentRepoMock.Setup(r => r.CreateAsync(It.IsAny<LoanPayment>())).Returns(Task.CompletedTask);

        var handler = new CreateLoanPaymentHandler(loanRepoMock.Object, paymentRepoMock.Object);
        var request = new CreateLoanPaymentRequest(LoanId: loan.Id, Amount: 100);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(0, loan.CurrentBalance);
        Assert.Equal(LoanStatus.Paid, loan.Status);
    }
}