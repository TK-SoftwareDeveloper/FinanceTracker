using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using FinanceTracker.Models;
using FinanceTracker.Services;
namespace FinanceTracker;

public partial class AddBillPage : ContentPage
{
	public AddBillPage()
	{
		InitializeComponent();
	}
    public async void OnSaveBillClicked(object sender, EventArgs e)
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
		// Create new Bill object
		var newBill = new Bill()
		{
			Name = NameEntry.Text.Trim(),
			Balance = balance,
			MinimumPayment = minimumPayment,
			DueDate = DueDatePicker.Date,
			IsRecurring = RecurringSwitch.IsToggled,
			Notes = NotesEditor.Text?.Trim() ?? ""
		};
		try
		{
			// Save to database
			await DatabaseService.Db.InsertAsync(newBill);
			//await DisplayAlert("Success", "Bill added successfully.", "OK");
			await Navigation.PopAsync(); // Go back to the previous page
		}
		catch (Exception ex)
		{
			await DisplayAlert("Error", $"Failed to save bill: {ex.Message}", "OK");
		}
    }
	public async void OnCancelClicked(object sender, EventArgs e)
	{
		await Navigation.PopAsync(); // Go back to the previous page
    }

}