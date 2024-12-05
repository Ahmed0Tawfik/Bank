using Microsoft.AspNetCore.Identity;

namespace Bank.Models
{
    public class Account : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public int UniqueId { get; set; }   

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public ICollection<LoanRequest> LoanRequests { get; set; } = new List<LoanRequest>();

    }
}
