using ZMoney.Controls;
using ZMoney.ViewModels;
using ZMoney.Services;

namespace ZMoney.Pages;

[QueryProperty(nameof(DataId), "DataId")]
public partial class RecordUpdatePage : ContentPage
{
    int id;
    public int DataId 
    {
        get => id;
        set
        {
            id = value;
            OnPropertyChanged();
        }
    }

    private RecordBindingModel _record;
    private DbManager _dbManager;
    public Dictionary<string, object> navParam = [];


    public RecordUpdatePage(DbManager dbManager)
	{
		InitializeComponent();
        _record = (RecordBindingModel)BindingContext;
        _dbManager = dbManager;
    }

    protected override void OnAppearing() 
    {
        base.OnAppearing();
        GetPicker();
        _record.GetRecordByRecordModel(_dbManager.GetRecordById(DataId));
        navParam["Date"]= _record.RecordDay;
    }

    protected override bool OnBackButtonPressed()
    {
        Shell.Current.GoToAsync("Home", navParam);
        return base.OnBackButtonPressed();
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


    //計算機相關

    /// <summary>
    /// 當Focused的對象指定為金額欄，跳出計算機
    /// </summary>
    private void OnEntryFocused(object sender, FocusEventArgs e)
    {
        Calculator.IsVisible = true;
        Creamy_butt_Image.IsVisible = false;
    }

    /// <summary>
    /// 當Focused的對象離開金額欄，關閉計算機
    /// </summary>
    private void OnEntryUnfocused(object sender, FocusEventArgs e)
    {
        Calculator.IsVisible = false;
        Creamy_butt_Image.IsVisible = true;
    }

    private void BackBTN_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("..", navParam);
    }

    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        try 
        {
            _record.CheckEntry();
            var recordModel = _record.SetRecordToRecordModel();
            var originalRecord = _dbManager.GetRecordById(recordModel.Id);
            _dbManager.UpdateRecord(recordModel);
            if (originalRecord.AccountId != recordModel.AccountId)
            {
                _dbManager.UpdateCurrentTotal(recordModel.AccountId, recordModel.AmountOfMoney);
                _dbManager.UpdateCurrentTotal(originalRecord.AccountId, 0 - originalRecord.AmountOfMoney);
            }
            else 
            {
                _dbManager.UpdateCurrentTotal(recordModel.AccountId, recordModel.AmountOfMoney - originalRecord.AmountOfMoney);
            }
            SharedMethod.SetAppCached(_dbManager);
            navParam["Date"] = recordModel.RecordDateTime;
            Shell.Current.GoToAsync($"Home", navParam);
        }
        catch (ArgumentException ex)
        {
            DisplayAlert(ex.Message, "", "OK");
        }
        catch(Exception ex) 
        {
            LocalFileLogger logger = new LocalFileLogger();
            logger.Log("RecordUpdateError:" + ex);
            DisplayAlert("出現異常錯誤，請重新嘗試", "無法排除請聯繫開發者", "OK");
        }
    }

    /// <summary>
    /// 刪除按鈕;
    /// IsDelete = true
    /// </summary>

    private async void DeleteButton_Clicked(object sender, EventArgs e) 
    {
        var ans = await DisplayAlert("確定要刪除嗎?", "修改將不儲存，並無法恢復該資料", "刪除", "取消");
        if (ans == true)
        {
            try 
            {
                var recordModel = _dbManager.GetRecordById(DataId);
                _dbManager.DeleteRecord(DataId);
                _dbManager.UpdateCurrentTotal(recordModel.AccountId, 0 - recordModel.AmountOfMoney);
                Shell.Current.GoToAsync($"Home", navParam);
            }
            catch (Exception ex)
            {
                LocalFileLogger logger = new LocalFileLogger();
                logger.Log("RecordUpdateError:" + ex);
                DisplayAlert("出現異常錯誤，請重新嘗試", "無法排除請聯繫開發者", "OK");
            }
        }
    }
}

