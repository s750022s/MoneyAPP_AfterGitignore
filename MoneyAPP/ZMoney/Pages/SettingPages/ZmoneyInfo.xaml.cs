using ZMoney.Services;
namespace ZMoney.Pages;

/// <summary>
/// APP資訊
/// </summary>
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
        Count_LB.Text = _dbManager.GetAllRecordCount().ToString() + "筆";

        //Read Version
        CurrentVersion.Text = VersionTracking.Default.CurrentVersion.ToString();
    }

    /// <summary>
    /// 返回上一頁
    /// </summary>
    private void BackBTN_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("..");
    }
}