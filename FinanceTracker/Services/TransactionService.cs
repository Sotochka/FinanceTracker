using Microsoft.EntityFrameworkCore;
using FinanceTracker.Components.Data;

namespace FinanceTracker.Services;

public class TransactionService
{
    private readonly AppDbContext _context;
    public TransactionService(AppDbContext context)
    {
        _context = context;
    }
    
    //Gets user by it's ID
    public async Task<List<Transaction>> GetTransactionsByUserId(int userId)
    {
        return await _context.Transactions
            .Where(t => t.UserId == userId)
            .ToListAsync();
    }


    //Add Transaction
    public async Task AddTransaction(Transaction transaction)
    {
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTransaction(int transactionId)
    {
        var transaction = await _context.Transactions.FindAsync(transactionId);
        if (transaction != null)
        {
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
        }
    }


}
