namespace MoneyAPP.Pages;

public partial class SettingsPage_SystemInfo : ContentPage
{
	public SettingsPage_SystemInfo()
	{
		InitializeComponent();
        GetInfo();
    }

    /// <summary>
    /// 返回上一頁
    /// </summary>
    private void BackButton_Clicked(object sender, EventArgs e)
    {
        Shell.Current.CurrentItem.CurrentItem.Items.Add(new SettingsPage());
        Shell.Current.CurrentItem.CurrentItem.Items.RemoveAt(0);
    }

    /// <summary>
    /// 取得總筆數及總時長
    /// </summary>
    private void GetInfo() 
    {
        //Get Count
        Count_LB.Text = App.ServiceRepo.GetAllRecordCount().ToString()+"筆";

        //Get Date length
        var firstDay = App.ServiceRepo.GetRecordFirstday();
        if (firstDay == DateTime.MinValue) 
        {
            DateLength_LB.Text = "0天";
        }
        else
        {
            firstDay = firstDay.Date;
            DateTime today = DateTime.Today.Date;
            DateLength_LB.Text = ((today - firstDay).Days + 1).ToString() + "天";
        }

        //Read Version
        CurrentVersion.Text = VersionTracking.Default.CurrentVersion.ToString();
    }
}