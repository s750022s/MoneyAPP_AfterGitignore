using MoneyAPP.Controls;
using System.Globalization;

namespace MoneyAPP.Pages;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
        int totalAmount = RecordTable.GetOnedayInfo(DateTime.Now);
        TotalAmount_LB.Text = totalAmount.ToString("N0");
    }

    public HomePage(DateTime date)
    {
        InitializeComponent();
        RecordDate_DatePicker.Date = date;
        int totalAmount = RecordTable.GetOnedayInfo(date);
        TotalAmount_LB.Text = totalAmount.ToString("N0");
    }

    /// <summary>
    /// 當收到不顯示時，調整fullMoonTips與hotdog_Image的屬性
    /// </summary>
    /// <param name="show">是否顯示Tips</param>
    protected void ISShowFullTips(bool show)
    {
        if (show == false)
        {
            FullMoonTips_LB.IsEnabled = false;
            FullMoonTips_LB.IsVisible = false;
            RecordTable.Margin = new Thickness(0, 0, 0, 50);
        }
    }

    private void RecordDate_DateSelected(object sender, DateChangedEventArgs e)
    {
        int totalAmount = RecordTable.GetOnedayInfo(e.NewDate);
        TotalAmount_LB.Text = totalAmount.ToString("N0");
    }
}

