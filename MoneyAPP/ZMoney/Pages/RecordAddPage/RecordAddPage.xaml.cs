using ZMoney.ViewModels;
using ZMoney.Services;
using ZMoney.Controls;

namespace ZMoney.Pages;

public partial class RecordAddPage : ContentPage
{
    private RecordBindingModel _record;
    private DbManager _dbManager;
    public RecordAddPage(DbManager dbManager)
	{
		InitializeComponent();
        _record = (RecordBindingModel)BindingContext;
        _dbManager = dbManager;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        GetPicker();
        _record.AddDefault();
    }

    /// <summary>
    /// 製作選單
    /// </summary>
    private void GetPicker()
    {
        SharedMethod.CheckAppCached(_dbManager);

        Account.ItemsSource = App.CachedAccounts;
        Account.ItemDisplayBinding = new Binding("Name");

        Category.ItemsSource = App.CachedCategorys;
        Category.ItemDisplayBinding = new Binding("Name");
    }


    /// <summary>
    /// 當Focus於計算機時，下方出現小算盤
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnEntryFocused(object sender, FocusEventArgs e)
    {
        Calculator.IsVisible = true;
        TabBar.IsVisible = false;
    }

    /// <summary>
    /// 當Focus離開計算機時，下方出現菜單
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnEntryUnfocused(object sender, FocusEventArgs e)
    {
        Calculator.IsVisible = false;
        TabBar.IsVisible = true;
    }

    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            _record.CheckEntry();
            var recordModel = _record.SetRecordToRecordModel();
            _dbManager.AddRecord(recordModel);
            _dbManager.UpdateCurrentTotal(recordModel.AccountId, recordModel.AmountOfMoney);
            SharedMethod.SetAppCached(_dbManager);
            var navParam = new Dictionary<string, object>() { { "Date", recordModel.RecordDateTime } };

            Shell.Current.GoToAsync($"Home", navParam);
        }
        catch (ArgumentException ex)
        {
            DisplayAlert(ex.Message, "", "OK");
        }
        catch (Exception ex) 
        {
            LocalFileLogger logger = new LocalFileLogger();
            logger.Log("RecordUpdateError:" + ex);
            DisplayAlert("出現異常錯誤，請重新嘗試", "無法排除請聯繫開發者", "OK");
        }
    }

    /// <summary>
    /// 不儲存，返回首頁
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CloseButton_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync($"Home");
    }
}