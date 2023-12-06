using MoneyAPP.Services;
using MoneyAPP.Models;

namespace MoneyAPP;

public partial class App : Application
{
    public static SqliteService ServiceRepo { get; private set; }
    public static List<CategoryModel> CachedCategorys { get; set; }
    public static List<AccountModel> CachedAccounts { get; set; }

    public App(SqliteService repo)
	{
		InitializeComponent();
		MainPage = new AppShell();
        ServiceRepo = repo;
    }
}
