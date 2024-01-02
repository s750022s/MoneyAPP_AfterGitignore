using MoneyAPP.Controls;
using MoneyAPP.Models;


namespace MoneyAPP.Pages;

/// <summary>
///  紀錄增加頁
/// </summary>
public partial class RecordAddPage : ContentPage
{
    public RecordAddPage()
    {
        InitializeComponent();

        //TimePicker為當下時間
        RecordTime.Time = DateTime.Now.TimeOfDay;

        //製作選單
        AccountModelToPicker();
        CategoryModelToPicker();

        //套用計算機提醒視窗及OK鍵
        Calculator.AlertRequest += Calculator_AlertRequest;
        Calculator.OKButtonClicked += OnOKButtonClicked;

        //設定Focus於類別選擇器
        Category.Focus();
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
    /// 整理紀錄資料，按下儲存鍵後觸發
    /// </summary>
    /// <returns>紀錄資料結構</returns>
    private RecordModel SetRecordModel()
    {
        RecordModel record = new RecordModel();
        record.RecordDay = RecordDay.Date;
        record.RecordTime = RecordTime.Time;
        record.IsExpenses = Expense.IsChecked;
        record.Item = Item.Text;
        record.LastUpdatedTime = DateTime.Now;
        record.IsDelete = false;

        try
        {
            int amount = Convert.ToInt32(Amount_Editor.Text);
            if (Expense.IsChecked == true)
            {
                record.Amount = amount * -1;
            }
            else
            {
                record.Amount = amount;
            }
        }
        catch
        {
            DisplayAlert("輸入不正確", "請不要輸入小數或文字，Money是不會有小數的喔!", "OK");
        }

        foreach (var a in App.CachedAccounts)
        {
            if (Account.SelectedIndex == a.Sequence)
            {
                record.AccountID = a.AccountID;
            }
        }

        foreach (var c in App.CachedCategorys)
        {
            if (Category.SelectedIndex == c.Sequence)
            {
                record.CategoryID = c.CategoryID;
            }
        }
        return record;
    }

    /// <summary>
    /// 按下儲存鍵，資料庫新增一筆資料，並返回首頁
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        RecordModel record = SetRecordModel();
        App.ServiceRepo.AddRecord(record);
        Close_BTN.Focus();
        Shell.Current.CurrentItem.CurrentItem.Items.Add((ShellContent)new HomePage(record.RecordDay));
        Shell.Current.CurrentItem.CurrentItem.Items.RemoveAt(0);
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