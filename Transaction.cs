

using System.Data;

namespace BankAccount02GeneriskaKlassOchJson
{
    public class Transaction
    {

        public int TransactionId { get; set; }
        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

        public string Type { get; set; } // Deposit, Withdrawal, Transfer

        
    }
}
