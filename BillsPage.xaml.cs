using FinanceTracker.Services;
using FinanceTracker.Models;
using SQLite;
using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;

namespace FinanceTracker;

public partial class BillsPage : ContentPage
{
	public ObservableCollection<Bill> Bills { get; set; } = new();
    public BillsPage()
	{
		InitializeComponent();
        //Task.Run(async () => await LoadBills());
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await DatabaseService.InitializeAsync();
        await DatabaseService.UpdateRecurringBills();

        //// Clear data for Github upload
        //await DatabaseService.ClearAllDataAsync();

        await LoadBillsAsync();
    }

    //added to change row color based on status
    public async Task LoadBillsAsync()
    {
        //var billsFromDb = await DatabaseService.Db.Table<Bill>().ToListAsync();
        var billsFromDb = await DatabaseService.GetBills(); //checking

        foreach (var bill in billsFromDb)
        {
            await bill.RefreshPaymentStatus(DatabaseService.Db); //checking

            if (bill.IsPaid)
            {
                bill.RowColor = Colors.LightGreen; // Paid bills in green
            }
            
            else
            {
                var daysUntilDue = (bill.DueDate - DateTime.Today).TotalDays;
                if (daysUntilDue < 0)
                    bill.RowColor = Colors.LightCoral;
                else if (daysUntilDue <= 5)
                    bill.RowColor = Color.FromArgb("#FFD966");
                else
                    bill.RowColor = Colors.LightBlue;
            }

        }
        var orderBills = billsFromDb.OrderBy(b => GetBillPriority(b)).ThenBy(b => b.DueDate).ToList();

        Bills.Clear();
        foreach (var bill in orderBills) //billsFromDb)
        {
            Bills.Add(bill);
        }
        UpdateTotals();
    }
    public int GetBillPriority(Bill bill)
    {
        if (!bill.IsPaid && bill.DueDate < DateTime.Today) return 0;
        if (!bill.IsPaid && (bill.DueDate - DateTime.Today).TotalDays <= 7) return 1;
        if (!bill.IsPaid) return 2;
        return 3;
    }


    public async void OnAddBillClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddBillPage());
    }

    public async void OnAddPaymentClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var bill = (Bill)button.BindingContext;
        if (bill != null)
        {
            await Navigation.PushAsync(new AddPaymentPage(bill));
        }
    }

    public async void OnEditButtonClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var bill = (Bill)button.BindingContext;
        if (bill != null)
        {
            await Navigation.PushAsync(new EditBillPage(bill));
        }
    }

    private void UpdateTotals()
    {
        decimal totalDebt = Bills.Sum(b => b.Balance);
        decimal monthlyTotal = Bills.Sum(b => b.MinimumPayment);

        TotalDebtLabel.Text = $"Total Debt: {totalDebt:C}";
        MonthlyTotalLabel.Text = $"Monthly Total: {monthlyTotal:C}";
    }

}