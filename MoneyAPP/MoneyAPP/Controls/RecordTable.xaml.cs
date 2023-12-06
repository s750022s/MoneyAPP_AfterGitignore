using System.Collections.ObjectModel;
using MoneyAPP.Models;


namespace MoneyAPP.Controls;

/// <summary>
/// 滾動紀錄，會隨灌入資料增長
/// </summary>
public partial class RecordTable : ContentView
{
    ObservableCollection<HomePageData> datas = new ObservableCollection<HomePageData>();
    public ObservableCollection<HomePageData> Datas { get { return datas; } }

    public RecordTable()
    {
        InitializeComponent();
    }

    public int GetOnedayInfo(DateTime recordDay)
    {
        var infos = App.ServiceRepo.GetHomePageData(recordDay);
        DatasCollectionView.ItemsSource = infos;
        int totalAmount = 0;
        foreach (var item in infos)
        {
            totalAmount += item.Amount;
        }
        return totalAmount;

    }

    //可被點擊
    private void OnBorderTapped(object sender, TappedEventArgs e)
    {
        RecordBorder recordBorder = (RecordBorder)sender;
        int id = recordBorder.RecordId;
        Shell.Current.CurrentItem.CurrentItem.Items.Add(new Pages.RecordRevisePage(id));
        Shell.Current.CurrentItem.CurrentItem.Items.RemoveAt(0);
    }
}





