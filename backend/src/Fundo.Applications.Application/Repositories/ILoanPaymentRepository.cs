using Fundo.Applications.Domain.Entities;

namespace Fundo.Applications.Application.Repositories;

/// <summary>
/// Defines methods for managing <see cref="LoanPayment"/> entities in the data store.
/// </summary>
public interface ILoanPaymentRepository
{
    /// <summary>
    /// Creates a new loan payment.
    /// </summary>
    /// <param name="loanPayment">The <see cref="LoanPayment"/> entity to create.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CreateAsync(LoanPayment loanPayment);
}