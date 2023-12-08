using MoneyAPP.Controls;

namespace MoneyAPP.Pages;

public partial class UploadPage : ContentPage
{
	public UploadPage()
	{
        InitializeComponent();
    }

    private void BackButton_Clicked(object sender, EventArgs e)
    {
        Shell.Current.CurrentItem.CurrentItem.Items.Add(new SettingsPage());
        Shell.Current.CurrentItem.CurrentItem.Items.RemoveAt(0);
    }
}