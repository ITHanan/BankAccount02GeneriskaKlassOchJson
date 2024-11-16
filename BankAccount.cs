

namespace BankAccount02GeneriskaKlassOchJson
{
    public class BankAccount : IIdentifiable
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public string AccountType { get; set; }

        public decimal Balance { get; set; }

        public List<Transaction> Transactions { get; set; } = new List<Transaction>();


        public BankAccount(int id, int customerId, string accountType, decimal balance)
        {
            Id = id;
            CustomerId = customerId;
            AccountType = accountType;
            Balance = balance;

        }
    }
}
