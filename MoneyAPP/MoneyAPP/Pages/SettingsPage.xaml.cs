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
    /// ち传称临
    /// </summary>
    private void Backup_Tapped(object sender, EventArgs e)
    {
        Shell.Current.CurrentItem.CurrentItem.Items.Add(new SettingsPage_Backup());
        Shell.Current.CurrentItem.CurrentItem.Items.RemoveAt(0);
    }

    /// <summary>
    /// ち传蹲EXCEL
    /// </summary>
    private void ConvertExcel_Tapped(object sender, EventArgs e)
    {
        Shell.Current.CurrentItem.CurrentItem.Items.Add(new UploadPage());
        Shell.Current.CurrentItem.CurrentItem.Items.RemoveAt(0);
    }

    /// <summary>
    /// ち传╰参戈癟
    /// </summary>
    private void SystemInfo_Tapped(object sender, EventArgs e)
    {
        Shell.Current.CurrentItem.CurrentItem.Items.Add(new SettingsPage_SystemInfo());
        Shell.Current.CurrentItem.CurrentItem.Items.RemoveAt(0);
    }

    private void Backup_Tapped(object sender, TappedEventArgs e)
    {

    }
}