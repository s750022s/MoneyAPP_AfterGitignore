using ZMoney.Services;

namespace ZMoney
{
    public partial class App : Application
    {
        public static IDbServices DbServices { get; private set; }

        public App(IDbServices sqlite)
        {
            InitializeComponent();

            MainPage = new AppShell();
            DbServices = sqlite;
        }
    }
}
