using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.Models
{
    public class Payment
    {
        [PrimaryKey, AutoIncrement]
        public int PaymentId { get; set; }

        public int BillId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }

        public decimal BalanceAfterPayment { get; set; }


        public async Task RefreshPayments(SQLiteAsyncConnection db)
        {
            var payments = await db.Table<Payment>()
                .Where(p => p.BillId == BillId)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
            var recentPayment = payments.FirstOrDefault(p => p.PaymentDate.Month == DateTime.Now.Month && p.PaymentDate.Year == DateTime.Now.Year);
            
        }
    }
}
