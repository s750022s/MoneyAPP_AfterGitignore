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
    int CachedId = 0;

    /// <summary>
    /// 建構頁面並寫入資料
    /// </summary>
    /// <param name="id">紀錄ID</param>
    public RecordRevisePage(int id)
    {
        InitializeComponent();

        //選單
        AccountModelToPicker();
        CategoryModelToPicker();

        //寫入資料
        CachedId = id;
        SetRecordInfo(id);

        //計算機
        Calculator.AlertRequest += Calculator_AlertRequest;
        Calculator.OKButtonClicked += OnOKButtonClicked;

        //預設Focus
        Category.Focus();
    }
    
    
  
    //頁面相關
    
    
    
    /// <summary>
    /// 儲存按鈕
    /// </summary>
    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        RecordModel record = SetRecordModel();
        App.ServiceRepo.UpdateRecord(record);
    }

    /// <summary>
    /// 刪除按鈕;
    /// IsDelete = true
    /// </summary>

    private void DeleteButton_Clicked(object sender, EventArgs e)
    {
        RecordModel record = SetRecordModel();
        record.IsDelete = true;
        App.ServiceRepo.UpdateRecord(record);
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
    
    
    
    //資料相關
    
    
    
    /// <summary>
    /// 整理資料數據
    /// </summary>
    /// <returns></returns>
    private RecordModel SetRecordModel() 
    {
        RecordModel record = new RecordModel();
        record.RecordID = CachedId;
        record.RecordDay = RecordDay.Date;
        record.RecordTime = RecordTime.Time;
        record.IsExpenses = Expense.IsChecked;
        record.Item = Item.Text;
        record.LastUpdatedTime = DateTime.Now;
        record.IsDelete = false;

        try
        {
            int amount = Convert.ToInt32(Amount.Text.Replace(",", ""));
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
    /// 撈取紀錄
    /// </summary>
    private void SetRecordInfo(int id) 
    {
        RecordModel info = App.ServiceRepo.GetRecordById(id);
        RecordDay.Date = info.RecordDay.Date;
        RecordTime.Time = info.RecordTime;
        Item.Text = info.Item;
        if (info.IsExpenses == true)
        {
            Expense.IsChecked = true;
            Amount.Text = ((info.Amount)*-1).ToString("N0");
        }
        else 
        {
            Revenues.IsChecked = true;
            Amount.Text = info.Amount.ToString("N0");
        }

        foreach (var a in App.CachedAccounts)
        {
            if (info.AccountID == a.AccountID)
            {
                Account.SelectedIndex = a.Sequence;
            }
        }

        foreach (var c in App.CachedCategorys)
        {
            if (info.CategoryID == c.CategoryID)
            {
                Category.SelectedIndex = c.Sequence;
            }
        }
    }  
}
