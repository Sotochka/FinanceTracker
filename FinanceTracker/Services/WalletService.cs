using FinanceTracker.Components.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceTracker.Services;
public class WalletService 
{
    private readonly AppDbContext _context;
    public WalletService(AppDbContext context)
    {
        _context = context;
    }
    public async Task AddWalletAsync(Wallet wallet, int userId)
    {
        wallet.UserId = userId;
        _context.Wallets.Add(wallet);
        await _context.SaveChangesAsync();
    }
    public async Task<List<Wallet>> GetWalletsByUserIdAsync(int userId)
    {
        return await _context.Wallets.Where(w => w.UserId == userId).ToListAsync();
    } 
    public async Task DeleteWalletAsync(int walletId)
    {
        var wallet = await _context.Wallets.FindAsync(walletId);
        if (wallet != null)
        {
            _context.Wallets.Remove(wallet);
            await _context.SaveChangesAsync();
        }
    }
    public async Task<decimal> GetWalletBalanceAsync(int walletId)
    {
        var transactions = await _context.Transactions
            .Where(t => t.WalletId == walletId)
            .ToListAsync();

        decimal balance = transactions
            .Where(t => t.Type == "Receive")
            .Sum(t => t.Amount) - transactions
            .Where(t => t.Type == "Send")
            .Sum(t => t.Amount);

        return balance;
    }
    public async Task UpdateWalletBalanceAsync(int walletId)
    {
        var transactions = await _context.Transactions
            .Where(t => t.WalletId == walletId)
            .ToListAsync();

        decimal balance = transactions
            .Where(t => t.Type == "Receive")
            .Sum(t => t.Amount) - transactions
            .Where(t => t.Type == "Send")
            .Sum(t => t.Amount);

        var wallet = await _context.Wallets.FindAsync(walletId);
        if (wallet != null)
        {
            wallet.Balance = balance;
            await _context.SaveChangesAsync();
        }
    }


}
