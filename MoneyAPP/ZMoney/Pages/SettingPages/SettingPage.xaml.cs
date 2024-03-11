namespace ZMoney.Pages;

/// <summary>
/// 設定主頁
/// </summary>
public partial class SettingPage : ContentPage
{
    public SettingPage()
    {
        InitializeComponent();
    }

    /// <summary>
    /// 前往選單編輯
    /// </summary>
    private void ListSetting_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("Setting/ListSetting");
    }

    /// <summary>
    /// 前往還原
    /// </summary>
    private void Backup_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("Setting/BackupAndReset");
    }

    /// <summary>
    /// 前往APP資訊
    /// </summary>
    private void SystemInfo_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("Setting/AppInfo");
    }
}