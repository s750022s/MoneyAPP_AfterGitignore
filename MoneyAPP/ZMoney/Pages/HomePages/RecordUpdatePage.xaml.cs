using ZMoney.Controls;
using ZMoney.ViewModels;
using ZMoney.Services;

namespace ZMoney.Pages;

/// <summary>
/// 紀錄修改頁；接收參數DataId及是否來自首頁。
/// 來自首頁:回首頁的記錄日期，否則回統計頁。
/// </summary>
[QueryProperty(nameof(DataId), "DataId")]
[QueryProperty(nameof(IsFromHome), "IsFromHome")]
public partial class RecordUpdatePage : ContentPage
{

    /// <summary>
    /// 紀錄Id
    /// </summary>
    private int _id;
    public int DataId 
    {
        get => _id;
        set
        {
            _id = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 是否來自首頁
    /// </summary>
    private bool _isFromHome;
    public bool IsFromHome
    {
        get => _isFromHome;
        set
        {
            _isFromHome = value;
            OnPropertyChanged();
        }
    }

    private RecordBindingModel _record;
    private DbManager _dbManager;

    /// <summary>
    /// 頁面參數搜集器
    /// </summary>
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

        //添加計算機OK鍵功能
        Calculator.OKButtonClicked += OnOKButtonClicked;
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

    /// <summary>
    /// 計算機OK鍵回傳
    /// </summary>
    private void OnOKButtonClicked(object sender, Calculator.OKButtonClickedEventArgs e)
    {
        AmountOfMoney.Text = e.Total.ToString();
    }


    /// <summary>
    /// 返回鍵
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BackBTN_Clicked(object sender, EventArgs e)
    {
        if (_isFromHome == true)
        {
            Shell.Current.GoToAsync("Home", navParam);
        }
        else
        {
            Shell.Current.GoToAsync("..");
        }
    }


    /// <summary>
    /// 儲存按鈕
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        try 
        {
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
                await Shell.Current.GoToAsync($"Home", navParam);
            }
            catch (Exception ex)
            {
                LocalFileLogger logger = new LocalFileLogger();
                logger.Log("RecordUpdateError:" + ex);
                await DisplayAlert("出現異常錯誤，請重新嘗試", "無法排除請聯繫開發者", "OK");
            }
        }
    }
}

