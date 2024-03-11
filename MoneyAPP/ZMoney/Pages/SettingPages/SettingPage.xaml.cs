namespace ZMoney.Pages;

/// <summary>
/// �]�w�D��
/// </summary>
public partial class SettingPage : ContentPage
{
    public SettingPage()
    {
        InitializeComponent();
    }

    /// <summary>
    /// �e�����s��
    /// </summary>
    private void ListSetting_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("Setting/ListSetting");
    }

    /// <summary>
    /// �e���٭�
    /// </summary>
    private void Backup_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("Setting/BackupAndReset");
    }

    /// <summary>
    /// �e��APP��T
    /// </summary>
    private void SystemInfo_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("Setting/AppInfo");
    }
}