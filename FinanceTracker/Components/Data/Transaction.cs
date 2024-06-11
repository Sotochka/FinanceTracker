

using System.ComponentModel.DataAnnotations;

namespace FinanceTracker;

public class Transaction
{
    public int Id { get; set; }
    public int UserId { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public decimal Amount { get; set; }
    [Required]
    public string Category{ get; set; }
    [Required]
    public string Type{ get; set; }

}
