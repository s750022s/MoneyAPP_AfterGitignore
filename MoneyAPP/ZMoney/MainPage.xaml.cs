using ZMoney.Services;

namespace ZMoney
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        private IDbServices _dbServices;

        public MainPage(IDbServices dbServices)
        {
            InitializeComponent();

            _dbServices = dbServices;
        }

        protected override void OnAppearing() 
        {
            var categary = _dbServices.GetCategoryOrderBySequence();
            LB.Text = categary.ToString();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }

}
