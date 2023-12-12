using System.Collections.ObjectModel;
using MoneyAPP.Models;


namespace MoneyAPP.Controls;

/// <summary>
/// 滾動紀錄，會隨灌入資料增長
/// </summary>
public partial class RecordTable : ContentView
{

    /// <summary>
    /// 資料繫結
    /// </summary>
    ObservableCollection<HomePageData> datas = new ObservableCollection<HomePageData>();
    public ObservableCollection<HomePageData> Datas { get { return datas; } }

    public RecordTable()
    {
        InitializeComponent();
    }

    /// <summary>
    /// 取得日期參數的所有資料(不包含已刪除者)
    /// 1. 資料繫結至CollectionView
    /// 2. 計算當天總額
    /// </summary>
    /// <param name="recordDay">要查詢的日期</param>
    /// <returns>當天總額</returns>
    public int GetOnedayInfo(DateTime recordDay)
    {
        var infos = App.ServiceRepo.GetHomePageData(recordDay);

        //資料繫結至CollectionView
        DatasCollectionView.ItemsSource = infos;

        //計算當天總額
        int totalAmount = 0;
        foreach (var item in infos)
        {
            totalAmount += item.Amount;
        }
        return totalAmount;
    }

    /// <summary>
    /// 每筆資料都可被點擊，進入修改資料頁
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnBorderTapped(object sender, TappedEventArgs e)
    {
        RecordBorder recordBorder = (RecordBorder)sender;
        int id = recordBorder.RecordId;
        Shell.Current.CurrentItem.CurrentItem.Items.Add(new Pages.RecordRevisePage(id));
        Shell.Current.CurrentItem.CurrentItem.Items.RemoveAt(0);
    }
}