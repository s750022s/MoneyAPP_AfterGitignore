using ZMoney.Services;
namespace ZMoney.Pages;

public partial class SettingPage : ContentPage
{
	public SettingPage()
	{
		InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
        string filePath = FileAccessHelper.GetLocalFilePath("ZMoney.db");
        await Share.Default.RequestAsync(new ShareFileRequest
        {
            Title = "¦@¨ÉÀÉ®×",
            File = new ShareFile(filePath)
        });
    }
}