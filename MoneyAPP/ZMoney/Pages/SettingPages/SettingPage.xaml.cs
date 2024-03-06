using ZMoney.Controls;
namespace ZMoney.Pages;

public partial class SettingPage : ContentPage
{
	public SettingPage()
	{
		InitializeComponent();


    }

    private void ListSetting_Clicked(object sender, EventArgs e) 
    {
        Shell.Current.GoToAsync("Setting/ListSetting");
    }


    private void Backup_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("Setting/BackupAndReset");
    }

    private void SystemInfo_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("Setting/AppInfo");
    }

}