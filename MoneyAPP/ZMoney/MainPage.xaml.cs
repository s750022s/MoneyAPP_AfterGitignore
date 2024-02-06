using ZMoney.Services;
using ZMoney.Models;


namespace ZMoney
{
    public partial class MainPage : ContentPage
    {
        private DbManager _dbManager;

        public MainPage(DbManager dbManager)
        {
            InitializeComponent();

            _dbManager = dbManager;
        }

        protected override void OnAppearing() 
        {
            changePage();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            var test = new RecordModel()
            {
                Id = 10,
                RecordDateTime = DateTime.Now,
                IsExpenses = true,
                AccountId = 2,
                CategoryId = 1,
                Description = "AAA",
                AmountOfMoney = 1000,
                IsDelete = false
            };
            _dbManager.AddRecord(test);
            changePage();
        }

        private void changePage() 
        {
            var tests = _dbManager.GetTotalByIsExpenses(new DateTime(2024,2,6,0,0,0), new DateTime(2024, 2, 7, 0, 0, 0));
            //foreach (var test in tests)
            //{
            //    string lb = string.Join
            //    (
            //        "\n",
            //        test.RecordDateTime.ToString("yyyy-MM-dd hh:mm tt"),
            //        test.IsExpenses.ToString(),
            //        test.AccountId.ToString(),
            //        test.CategoryId.ToString(),
            //        test.Description,
            //        test.AmountOfMoney.ToString(),
            //        test.IsDelete.ToString()
            //    );

            //}


            string lb = "";
            foreach (var item in tests)
            {
                lb += string.Join("\n", item.ToString());
            }
            LB.Text = lb;
        }
    }

}
