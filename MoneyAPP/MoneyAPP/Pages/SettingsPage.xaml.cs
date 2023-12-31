using System;
using MoneyAPP.Models;
using MoneyAPP.Services;

namespace MoneyAPP.Pages;

public partial class SettingsPage : ContentPage
{
	public SettingsPage()
	{
		InitializeComponent();
	}

    /// <summary>
    /// 切換備份還原頁
    /// </summary>
    private void Backup_Tapped(object sender, EventArgs e)
    {
        Shell.Current.CurrentItem.CurrentItem.Items.Add(new SettingsPage_Backup());
        Shell.Current.CurrentItem.CurrentItem.Items.RemoveAt(0);
    }

    /// <summary>
    /// 切換匯出EXCEL頁
    /// </summary>
    private void ConvertExcel_Tapped(object sender, EventArgs e)
    {
        Shell.Current.CurrentItem.CurrentItem.Items.Add(new UploadPage());
        Shell.Current.CurrentItem.CurrentItem.Items.RemoveAt(0);
    }

    /// <summary>
    /// 切換系統資訊頁
    /// </summary>
    private void SystemInfo_Tapped(object sender, EventArgs e)
    {
        Shell.Current.CurrentItem.CurrentItem.Items.Add(new SettingsPage_SystemInfo());
        Shell.Current.CurrentItem.CurrentItem.Items.RemoveAt(0);
    }
}