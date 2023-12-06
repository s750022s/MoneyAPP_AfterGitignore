namespace MoneyAPP;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(Pages.HomePage), typeof(Pages.HomePage));
        Routing.RegisterRoute(nameof(Pages.RecordAddPage), typeof(Pages.RecordAddPage));
        Routing.RegisterRoute(nameof(Pages.UploadPage), typeof(Pages.UploadPage));
        Routing.RegisterRoute(nameof(Pages.SettingsPage), typeof(Pages.SettingsPage));
        Routing.RegisterRoute(nameof(Pages.OtherPage), typeof(Pages.OtherPage));
    }
}
