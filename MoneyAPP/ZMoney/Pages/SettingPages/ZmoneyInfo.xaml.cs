using ZMoney.Services;
namespace ZMoney.Pages;

public partial class ZmoneyInfo : ContentPage
{
    private DbManager _dbManager;

    public ZmoneyInfo(DbManager dbManager)
	{
		InitializeComponent();
        _dbManager = dbManager;
    }

    protected override void OnAppearing() 
    {
        //Get Count
        Count_LB.Text = _dbManager.GetAllRecordCount().ToString() + "µ§";

        //Read Version
        CurrentVersion.Text = VersionTracking.Default.CurrentVersion.ToString();
    }

    /// <summary>
    /// ªð¦^¤W¤@­¶
    /// </summary>
    private void BackBTN_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("..");
    }
}