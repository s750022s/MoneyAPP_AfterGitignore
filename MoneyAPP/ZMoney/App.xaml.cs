using ZMoney.Models;
namespace ZMoney
{
    public partial class App : Application
    {
        /// <summary>
        /// 緩存的類別清單
        /// </summary>
        public static List<CategoryModel> CachedCategorys { get; set; } = null!;

        /// <summary>
        /// 緩存的帳戶清單
        /// </summary>
        public static List<AccountModel> CachedAccounts { get; set; } = null!;

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
