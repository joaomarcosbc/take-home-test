using Fundo.Applications.Application.Repositories;
using Fundo.Applications.Domain.Entities;
using Fundo.Applications.Infrastructure.DatabaseContext;

namespace Fundo.Applications.Infrastructure.Repositories;

public class LoanPaymentRepository : ILoanPaymentRepository
{
    private readonly FundoDatabaseContext _context;
    public LoanPaymentRepository(FundoDatabaseContext context)
    {
        _context = context;
    }
    public async Task CreateAsync(LoanPayment loanPayment)
    {
        await _context.AddAsync(loanPayment);
        await _context.SaveChangesAsync();
    }
}
