using System;
using System.Collections.Generic;

namespace FinanceManagementSystem
{
    // a. Record type for transactions
    public record Transaction(
        int Id,
        DateTime Date,
        decimal Amount,
        string Category
    );

    // b. Interface for transaction processing
    public interface ITransactionProcessor
    {
        void Process(Transaction transaction);
    }

    // c. Implementations of ITransactionProcessor
    public class BankTransferProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"[Bank Transfer] Processing {transaction.Amount:C} for {transaction.Category}");
        }
    }

    public class MobileMoneyProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"[Mobile Money] Processing {transaction.Amount:C} for {transaction.Category}");
        }
    }

    public class CryptoWalletProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"[Crypto Wallet] Processing {transaction.Amount:C} for {transaction.Category}");
        }
    }

    // d. Base Account class
    public class Account
    {
        public string AccountNumber { get; private set; }
        public decimal Balance { get; protected set; }

        public Account(string accountNumber, decimal initialBalance)
        {
            AccountNumber = accountNumber;
            Balance = initialBalance;
        }

        public virtual void ApplyTransaction(Transaction transaction)
        {
            Balance -= transaction.Amount;
            Console.WriteLine($"Transaction applied. New balance: {Balance:C}");
        }
    }

    // e. Sealed SavingsAccount class
    public sealed class SavingsAccount : Account
    {
        public SavingsAccount(string accountNumber, decimal initialBalance)
            : base(accountNumber, initialBalance) { }

        public override void ApplyTransaction(Transaction transaction)
        {
            if (transaction.Amount > Balance)
            {
                Console.WriteLine("Insufficient funds.");
            }
            else
            {
                Balance -= transaction.Amount;
                Console.WriteLine($"Transaction successful. Updated balance: {Balance:C}");
            }
        }
    }

    // f. FinanceApp class
    public class FinanceApp
    {
        private List<Transaction> _transactions = new List<Transaction>();

        public void Run()
        {
            // i. Create a SavingsAccount
            var savingsAccount = new SavingsAccount("ACC123", 1000m);

            // ii. Create sample transactions
            var transaction1 = new Transaction(1, DateTime.Now, 200m, "Groceries");
            var transaction2 = new Transaction(2, DateTime.Now, 150m, "Utilities");
            var transaction3 = new Transaction(3, DateTime.Now, 50m, "Entertainment");

            // iii. Process each transaction
            var mobileMoneyProcessor = new MobileMoneyProcessor();
            var bankTransferProcessor = new BankTransferProcessor();
            var cryptoWalletProcessor = new CryptoWalletProcessor();

            mobileMoneyProcessor.Process(transaction1);
            bankTransferProcessor.Process(transaction2);
            cryptoWalletProcessor.Process(transaction3);

            // iv. Apply each transaction to the account
            savingsAccount.ApplyTransaction(transaction1);
            savingsAccount.ApplyTransaction(transaction2);
            savingsAccount.ApplyTransaction(transaction3);

            // v. Add transactions to the list
            _transactions.Add(transaction1);
            _transactions.Add(transaction2);
            _transactions.Add(transaction3);
        }
    }

    // Main entry point
    class Program
    {
        static void Main(string[] args)
        {
            var app = new FinanceApp();
            app.Run();
        }
    }
}
