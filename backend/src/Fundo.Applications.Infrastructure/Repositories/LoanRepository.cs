using Fundo.Applications.Application.Repositories;
using Fundo.Applications.Domain.Entities;
using Fundo.Applications.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace Fundo.Applications.Infrastructure.Repositories;

public class LoanRepository : ILoanRepository
{
    private readonly FundoDatabaseContext _context;
    public LoanRepository(FundoDatabaseContext context)
    {
        _context = context;
    }
    public async Task CreateAsync(Loan loan)
    {
        await _context.AddAsync(loan);
        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<Loan>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;

        return await _context.Loans
            .AsNoTracking()
            .OrderBy(l => l.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Loan?> GetByIdAsync(int id)
    {
        return await _context.Loans
            .AsNoTracking()
            .Include(l => l.Payments)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task UpdateAsync(Loan loan)
    {
        _context.Entry(loan).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
}