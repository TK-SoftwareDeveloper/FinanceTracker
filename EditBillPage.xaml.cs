using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using FinanceTracker.Models;
using FinanceTracker.Services;

namespace FinanceTracker;


public partial class EditBillPage : ContentPage
{
	private Bill _bill;
	public EditBillPage(Bill billtoEdit)
	{
		InitializeComponent();
		_bill = billtoEdit;

		NameEntry.Text = _bill.Name;
		BalanceEntry.Text = _bill.Balance.ToString();
		MinimumPaymentEntry.Text = _bill.MinimumPayment.ToString();
		DueDatePicker.Date = _bill.DueDate;
		RecurringSwitch.IsToggled = _bill.IsRecurring;
		NotesEditor.Text = _bill.Notes;
	}

	public async void OnSaveChangesClicked(object sender, EventArgs e)
	{
        // Validate inputs
        if (string.IsNullOrWhiteSpace(NameEntry.Text) ||
            string.IsNullOrWhiteSpace(BalanceEntry.Text) ||
            string.IsNullOrWhiteSpace(MinimumPaymentEntry.Text) ||
            DueDatePicker.Date == null)
        {
            await DisplayAlert("Error", "Please fill in all required fields.", "OK");
            return;
        }
        if (!decimal.TryParse(BalanceEntry.Text, out decimal balance) ||
            !decimal.TryParse(MinimumPaymentEntry.Text, out decimal minimumPayment))
        {
            await DisplayAlert("Error", "Please enter valid numbers for Balance and Minimum Payment.", "OK");
            return;
        }

        _bill.Name = NameEntry.Text;
        _bill.Balance = balance;
        _bill.MinimumPayment = minimumPayment;
        _bill.DueDate = DueDatePicker.Date;
        _bill.IsRecurring = RecurringSwitch.IsToggled;
        _bill.Notes = NotesEditor.Text;

        try
        {
            await DatabaseService.Db.UpdateAsync(_bill);

            //await DisplayAlert("Success", "Bill updated successfully.", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex) 
        {
            await DisplayAlert("Error", $"Failed to update bill: {ex.Message}", "OK");
        }
    }

    public async void OnDeleteClicked(object sender, EventArgs e)
    {
        var confirm = await DisplayAlert("Confirm Delete", "Are you sure you want to delete this bill?", "Yes", "No");
        if (confirm)
        {
            try
            {
                await DatabaseService.Db.DeleteAsync(_bill);
                //await DisplayAlert("Success", "Bill deleted successfully.", "OK");
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to delete bill: {ex.Message}", "OK");
            }
        }
    }


    public async void OnCancelClicked(object sender, EventArgs e)
	{
        await Navigation.PopAsync(); // Go back to the previous page
    }

}