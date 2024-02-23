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
        string filePath = FileAccessHelper.GetLocalFilePath("ZMoney.db");
        await Share.Default.RequestAsync(new ShareFileRequest
        {
            Title = "�@���ɮ�",
            File = new ShareFile(filePath)
        });
        _logger.Log("�ץX��Ʈw");
    }

    /// <summary>
    /// �פJ�ƥ���
    /// </summary>
    private void ImportBackupFile_Clicked(object sender, EventArgs e)
    {}

    /// <summary>
    /// �٩��l�]�w�A�R��DB��
    /// </summary>
    private async void Restore_Clicked(object sender, EventArgs e)
    {
        var ans = await DisplayAlert("�٭�L�k�_��A�T�w�n�٭��?", "", "�٭�", "����");
        if (ans == true)
        {
            try
            {
                string FilePath = FileAccessHelper.GetLocalFilePath("record.db3");
                if (File.Exists(FilePath))
                {
                    File.Delete(FilePath);
                    var a = DisplayAlert("�٭즨�\", "�Э��s�Ұ����ε{��", "OK");
                    _logger.Log("�٭��Ʈw�C");
                }
            }
            catch(Exception ex) 
            {
                var a = DisplayAlert("�٭쥢��", "���ѭ�]�G�䤣��]�w���", "OK");
                _logger.Log("�٭��Ʈw����:" + ex);
            }
        }
    }

    /// <summary>
    /// �פJ�ƥ���
    /// </summary>
    private void Log_Clicked(object sender, EventArgs e)
    { }
}