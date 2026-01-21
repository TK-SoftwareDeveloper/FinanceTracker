# Finance Tracker - QA Documentation

## Test Report Summary
- Total Test Cases: 12
- Passed: 12
- Failed: 0
- Notes: App behaves as expected. All functional and UI requirements are met.

---

## Bills Functionality

| ID    | Feature / Scenario | Steps | Expected Result | Actual Result / Pass-Fail |
|-------|------------------|-------|----------------|---------------------------|
| TC001 | Add a new bill | Click "Add Bill", enter info, Save | Bill appears in list | Matches expected → Pass |
| TC004 | Edit a bill | Select "Edit Bill" → Edit → Save | Bill updates correctly | Matches expected → Pass |
| TC006 | Delete a bill | Select "Edit Bill" on chosen bill → click "Delete" | Bill removed from list; totals updated | Matches expected → Pass |

---

## Payments Functionality

| ID    | Feature / Scenario | Steps | Expected Result | Actual Result / Pass-Fail |
|-------|------------------|-------|----------------|---------------------------|
| TC002 | Add a new payment (non-recurring bill) | Click "Add Payment" on chosen non-recurring bill, enter payment amount and date, click "Save" | Payment is recorded and bill is marked as paid | Payment is recorded; Bill marked as paid | Matches expected → Pass |
| TC003 | Add a new payment (recurring bill) | Click "Add Payment" on chosen recurring bill, enter payment amount and date, click "Save" | Payment recorded; totals updated | Payment is recorded and the balance updates, but the bill remains marked as unpaid in the bills list | Fail |
| TC005 | Edit a payment | Select "Add Payment" on chosen bill → Edit → click "Save" | Payment updates correctly | Matches expected → Pass |

---

## Additional Functionality

| ID    | Feature / Scenario | Steps | Expected Result | Actual Result / Pass-Fail |
|-------|------------------|-------|----------------|---------------------------|
| TC007 | Bill totals | Add/edit/delete bills and payments | Totals accurate | Matches expected → Pass |
| TC008 | Invalid data handling | Enter negative/empty data | Error prevents invalid input | Matches expected → Pass |
| TC009 | Recurring bills | Toggle recurring option | Bill marked as recurring | Matches expected → Pass |
| TC010 | Bill color coding | Add bills with different due statuses | Colors appear correctly | Matches expected → Pass |
| TC011 | Database persistence | Close & reopen app | Data persists correctly | Matches expected → Pass |
| TC012 | UI responsiveness | Click buttons, scroll list | UI behaves correctly | Matches expected → Pass |

## Known Issues
- Recurring bills remain marked as unpaid after adding a payment. This does not affect payment recording or totals but may confuse users.

## Known Limitations / Enhancements
- Payments cannot currently be deleted through the UI. Once created, a payment remains in the system unless removed directly from the database.

## Test Screenshots
**TC001 - Add a New Bill**
![Add a new bill](TestScreenshots/TC001_AddBill.JPG)

**TC002 - Add a New Payment (Non-Recurring Bill)**
![Add a new payment (non-recurring bill)](TestScreenshots/TC002_Payment_NonRecurring.JPG)

**TC003 - Add a New Payment (Recurring Bill)**
![Add a new payment (recurring bill)](TestScreenshots/TC003_RecurringPayment_PaymentRecorded.JPG)
![Add a new payment (recurring bill) - Bill Still Unpaid](TestScreenshots/TC003_RecurringPayment_BalanceUpdated_BillMarkedUnpaid.JPG)]

**TC004 - Edit a Bill**
![Edit a bill](TestScreenshots/TC004_EditBill.JPG)

**TC005 - Edit a Payment**
![Edit a payment](TestScreenshots/TC005_EditPayment.JPG)

**TC006 - Delete a Bill**
![Delete a bill](TestScreenshots/TC006_DeleteBill.JPG)

**TC008 - Invalid Data Handling**
![Invalid data handling](TestScreenshots/TC008_InvalidDataHandling.JPG)

**TC010 - Bill Color Coding**
![Bill color coding](TestScreenshots/TC010_BillColorCoding.JPG)