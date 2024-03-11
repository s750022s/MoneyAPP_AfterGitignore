using ZMoney.Models;
namespace ZMoney
{
    public partial class App : Application
    {
        public static List<CategoryModel> CachedCategorys { get; set; } = null!;
        public static List<AccountModel> CachedAccounts { get; set; } = null!;

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
