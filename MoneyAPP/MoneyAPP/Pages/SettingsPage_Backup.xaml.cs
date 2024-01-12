using Microsoft.Maui.Controls.PlatformConfiguration;
using MoneyAPP.Services;

namespace MoneyAPP.Pages;

/// <summary>
/// �ƥ��٭쭶
/// </summary>
public partial class SettingsPage_Backup : ContentPage
{
	public SettingsPage_Backup()
	{
		InitializeComponent();
	}

    /// <summary>
    /// �إ߳ƥ��ɡA�ϥΦ@���ɮץ\��
    /// </summary>
    private async void CreatBackupFile_Tapped(object sender, TappedEventArgs e)
    {
        string filePath = FileAccessHelper.GetLocalFilePath("record.db3");
        await Share.Default.RequestAsync(new ShareFileRequest
        {
            Title = "�@���ɮ�",
            File = new ShareFile(filePath)
        });
    }

    /// <summary>
    /// �פJ�ƥ���
    /// </summary>
    private void ImportBackupFile_Tapped(object sender, TappedEventArgs e)
    {
        //TOT ����ɮרñN�ɮש����w��m
    }

    /// <summary>
    /// �٩��l�]�w�A�R��DB��
    /// </summary>
    private async void Restore_Tapped(object sender, TappedEventArgs e)
    {
        var ans = await DisplayAlert("�٭�L�k�_��A�T�w�n�٭��?", "", "�٭�", "����");
        if (ans == true) 
        {
            string FilePath = FileAccessHelper.GetLocalFilePath("record.db3");
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
                var a = DisplayAlert("�٭즨�\", "�Э��s�Ұ����ε{��", "OK");
            }
            else
            {
                var a = DisplayAlert("�٭쥢��", "���ѭ�]�G�䤣��]�w���", "OK");
            }
        }
    }

    /// <summary>
    /// ��^�W�@��
    /// </summary>
    private void BackButton_Clicked(object sender, EventArgs e)
    {
        Shell.Current.CurrentItem.CurrentItem.Items.Add(new SettingsPage());
        Shell.Current.CurrentItem.CurrentItem.Items.RemoveAt(0);
    }
}