using FinanceTracker.Models;
using FinanceTracker.Services;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker;

public partial class EditPaymentPage : ContentPage
{

    private Payment _payment;
	private Bill _bill;
    public EditPaymentPage(Payment paymenttoEdit, Bill billtoEdit)
	{
		InitializeComponent();
		_payment = paymenttoEdit;
		_bill = billtoEdit;

        //BillNameLabel.Text = _bill.Name;
        PaymentAmountEntry.Text = _payment.Amount.ToString("F2");
        PaymentDatePicker.Date = _payment.PaymentDate;
    }

    public async void OnSaveClicked(object sender, EventArgs e)
    {
        if (!decimal.TryParse(PaymentAmountEntry.Text, out decimal newPaymentAmount) || newPaymentAmount <= 0)
        {
            await DisplayAlert("Error", "Please enter a valid payment amount.", "OK");
            return;
        }

        // Calculate the difference
        decimal amountDifference = newPaymentAmount - _payment.Amount;

        // Update bill balance
        _bill.Balance -= amountDifference;

        // Update payment
        _payment.Amount = newPaymentAmount;
        _payment.PaymentDate = PaymentDatePicker.Date;
        _payment.BalanceAfterPayment = _bill.Balance;

        //await DisplayAlert("Payment Update", $"Updating PaymentId {_payment.PaymentId}\nAmount: {_payment.Amount}\nDate: {_payment.PaymentDate:MM/dd/yyyy}", "OK");
        try
        {

            await DatabaseService.UpdatePayment(_payment);

            await DatabaseService.Db.UpdateAsync(_bill);    // UPDATE bill

            // Go back to AddPaymentPage
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to update payment: {ex.Message}", "OK");
        }
    }


    public async void OnCancelClicked(object sender, EventArgs e)
	{ 		
		await Navigation.PopAsync(); // Go back to the previous page
    }

	
}