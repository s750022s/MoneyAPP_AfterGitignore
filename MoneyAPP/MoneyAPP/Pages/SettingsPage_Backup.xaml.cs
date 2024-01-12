using Microsoft.Maui.Controls.PlatformConfiguration;
using MoneyAPP.Services;

namespace MoneyAPP.Pages;

/// <summary>
/// 備份還原頁
/// </summary>
public partial class SettingsPage_Backup : ContentPage
{
	public SettingsPage_Backup()
	{
		InitializeComponent();
	}

    /// <summary>
    /// 建立備份檔，使用共享檔案功能
    /// </summary>
    private async void CreatBackupFile_Tapped(object sender, TappedEventArgs e)
    {
        string filePath = FileAccessHelper.GetLocalFilePath("record.db3");
        await Share.Default.RequestAsync(new ShareFileRequest
        {
            Title = "共享檔案",
            File = new ShareFile(filePath)
        });
    }

    /// <summary>
    /// 匯入備份檔
    /// </summary>
    private void ImportBackupFile_Tapped(object sender, TappedEventArgs e)
    {
        //TOT 選擇檔案並將檔案放到指定位置
    }

    /// <summary>
    /// 還於初始設定，刪除DB檔
    /// </summary>
    private async void Restore_Tapped(object sender, TappedEventArgs e)
    {
        var ans = await DisplayAlert("還原無法復原，確定要還原嗎?", "", "還原", "取消");
        if (ans == true) 
        {
            string FilePath = FileAccessHelper.GetLocalFilePath("record.db3");
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
                var a = DisplayAlert("還原成功", "請重新啟動應用程式", "OK");
            }
            else
            {
                var a = DisplayAlert("還原失敗", "失敗原因：找不到設定資料", "OK");
            }
        }
    }

    /// <summary>
    /// 返回上一頁
    /// </summary>
    private void BackButton_Clicked(object sender, EventArgs e)
    {
        Shell.Current.CurrentItem.CurrentItem.Items.Add(new SettingsPage());
        Shell.Current.CurrentItem.CurrentItem.Items.RemoveAt(0);
    }
}