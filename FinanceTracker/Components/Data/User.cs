using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Components.Data;

public class User
{
    [Key]
    public int Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
}