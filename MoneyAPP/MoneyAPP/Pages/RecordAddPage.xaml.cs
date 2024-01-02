using MoneyAPP.Controls;
using MoneyAPP.Models;


namespace MoneyAPP.Pages;

/// <summary>
///  紀錄增加頁
/// </summary>
public partial class RecordAddPage : ContentPage
{
    private RecordByContent _record;

    public RecordAddPage()
    {
        InitializeComponent();
        _record = (RecordByContent)BindingContext;

        //製作選單
        AccountModelToPicker();
        CategoryModelToPicker();

        //套用計算機提醒視窗及OK鍵
        Calculator.AlertRequest += Calculator_AlertRequest;
        Calculator.OKButtonClicked += OnOKButtonClicked;

    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        _record.AddDefault();



    }

    /// <summary>
    /// 製作類別選單
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
    /// 製作帳戶選單
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


    //資料處理程序

    /// <summary>
    /// 按下儲存鍵，資料庫新增一筆資料，並返回首頁
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        var message = _record.FiledCheck();

        if (message == true)
        {
            RecordModel recordModel = _record.SetRecordToRecordModel();
            App.ServiceRepo.AddRecord(recordModel);
            Shell.Current.CurrentItem.CurrentItem.Items.Add((ShellContent)new HomePage(recordModel.RecordDay));
            Shell.Current.CurrentItem.CurrentItem.Items.RemoveAt(0);
        }
        else
        {
            MessagingCenter.Subscribe<object, string>(_record, "Error", (sender, arg) =>
            {
                DisplayAlert("輸入錯誤", arg, "OK");
            });
        };
    }

        /// <summary>
        /// 不儲存，返回首頁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Clicked(object sender, EventArgs e)
    {
        Shell.Current.CurrentItem.CurrentItem.Items.Add((ShellContent)new HomePage());
        Shell.Current.CurrentItem.CurrentItem.Items.RemoveAt(0);
    }

    
 

    //與計算機相關功能



    /// <summary>
    /// 接收並顯示計算機的提醒
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void Calculator_AlertRequest(object sender, Calculator.AlertRequestEventArgs e)
    {
        DisplayAlert(e.Title, e.Message, e.Cancel);
    }

    /// <summary>
    /// 計算機按下ok鍵，數字傳送到Amount_Editor
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnOKButtonClicked(object sender, Calculator.OKButtonClickedEventArgs e)
    {
        Amount_Editor.Text = e.Total.ToString();
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
}