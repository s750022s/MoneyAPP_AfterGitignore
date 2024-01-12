using MoneyAPP.Controls;
using MoneyAPP.Models;


namespace MoneyAPP.Pages;

/// <summary>
/// 紀錄修改頁
/// </summary>
public partial class RecordRevisePage : ContentPage
{
    /// <summary>
    /// 暫存ID，方便查找與修改
    /// </summary>
    private int cachedId = 0;


    private RecordByContent _record;

    /// <summary>
    /// 建構頁面並寫入資料
    /// </summary>
    /// <param name="id">紀錄ID</param>
    public RecordRevisePage(int id)
    {
        InitializeComponent();
        _record = (RecordByContent)BindingContext;
        cachedId = id;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        //選單
        AccountModelToPicker();
        CategoryModelToPicker();

        _record.GetRecordByRecordModel(App.ServiceRepo.GetRecordById(cachedId));

        //計算機
        Calculator.AlertRequest += Calculator_AlertRequest;
        Calculator.OKButtonClicked += OnOKButtonClicked;

    }

    //頁面相關



    /// <summary>
    /// 儲存按鈕
    /// </summary>
    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        var message = _record.FiledCheck();
        if (message == true)
        {
            RecordModel recordModel = _record.SetRecordToRecordModel();
            recordModel.RecordID = cachedId;
            App.ServiceRepo.UpdateRecord(recordModel);
            Shell.Current.CurrentItem.CurrentItem.Items.Add((ShellContent)new HomePage(recordModel.RecordDay));
            Shell.Current.CurrentItem.CurrentItem.Items.RemoveAt(0);
        }
        else
        {
            MessagingCenter.Subscribe<object, string>(_record, "Error", (sender, arg) =>
            {
                DisplayAlert("輸入錯誤", arg, "OK");
            });
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
            RecordModel record = App.ServiceRepo.GetRecordById(cachedId);
            record.IsDelete = true;
            App.ServiceRepo.UpdateRecord(record);
            Shell.Current.CurrentItem.CurrentItem.Items.Add((ShellContent)new HomePage(record.RecordDay));
            Shell.Current.CurrentItem.CurrentItem.Items.RemoveAt(0);
        }
    }

    /// <summary>
    /// 產生類別選單
    /// </summary>
    private void CategoryModelToPicker()
    {
        if (App.CachedCategorys == null)
        {
            App.CachedCategorys = App.ServiceRepo.GetCategoryOrderBySequence();
        }
        Category.ItemsSource = App.CachedCategorys;
        Category.ItemDisplayBinding = new Binding("Name");
        Category.SelectedIndex = 0;
    }

    /// <summary>
    /// 產生帳戶選單
    /// </summary>
    private void AccountModelToPicker()
    {
        if (App.CachedAccounts == null)
        {
            App.CachedAccounts = App.ServiceRepo.GetAccountOrderBySequence();
        }
        Account.ItemsSource = App.CachedAccounts;
        Account.ItemDisplayBinding = new Binding("Name");
        Account.SelectedIndex = 0;
    }

    /// <summary>
    /// 返回上一頁
    /// </summary>
    private void BackButton_Clicked(object sender, EventArgs e)
    {
        Shell.Current.CurrentItem.CurrentItem.Items.Add(new HomePage(RecordDay.Date));
        Shell.Current.CurrentItem.CurrentItem.Items.RemoveAt(0);
    }
    
    
    
    //計算機相關
    
    
    
    /// <summary>
    /// 計算機的提醒接收器
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void Calculator_AlertRequest(object sender, Calculator.AlertRequestEventArgs e)
    {
        DisplayAlert(e.Title, e.Message, e.Cancel);
    }

    /// <summary>
    /// 當Focused的對象指定為金額欄，跳出計算機
    /// </summary>
    private void OnEntryFocused(object sender, FocusEventArgs e)
    {
        Calculator.IsVisible = true;
        creamy_butt_Image.IsVisible = false;
    }

    /// <summary>
    /// 當Focused的對象離開金額欄，關閉計算機
    /// </summary>
    private void OnEntryUnfocused(object sender, FocusEventArgs e)
    {
        Calculator.IsVisible = false;
        creamy_butt_Image.IsVisible = true;
    }

    /// <summary>
    /// 計算機OK鍵回傳
    /// </summary>
    private void OnOKButtonClicked(object sender, Calculator.OKButtonClickedEventArgs e)
    {
        Amount.Text = e.Total.ToString();
    }

}
