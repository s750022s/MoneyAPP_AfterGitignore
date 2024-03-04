using ZMoney.Models;
using ZMoney.Services;
using ZMoney.Controls;
using System.Collections.ObjectModel;
namespace ZMoney.Pages;

[QueryProperty(nameof(Start), "Start")]
[QueryProperty(nameof(End), "End")]
[QueryProperty(nameof(DataId), "DataId")]
[QueryProperty(nameof(IsCategory), "IsCategory")]
public partial class RecordsByClass : ContentPage
{
    private DateTime start, end;
    private int id;
    private bool isCategory;

    public DateTime Start
    {
        get => start;
        set
        {
            start = value;
            OnPropertyChanged();
        }
    }

    public DateTime End
    {
        get => end;
        set
        {
            end = value;
            OnPropertyChanged();
        }
    }

    public int DataId
    {
        get => id;
        set
        {
            id = value;
            OnPropertyChanged();
        }
    }

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
        /// ªð¦^¤W¤@­¶
        /// </summary>
        private void BackButton_Clicked(object sender, EventArgs e) 
    {
        Shell.Current.GoToAsync("..");
    }

    public ObservableCollection<RecordsFromGroup> GetNameAndData() 
    {
        var datas = new ObservableCollection<RecordsFromGroup>(_dbManager.GetRecordsFromGroup(start, end, id, isCategory));
        CategoryName_Label.Text = IsCategory == true ? 
            App.CachedCategorys.First(x => x.Id == id).Name : 
            App.CachedAccounts.First(x => x.Id == id).Name;
       return datas;

    }
    private void OnBorderTapped(object sender, TappedEventArgs e)
    {
        CustomBorder recordBorder = (CustomBorder)sender;
        int recordId = recordBorder.DataId;
        var navParam = new Dictionary<string, Object>() { { "DataId", recordId }, { "IsFromHome", false } };
        Shell.Current.GoToAsync("Home/RecordUpdate", navParam);

    }
}