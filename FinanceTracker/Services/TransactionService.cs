﻿using Microsoft.EntityFrameworkCore;
using FinanceTracker.Components.Data;

namespace FinanceTracker.Services;

public class TransactionService
{
    private readonly AppDbContext _context;
    private readonly WalletService _walletService;
    public TransactionService(AppDbContext context, WalletService walletService)
    {
        _context = context;
        _walletService = walletService;
    }   
    
    //Gets user by its ID
    public async Task<List<Transaction>> GetTransactionsByUserId(int userId)
    {
        return await _context.Transactions
            .Where(t => t.UserId == userId)
            .ToListAsync();
    }


    //Add Transaction
    public async Task AddTransaction(Transaction transaction, int walletId, int userId)
    {
        transaction.WalletId = walletId;
        transaction.UserId = userId;
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
        await _walletService.UpdateWalletBalanceAsync(walletId);
    }

    public async Task DeleteTransaction(int transactionId)
    {
        var transaction = await _context.Transactions.FindAsync(transactionId);
        if (transaction != null)
        {
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            await _walletService.UpdateWalletBalanceAsync(transaction.WalletId);
        }
    }
    public async Task<List<Transaction>> GetTransactionsByWalletIdAsync(int walletId)
    {
        return await _context.Transactions.Where(t => t.WalletId == walletId).ToListAsync();
    }

}
