using FinanceTracker.Models;
using FinanceTracker.Services;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Javax.Security.Auth;

namespace FinanceTracker;

public partial class AddPaymentPage : ContentPage
{
	private Bill _bill;
    private Payment _selectedPayment; 

    //checking if this will work
    public ObservableCollection<Payment> Payments { get; set; } = new();

    public AddPaymentPage(Bill bill)
	{
		InitializeComponent();
		_bill = bill;

        BindingContext = this;

        BillNameLabel.Text = _bill.Name;
        CurrentBalanceLabel.Text = $"Current Balance: {_bill.Balance:C}";
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadPaymentsAsync();
    }

    public async void OnSavePaymentClicked(object sender, EventArgs e)
	{
		if (decimal.TryParse(PaymentAmountEntry.Text, out decimal paymentAmount) && paymentAmount > 0)
		{
			try
			{
                _bill.Balance -= paymentAmount;

                var payment = new Payment
				{
					BillId = _bill.BillId,
					Amount = paymentAmount,
                    //PaymentDate = PaymentDatePicker.Date,
                    PaymentDate = (PaymentDatePicker.Date == DateTime.Today) ? DateTime.Now : PaymentDatePicker.Date,
                    BalanceAfterPayment = _bill.Balance
                };

                await DatabaseService.AddPayment(payment);

                // Handle recurring bill logic
                if (_bill.IsRecurring && _bill.Balance <= 0)
                {
                    // Bill is fully paid, move due date forward and reset balance
                    _bill.DueDate = _bill.DueDate.AddMonths(1);
                    _bill.Balance = _bill.MinimumPayment; // reset for next month
                }
                await DatabaseService.Db.UpdateAsync(_bill);

                //await DisplayAlert("Success", "Payment added successfully.", "OK");

                //LoadPayments();
                await LoadPaymentsAsync();

                PaymentAmountEntry.Text = string.Empty;
                PaymentAmountEntry.Unfocus();
                PaymentDatePicker.Date = DateTime.Today; // optional, reset date too

            }

            catch (Exception ex)
			{
				await DisplayAlert("Error", $"Failed to add payment: {ex.Message}", "OK");
            }
           
        }
		else
		{
			await DisplayAlert("Error", "Please enter a valid payment amount.", "OK");
        }
    }

    public async Task LoadPaymentsAsync()
    {
        var paymentsFromDb = await DatabaseService.GetPaymentsForBill(_bill.BillId);

        var orderedPayments = paymentsFromDb
        .OrderByDescending(p => p.PaymentDate)
        .ThenByDescending(p => p.PaymentId)
        .ToList();

        Payments.Clear(); // remove old items

        foreach (var payment in paymentsFromDb)
        {
            Payments.Add(payment);
        }
        PaymentAmountEntry.Text = string.Empty;

        CurrentBalanceLabel.Text = $"Current Balance: {_bill.Balance:C}";
    }

    public async void OnEditPaymentClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var selectedPayment = button.BindingContext as Payment;

        if (selectedPayment != null)
        {
            // Navigate to EditPaymentPage using the tracked payment
            await Navigation.PushAsync(new EditPaymentPage(selectedPayment, _bill));
        }
    }

    public async void OnCancelClicked(object sender, EventArgs e)
	{
		await Navigation.PopAsync();
    }

}