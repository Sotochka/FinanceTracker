namespace FinanceTracker;

public class NewTransaction
{
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public string Category { get; set; }
    public int WalletId { get; set; }
    public string Type { get; set; }
}
