using Excely.EPPlus.LGPL.Shaders;
using Excely.TableFactories;
using Excely.Workflows;
using OfficeOpenXml;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO.Compression;
using System.Reflection;
using ZMoney.Models;
using ZMoney.Services;


namespace ZMoney.Pages;

/// <summary>
/// 匯出匯入與重置
/// </summary>
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

    /// <summary>
    /// 顯示選取檔案畫面
    /// </summary>
    /// <param name="options">選取檔案設定</param>
    /// <returns></returns>
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
    /// 還原初始設定，刪除DB檔
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
            catch (Exception ex)
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
        var path = Directory.GetParent(_logger.LogPath);
        string zipFilePath;
        if (path != null)
        {
            zipFilePath = Path.Join(path.FullName, "Log.zip");
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
        else 
        {
            await DisplayAlert("沒有log資料夾", "請重啟APP", "OK");
        }
    }

    /// <summary>
    /// 匯出Excel檔
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

    /// <summary>
    /// 轉換Excel
    /// </summary>
    private FileInfo ExportExcel(IOrderedEnumerable<ExcelModels> data, string filePath)
    {
        var output = new FileInfo(FileAccessHelper.GetLocalFilePath(filePath));

        var exportOptions = new ClassListTableFactoryOptions<ExcelModels>
        {
            PropertyNamePolicy = property =>
            {
                var displayNameAttribute = property.GetCustomAttribute<DisplayNameAttribute>();
                return displayNameAttribute?.DisplayName ?? property.Name;
            },
            CustomValuePolicy = (property, obj) => property.Name switch
            {
                nameof(ExcelModels.RecordDateTime) => obj.RecordDateTime.ToString("yyyy/MM/dd hh:mm tt"),
                nameof(ExcelModels.AmountOfMoney) => obj.AmountOfMoney.ToString("N0"),
                _ => ClassListTableFactoryOptions<ExcelModels>.DefaultCustomValuePolicy(property, obj)
            }
        };
        var exporter = ExcelyExporter.FromClassList<ExcelModels>(options: exportOptions);
        using var excel = exporter.ToExcel(data);
        var worksheet = excel.Workbook.Worksheets.First();
        var cellFittingShader = new CellFittingShader();
        cellFittingShader.Excute(worksheet);
        excel.SaveAs(output);
        return output;

    }
}


