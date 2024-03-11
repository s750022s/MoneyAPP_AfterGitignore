using System.Collections.ObjectModel;
using ZMoney.Controls;
using ZMoney.Models;
using ZMoney.Services;

namespace ZMoney.Pages;

/// <summary>
/// 特定項目特定區間的紀錄列表，
/// 接收參數Start起始日期、End結束日期、DataId項目Id、IsCategory是不是種類。
/// </summary>
[QueryProperty(nameof(Start), "Start")]
[QueryProperty(nameof(End), "End")]
[QueryProperty(nameof(DataId), "DataId")]
[QueryProperty(nameof(IsCategory), "IsCategory")]
public partial class RecordsByClass : ContentPage
{
    private DateTime start, end;
    private int id;
    private bool isCategory;

    /// <summary>
    /// 起始日期
    /// </summary>
    public DateTime Start
    {
        get => start;
        set
        {
            start = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 結束日期
    /// </summary>
    public DateTime End
    {
        get => end;
        set
        {
            end = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 項目Id
    /// </summary>
    public int DataId
    {
        get => id;
        set
        {
            id = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 是不是種類
    /// </summary>
    public bool IsCategory
    {
        get => isCategory;
        set
        {
            isCategory = value;
            OnPropertyChanged();
        }
    }

    private DbManager _dbManager;
    public RecordsByClass(DbManager dbManager)
    {
        InitializeComponent();
        _dbManager = dbManager;
    }

    /// <summary>
    /// 如果沒有資料，顯示NoRecord。
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();
        var datas = GetNameAndData();
        if (datas.Count != 0)
        {
            DatasCollectionView.IsVisible = true;
            DatasCollectionView.ItemsSource = datas;
            NoRecord.IsVisible = false;
        }
        else
        {
            DatasCollectionView.IsVisible = false;
            NoRecord.IsVisible = true;
        }
    }

    /// <summary>
    /// 返回上一頁
    /// </summary>
    private void BackButton_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("..");
    }

    /// <summary>
    /// 以Id取得特定項目名稱
    /// </summary>
    /// <returns>RecordsFromGroup集合</returns>
    public ObservableCollection<RecordsFromGroup> GetNameAndData()
    {
        var datas = new ObservableCollection<RecordsFromGroup>(_dbManager.GetRecordsFromGroup(start, end, id, isCategory));
        CategoryName_Label.Text = IsCategory == true ?
            App.CachedCategorys.First(x => x.Id == id).Name :
            App.CachedAccounts.First(x => x.Id == id).Name;
        return datas;
    }

    /// <summary>
    /// 點擊Border進入資料編輯頁。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnBorderTapped(object sender, TappedEventArgs e)
    {
        CustomBorder recordBorder = (CustomBorder)sender;
        int recordId = recordBorder.DataId;
        var navParam = new Dictionary<string, Object>() { { "DataId", recordId }, { "IsFromHome", false } };
        Shell.Current.GoToAsync("Home/RecordUpdate", navParam);
    }
}