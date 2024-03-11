using System.Collections.ObjectModel;
using ZMoney.Controls;
using ZMoney.Models;
using ZMoney.Services;

namespace ZMoney.Pages;

/// <summary>
/// �S�w���دS�w�϶��������C��A
/// �����Ѽ�Start�_�l����BEnd��������BDataId����Id�BIsCategory�O���O�����C
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
    /// �_�l���
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
    /// �������
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
    /// ����Id
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
    /// �O���O����
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
    /// �p�G�S����ơA���NoRecord�C
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
    /// ��^�W�@��
    /// </summary>
    private void BackButton_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("..");
    }

    /// <summary>
    /// �HId���o�S�w���ئW��
    /// </summary>
    /// <returns>RecordsFromGroup���X</returns>
    public ObservableCollection<RecordsFromGroup> GetNameAndData()
    {
        var datas = new ObservableCollection<RecordsFromGroup>(_dbManager.GetRecordsFromGroup(start, end, id, isCategory));
        CategoryName_Label.Text = IsCategory == true ?
            App.CachedCategorys.First(x => x.Id == id).Name :
            App.CachedAccounts.First(x => x.Id == id).Name;
        return datas;
    }

    /// <summary>
    /// �I��Border�i�J��ƽs�譶�C
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