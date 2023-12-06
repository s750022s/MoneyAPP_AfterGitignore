namespace MoneyAPP.Pages;

public partial class SettingsPage_Backup : ContentPage
{
	public SettingsPage_Backup()
	{
		InitializeComponent();
	}

    private void CreatBackupFile_Tapped(object sender, TappedEventArgs e)
    {
        //TOT �Q�ΤU���覡�إ߳ƥ��ɩ�Doanload
    }

    private void ImportBackupFile_Tapped(object sender, TappedEventArgs e)
    {
        //TOT ����ɮרñN�ɮש����w��m
    }

    private async void Restore_Tapped(object sender, TappedEventArgs e)
    {
        var ans = await DisplayAlert("�٭�L�k�_��A�T�w�n�٭��?", "", "�٭�", "����");
        if (ans == true) 
        {
            string FilePath = FileAccessHelper.GetLocalFilePath("record.db3");
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
                DisplayAlert("�٭즨�\", "", "OK");
            }
            else
            {
                DisplayAlert("�٭쥢��", "���ѭ�]�G�䤣��]�w���", "OK");
            }
        }
    }

    private void BackButton_Clicked(object sender, EventArgs e)
    {
        Shell.Current.CurrentItem.CurrentItem.Items.Add(new SettingsPage());
        Shell.Current.CurrentItem.CurrentItem.Items.RemoveAt(0);
    }
}