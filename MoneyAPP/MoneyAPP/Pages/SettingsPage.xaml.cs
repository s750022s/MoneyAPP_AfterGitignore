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

    private void Backup_Tapped(object sender, EventArgs e)
    {
        Shell.Current.CurrentItem.CurrentItem.Items.Add(new SettingsPage_Backup());
        Shell.Current.CurrentItem.CurrentItem.Items.RemoveAt(0);
    }

    private void ConvertExcel_Tapped(object sender, EventArgs e)
    {
        Shell.Current.CurrentItem.CurrentItem.Items.Add(new UploadPage());
        Shell.Current.CurrentItem.CurrentItem.Items.RemoveAt(0);
    }

    private void SystemInfo_Tapped(object sender, EventArgs e)
    {
        Shell.Current.CurrentItem.CurrentItem.Items.Add(new SettingsPage_SystemInfo());
        Shell.Current.CurrentItem.CurrentItem.Items.RemoveAt(0);

    }
}