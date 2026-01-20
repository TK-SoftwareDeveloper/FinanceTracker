using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using FinanceTracker.Models;

namespace FinanceTracker.Services
{
    public static class DatabaseService
    {
        // This class will handle all database operations such as connecting to the database,
        // executing queries, and managing transactions.    
        private static SQLiteAsyncConnection _db;

        public static SQLiteAsyncConnection Db => _db;

        // Initialize database
        public static async Task InitializeAsync()
        {
            if (_db != null)
                return;

            var databasePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "finance_tracker.db3");

            _db = new SQLiteAsyncConnection(databasePath);

            // ---------- Developer Notes ----------
            // The following code drops the tables completely. 
            // It was used during development/testing to reset the database schema.
            // It is now commented out to prevent accidental data loss.
            // For QA purposes, we typically clear data using ClearAllDataAsync() instead of dropping tables.
            // Keeping this here as a reference for future schema resets if needed.
            // Developer: Tiffany K.

            //await Db.DropTableAsync<Bill>();
            //await Db.DropTableAsync<Payment>();

            // Create tables
            await _db.CreateTableAsync<Bill>();
            await _db.CreateTableAsync<Payment>(); 
        }

        // ---------- QA / Developer Notes ----------
        // This method clears all data from the database tables (Bill and Payment) 
        // without dropping the tables themselves. 
        // It is intended for testing and QA purposes to ensure a clean, repeatable 
        // starting state for the application.
        //
        // Usage Notes:
        // - Call this method temporarily when you want an empty database for testing.
        // - Do NOT call this automatically in production, as it will delete all user data.
        // - The database schema remains intact, which follows QA best practices.
        //
        // Developer: Tiffany K.
        public static async Task ClearAllDataAsync()
        {
            if (_db == null)
                await InitializeAsync();

            // Delete all rows from Payments first (foreign key safety)
            await _db.ExecuteAsync("DELETE FROM Payment");

            // Delete all rows from Bills
            await _db.ExecuteAsync("DELETE FROM Bill");
        }

        //To insert a new bill into the database
        public static Task AddBill(Bill bill)
        {
            return _db.InsertAsync(bill);
        }

        //To retrieve all bills from the database
        public static Task<List<Bill>> GetBills()
        {
            return _db.Table<Bill>().ToListAsync();
        }

        public static async Task UpdateRecurringBills()
        {
            var bills = await _db.Table<Bill>()
                                 .Where(b => b.IsRecurring)
                                 .ToListAsync();

            foreach (var bill in bills)
            {
                // Move the due date forward until it's >= today
                while (bill.DueDate < DateTime.Today)
                {
                    // Roll over unpaid balance into the next month
                    if (bill.Balance > 0)
                        bill.Balance += bill.MinimumPayment;
                    else
                        bill.Balance = bill.MinimumPayment;

                    bill.DueDate = bill.DueDate.AddMonths(1);
                }

                await _db.UpdateAsync(bill);
            }
        }


        //To insert a new payment into the database
        public static Task AddPayment(Payment payment)
        {
            return _db.InsertAsync(payment);
        }
        //To update a payment for a specific bill
        public static Task UpdatePayment(Payment payment)
        {
            return _db.UpdateAsync(payment);
        }
        //To retrieve all payments for a specific bill
        public static Task<List<Payment>> GetPaymentsForBill(int billId)
        {
            try
            {
                return _db.Table<Payment>()
                          .Where(p => p.BillId == billId)
                          .OrderByDescending(p => p.PaymentDate)
                          .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving payments for bill {billId}: {ex.Message}");
                return Task.FromResult(new List<Payment>());
            }
        }
    }
}
