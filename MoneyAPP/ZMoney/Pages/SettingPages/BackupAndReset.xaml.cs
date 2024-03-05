using Syncfusion.XlsIO;
using System.IO.Compression;
using ZMoney.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace ZMoney.Pages;

public partial class BackupAndReset : ContentPage
{
    private LocalFileLogger _logger;
    private DbManager _dbManager;
    public BackupAndReset(LocalFileLogger logger, DbManager dbManager)
	{
		InitializeComponent();
        _logger = logger;
        _dbManager = dbManager;
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
        string date = DateTime.Today.ToString("yyyy-MM-dd");
        string newFileName = string.Format("ZMoney-{0}.db", date);
        File.Copy(
            FileAccessHelper.GetLocalFilePath("ZMoney.db"), 
            FileAccessHelper.GetLocalFilePath(newFileName));
        await Share.Default.RequestAsync(new ShareFileRequest
        {
            Title = "共享檔案",
            File = new ShareFile(FileAccessHelper.GetLocalFilePath(newFileName))
        });
        _logger.Log("匯出資料庫");
    }

    /// <summary>
    /// 匯入備份檔
    /// </summary>
    private async void ImportBackupFile_Clicked(object sender, EventArgs e)
    {
        var ans = await DisplayAlert("匯入備份檔將會覆蓋手機中現有資料，確定要匯入嗎?", "", "匯入", "取消");
        if (ans == true) 
        {
            try
            {
                _dbManager.Close();
                await PickAndShow(new PickOptions());
                _dbManager.Open();
                await DisplayAlert("已匯入完成", "", "OK");
            }
            catch (Exception ex) 
            {
                _logger.Log("匯入失敗:" + ex.ToString());
                await DisplayAlert("匯入失敗", "", "OK");
            }
        }
    }

    private async Task PickAndShow(PickOptions options) 
    {
        var result = await FilePicker.Default.PickAsync(options);
        if (result != null)
        {
            if (result.FileName.EndsWith("db", StringComparison.OrdinalIgnoreCase))
            {
                using var stream = await result.OpenReadAsync();
                using var targetStream = File.Open(FileAccessHelper.GetLocalFilePath("ZMoney.db"), FileMode.Create);
                await stream.CopyToAsync(targetStream);

            }
        }
    }

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
                string FilePath = FileAccessHelper.GetLocalFilePath("ZMoney.db");
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
    /// 匯出log檔
    /// </summary>
    private async void Log_Clicked(object sender, EventArgs e)
    {
        var zipFilePath = Path.Join(Directory.GetParent(_logger.LogPath).FullName, "Log.zip");
        if (File.Exists(zipFilePath))
        {
            File.Delete(zipFilePath);
        }
        ZipFile.CreateFromDirectory(_logger.LogPath, zipFilePath);
        await Share.Default.RequestAsync(new ShareFileRequest
        {
            Title = "共享檔案",
            File = new ShareFile(FileAccessHelper.GetLocalFilePath(zipFilePath))
        });
        _logger.Log("匯出Log壓縮檔");
    }

    private async void ExportExcel_Clicked(object sender, EventArgs e) 
    {
        using (ExcelEngine excelEngine = new ExcelEngine())
        {
            // 初始化 Excel 應用程式
            Syncfusion.XlsIO.IApplication application = excelEngine.Excel;
            // 設定預設 Excel 版本為 Xlsx
            application.DefaultVersion = ExcelVersion.Xlsx;
            // 建立新的工作簿
            IWorkbook workbook = application.Workbooks.Create(1);
            // 取得工作簿中的第一個工作表
            IWorksheet worksheet = workbook.Worksheets[0];

            var reports = _dbManager.GetAllRecordByIsDelete().OrderByDescending(x => x.RecordDateTime);
            worksheet.ImportData(reports, 2, 1, false);



            //儲存檔案
            string date = DateTime.Today.ToString("yyyy-MM-dd");
            string filePath = FileAccessHelper.GetLocalFilePath(string.Format("Export-{0}.xlsx", date));

            Stream excelStream = File.Create(filePath);
            workbook.SaveAs(excelStream);
            excelStream.Dispose();
            await Share.Default.RequestAsync(new ShareFileRequest
            {
                Title = "共享檔案",
                File = new ShareFile(FileAccessHelper.GetLocalFilePath(filePath))
            });
            _logger.Log("匯出Excel");
        }
    }


}