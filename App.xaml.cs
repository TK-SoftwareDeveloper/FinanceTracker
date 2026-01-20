namespace FinanceTracker
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new BillsPage());
        }
    }
}
