namespace Bank.Models
{
    public class LoanRequest
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public Decimal Amount { get; set; }

        public LoanStatus Status { get; set; }

        public string AccountId { get; set; }
        public Account Account { get; set; }
    }
}
