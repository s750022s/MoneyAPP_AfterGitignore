
namespace MoneyAPP.Pages;

public partial class SettingsPage_SystemInfo : ContentPage
{
	public SettingsPage_SystemInfo()
	{
		InitializeComponent();
        GetInfo();

    }

    private void BackButton_Clicked(object sender, EventArgs e)
    {
        Shell.Current.CurrentItem.CurrentItem.Items.Add(new SettingsPage());
        Shell.Current.CurrentItem.CurrentItem.Items.RemoveAt(0);
    }

    private void GetInfo() 
    {
        //Get Count
        Count_LB.Text = App.ServiceRepo.GetAllRecordCount().ToString()+"µ§";

        //Get Date length
        //DateLength_LB.Text = ""
        var firstDay = App.ServiceRepo.GetRecordFirstday();
        if (firstDay == DateTime.MinValue) 
        {
            DateLength_LB.Text = "0¤Ñ";
        }
        else
        {
            firstDay = firstDay.Date;
            DateTime today = DateTime.Today.Date;
            DateLength_LB.Text = ((today - firstDay).Days + 1).ToString() + "¤Ñ";
        }


        //Read Version
        CurrentVersion.Text = VersionTracking.Default.CurrentVersion.ToString();
    }


}