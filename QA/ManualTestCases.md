# Finance Tracker - QA Documentation

## Test Report Summary
- Total Test Cases: 12
- Passed: 11
- Failed: 1
- Notes: App behaves as expected for all tested scenarios except one known issue (TC003 – recurring payments update balance but remain marked unpaid). Screenshots and test evidence included.


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
