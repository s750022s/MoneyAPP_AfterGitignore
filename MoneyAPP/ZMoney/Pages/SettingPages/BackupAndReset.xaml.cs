using ZMoney.Services;
namespace ZMoney.Pages;

public partial class BackupAndReset : ContentPage
{
    private LocalFileLogger _logger;
    public BackupAndReset(LocalFileLogger logger)
	{
		InitializeComponent();
        _logger = logger;
    }

    /// <summary>
    /// 返回上一頁
    /// </summary>
    private void BackBTN_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("..");
    }

    /// <summary>
    /// 建立備份檔，使用共享檔案功能
    /// </summary>
    private async void Button_Clicked(object sender, EventArgs e)
    {
        string filePath = FileAccessHelper.GetLocalFilePath("ZMoney.db");
        await Share.Default.RequestAsync(new ShareFileRequest
        {
            Title = "共享檔案",
            File = new ShareFile(filePath)
        });
        _logger.Log("匯出資料庫");
    }

    /// <summary>
    /// 匯入備份檔
    /// </summary>
    private void ImportBackupFile_Clicked(object sender, EventArgs e)
    {}

    /// <summary>
    /// 還於初始設定，刪除DB檔
    /// </summary>
    private async void Restore_Clicked(object sender, EventArgs e)
    {
        var ans = await DisplayAlert("還原無法復原，確定要還原嗎?", "", "還原", "取消");
        if (ans == true)
        {
            try
            {
                string FilePath = FileAccessHelper.GetLocalFilePath("record.db3");
                if (File.Exists(FilePath))
                {
                    File.Delete(FilePath);
                    var a = DisplayAlert("還原成功", "請重新啟動應用程式", "OK");
                    _logger.Log("還原資料庫。");
                }
            }
            catch(Exception ex) 
            {
                var a = DisplayAlert("還原失敗", "失敗原因：找不到設定資料", "OK");
                _logger.Log("還原資料庫失敗:" + ex);
            }
        }
    }

    /// <summary>
    /// 匯入備份檔
    /// </summary>
    private void Log_Clicked(object sender, EventArgs e)
    { }
}