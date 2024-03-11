using ZMoney.Controls;
using ZMoney.Models;
using ZMoney.Services;

namespace ZMoney.Pages;

/// <summary>
/// 首頁，接收參數When以設定生成時的日期。
/// </summary>
[QueryProperty(nameof(When), "Date")]
public partial class HomePage : ContentPage
{
    /// <summary>
    /// Shell參數When，設定生成時的日期。
    /// </summary>
    private DateTime when = DateTime.Today;
    public DateTime When
    {
        get => when;
        set
        {
            if (value != DateTime.MinValue)
            {
                when = value;
            }
            else
            {
                when = DateTime.Today;
            }
            OnPropertyChanged();
        }
    }
    private DbManager _dbManager;
    public HomePage(DbManager dbManager)
    {
        InitializeComponent();
        _dbManager = dbManager;
    }

    protected override void OnAppearing()
    {
        var data = GetOnedayData(when, out int totalAmount);
        RecordDate_DatePicker.Date = when;
        TotalAmount_LB.Text = totalAmount.ToString("N0");
        DatasCollectionView.ItemsSource = data;
    }

    /// <summary>
    /// 每筆資料都可被點擊，進入修改資料頁
    /// </summary>
    private void OnBorderTapped(object sender, TappedEventArgs e)
    {
        CustomBorder customBorder = (CustomBorder)sender;
        var navParam = new Dictionary<string, Object>() { { "DataId", customBorder.DataId }, { "IsFromHome", true } };
        Shell.Current.GoToAsync("Home/RecordUpdate", navParam);
    }

    /// <summary>
    /// 日期選擇器發生選取事件，切換成當天資料
    /// </summary>
    private void RecordDate_DateSelected(object sender, DateChangedEventArgs e)
    {
        var data = GetOnedayData(e.NewDate, out int totalAmount);
        DatasCollectionView.ItemsSource = data;
        TotalAmount_LB.Text = totalAmount.ToString("N0");
    }


    /// <summary>
    /// 取得一天的紀錄
    /// </summary>
    /// <param name="recordDay">查詢日期</param>
    /// <param name="totalAmount">out 總金額</param>
    /// <returns></returns>
    private List<HomePageData> GetOnedayData(DateTime recordDay, out int totalAmount)
    {
        var data = _dbManager.GetHomePageData(recordDay);

        //計算當天總額
        totalAmount = 0;
        foreach (var item in data)
        {
            totalAmount += item.AmountOfMoney;
        }
        return data;
    }
}

