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
/// �ץX�פJ�P���m
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
    /// ��^�W�@��
    /// </summary>
    private void BackBTN_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("..");
    }

    /// <summary>
    /// �إ߳ƥ��ɡA�ϥΦ@���ɮץ\��
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
            Title = "�@���ɮ�",
            File = new ShareFile(FileAccessHelper.GetLocalFilePath(newFileName))
        });
        _logger.Log("�ץX��Ʈw");
    }

    /// <summary>
    /// �פJ�ƥ���
    /// </summary>
    private async void ImportBackupFile_Clicked(object sender, EventArgs e)
    {
        var ans = await DisplayAlert("�פJ�ƥ��ɱN�|�л\������{����ơA�T�w�n�פJ��?", "", "�פJ", "����");
        if (ans == true)
        {
            try
            {
                _dbManager.Close();
                await PickAndShow(new PickOptions());
                _dbManager.Open();
                await DisplayAlert("�w�פJ����", "", "OK");
            }
            catch (Exception ex)
            {
                _logger.Log("�פJ����:" + ex.ToString());
                await DisplayAlert("�פJ����", "", "OK");
            }
        }
    }

    /// <summary>
    /// ��ܿ���ɮ׵e��
    /// </summary>
    /// <param name="options">����ɮ׳]�w</param>
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
    /// �٭��l�]�w�A�R��DB��
    /// </summary>
    private async void Restore_Clicked(object sender, EventArgs e)
    {
        var ans = await DisplayAlert("�٭�L�k�_��A�T�w�n�٭��?", "", "�٭�", "����");
        if (ans == true)
        {
            try
            {
                string FilePath = FileAccessHelper.GetLocalFilePath("ZMoney.db");
                if (File.Exists(FilePath))
                {
                    File.Delete(FilePath);
                    var a = DisplayAlert("�٭즨�\", "�Э��s�Ұ����ε{��", "OK");
                    _logger.Log("�٭��Ʈw�C");
                }
            }
            catch (Exception ex)
            {
                var a = DisplayAlert("�٭쥢��", "���ѭ�]�G�䤣��]�w���", "OK");
                _logger.Log("�٭��Ʈw����:" + ex);
            }
        }
    }

    /// <summary>
    /// �ץXlog��
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
                Title = "�@���ɮ�",
                File = new ShareFile(FileAccessHelper.GetLocalFilePath(zipFilePath))
            });
            _logger.Log("�ץXLog���Y��");
        }
        else 
        {
            await DisplayAlert("�S��log��Ƨ�", "�Э���APP", "OK");
        }
    }

    /// <summary>
    /// �ץXExcel��
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
            Title = "�@���ɮ�",
            File = new ShareFile(FileAccessHelper.GetLocalFilePath(excel.FullName))
        });
        _logger.Log("�ץXExcel");
    }

    /// <summary>
    /// �ഫExcel
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


