using OfficeOpenXml;
using System.ComponentModel;
using System.IO.Compression;
using System.Reflection;
using ZMoney.Models;
using ZMoney.Services;


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
        var reports = _dbManager.GetAllRecordByIsDelete().OrderByDescending(x => x.RecordDateTime);
        string date = DateTime.Today.ToString("yyyy-MM-dd");
        string filePath = FileAccessHelper.GetLocalFilePath(string.Format("Export-{0}.xlsx", date));
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        var excel = ExportExcel(reports, filePath);
        await Share.Default.RequestAsync(new ShareFileRequest
        {
            Title = "共享檔案",
            File = new ShareFile(FileAccessHelper.GetLocalFilePath(excel.FullName))
        });
        _logger.Log("匯出Excel");
    }

    private FileInfo ExportExcel(IOrderedEnumerable<ExcelModels> data, string filePath)
    {
        var output = new FileInfo(FileAccessHelper.GetLocalFilePath(filePath));

        ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        using (ExcelPackage workbook = new ExcelPackage(output))
        {
            // 取得工作簿中的第一個工作表
            ExcelWorksheet sheet = workbook.Workbook.Worksheets.Add("記帳歷史");

            // 用反射拿出Model裡的 DisplayName 的屬性
            var properties = typeof(ExcelModels).GetProperties()
                .Where(prop => prop.IsDefined(typeof(DisplayNameAttribute)));

            //定義表格大小及起始行
            var rows = data.Count() + 1;
            var cols = properties.Count();

            if (rows > 0 && cols > 0) 
            {
                sheet.Cells[1, 1].LoadFromCollection(data, true); // 寫入資料

                // 儲存格格式
                var colNumber = 1;
                foreach (var prop in properties) 
                {
                    // 時間處理，如果沒指定儲存格格式會變成 通用格式，就會以 int＝時間戳 的方式顯示
                    if (prop.PropertyType.Equals(typeof(DateTime)) ||
                       prop.PropertyType.Equals(typeof(DateTime?)))
                    {
                        sheet.Cells[2, colNumber, rows, colNumber].Style.Numberformat.Format = "yyyy/mm/dd hh:mm tt";
                    }
                    colNumber += 1;
                }
            }

            workbook.Save();
            return output;

        }
    }


}