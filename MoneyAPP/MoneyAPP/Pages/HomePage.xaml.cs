using MoneyAPP.Controls;

namespace MoneyAPP.Pages;

/// <summary>
/// 建構首頁
/// </summary>
public partial class HomePage : ContentPage
{
    /// <summary>
    /// 多型建構子，不帶日期參數，生成當天資料
    /// </summary>
	public HomePage()
	{
		InitializeComponent();
        int totalAmount = RecordTable.GetOnedayInfo(DateTime.Now);
        TotalAmount_LB.Text = totalAmount.ToString("N0"); //格式化字串，帶有千位符
    }

    /// <summary>
    /// 多型建構子，帶有日期參數會查詢參數資料
    /// </summary>
    /// <param name="date">查詢日期</param>
    public HomePage(DateTime date)
    {
        InitializeComponent();
        RecordDate_DatePicker.Date = date;
        int totalAmount = RecordTable.GetOnedayInfo(date);
        TotalAmount_LB.Text = totalAmount.ToString("N0");
    }

    /// <summary>
    /// 日期選擇器發生選取事件，切換成當天資料
    /// </summary>
    /// <param name="sender">Datepicker日期選擇器</param>
    /// <param name="e">切換事件</param>
    private void RecordDate_DateSelected(object sender, DateChangedEventArgs e)
    {
        int totalAmount = RecordTable.GetOnedayInfo(e.NewDate);
        TotalAmount_LB.Text = totalAmount.ToString("N0");
    }
}

