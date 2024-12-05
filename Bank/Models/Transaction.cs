namespace Bank.Models
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public Decimal Amount { get; set; }

        public TransactionType transactionType { get; set; }

        public string? RecipientId { get; set; }
        public Account? Recipient { get; set; }


        public string AccountId { get; set; }
        public Account Account { get; set; }
    }
}
