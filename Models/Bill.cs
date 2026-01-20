using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.Models
{
    public class Bill
    {
        [PrimaryKey, AutoIncrement]
        public int BillId { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public decimal MinimumPayment { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsRecurring { get; set; } 
        public string RecurringMonths { get; set; } //Monthly
        public bool IsPaid { get { return Balance <= 0; } }   
        public string Notes { get; set; }

        [Ignore]
        public Color RowColor { get; set; } // New property for row color


        // Computed property to determine if the bill is paid, overdue, or due soon
        public async Task RefreshPaymentStatus(SQLiteAsyncConnection db)
        {
            var payments = await db.Table<Payment>()
                .Where(p => p.BillId == BillId)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();

            var recentPayment = payments.FirstOrDefault(p => p.PaymentDate.Month == DateTime.Now.Month && p.PaymentDate.Year == DateTime.Now.Year);

        }
    }


}
