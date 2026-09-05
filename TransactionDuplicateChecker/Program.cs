using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionDuplicateChecker
{
    internal class Program
    {
        public class Transaction
        {
            public string Id { get; set; } = string.Empty;
            public decimal Amount { get; set; }
            public DateTime Timestamp { get; set; }
            public string AccountId { get; set; } = string.Empty;
        }

        public class DuplicateChecker
        {
            private readonly HashSet<string> _seenTransactionIds = new HashSet<string>();
            private readonly Dictionary<string, Transaction> _suspiciousTransactions = new Dictionary<string, Transaction>();

            public void ProcessTransactions(List<Transaction> transactions) 
            {
                foreach (var transaction in transactions)
                {
                    try
                    {
                        if (!_seenTransactionIds.Add(transaction.Id))
                        {
                            Console.WriteLine($"[DUPLICATE DETECTED] Transaction ID: {transaction.Id}");
                            _suspiciousTransactions[transaction.Id] = transaction;
                        }
                        else
                        {
                            Console.WriteLine($"[VALID] Transaction ID: {transaction.Id} processed successfully.");
                        }
                    }   
                    catch (Exception ex)  
                    {
                        Console.WriteLine($"Error processing transaction {transaction.Id}: {ex.Message}");
                    }
                }
            }
            public Dictionary<string, Transaction> GetSuspiciousTransactions()
            {
                return _suspiciousTransactions;
            }
        }
        static void Main(string[] args)
        {
            List<Transaction> transactions = new List<Transaction>
            {
                new Transaction { Id = "TX1001", Amount = 150_50m, Timestamp = DateTime.Now, AccountId = "ACC-01" },
                new Transaction { Id = "TX1002", Amount = 200_00m, Timestamp = DateTime.Now, AccountId = "ACC-02" },
                new Transaction { Id = "TX1001", Amount = 150_50m, Timestamp = DateTime.Now, AccountId = "ACC-01" },
                new Transaction { Id = "TX1003", Amount = 89_99m, Timestamp = DateTime.Now, AccountId = "ACC-03" },
                new Transaction { Id = "TX1002", Amount = 200_00m, Timestamp = DateTime.Now, AccountId = "ACC-02" }
            };

            DuplicateChecker checker = new DuplicateChecker();
            checker.ProcessTransactions(transactions);

            Console.WriteLine($"\nTotal suspicious/duplicate transactions caught: {checker.GetSuspiciousTransactions().Count}");
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadLine();
        }
    }
}
