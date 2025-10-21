using Fundo.Applications.Application.Common;
using Fundo.Applications.Domain.Entities;

namespace Fundo.Applications.Application.Repositories;

/// <summary>
/// Defines methods for managing <see cref="Loan"/> entities in the data store.
/// </summary>
public interface ILoanRepository
{
    /// <summary>
    /// Creates a new loan.
    /// </summary>
    /// <param name="loan">The <see cref="Loan"/> entity to create.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CreateAsync(Loan loan);

    /// <summary>
    /// Updates an existing loan.
    /// </summary>
    /// <param name="loan">The <see cref="Loan"/> entity to update.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateAsync(Loan loan);

    /// <summary>
    /// Retrieves a paginated list of loans.
    /// </summary>
    /// <param name="pageNumber">The page number (default is 1).</param>
    /// <param name="pageSize">The number of items per page (default is 10).</param>
    /// <returns>A task that returns a read-only list of <see cref="Loan"/> entities.</returns>
    Task<PagedResult<Loan>> GetAllAsync(int pageNumber = 1, int pageSize = 10);

    /// <summary>
    /// Retrieves a loan by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the loan.</param>
    /// <returns>A task that returns the <see cref="Loan"/> if found; otherwise, <c>null</c>.</returns>
    Task<Loan?> GetByIdAsync(int id);
}
