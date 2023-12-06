using MoneyAPP.Controls;
using MoneyAPP.Models;


namespace MoneyAPP.Pages;

public partial class RecordRevisePage : ContentPage
{
    int CachedId = 0;
    public RecordRevisePage(int id)
    {
        InitializeComponent();
        AccountModelToPicker();
        CategoryModelToPicker();
        SetRecordInfo(id);
        Calculator.AlertRequest += Calculator_AlertRequest;
        Calculator.OKButtonClicked += OnOKButtonClicked;
        CachedId = id;
        SetFocus();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        SetFocus();
    }

    private void SetFocus()
    {
        Category.Focus();
    }

    public void Calculator_AlertRequest(object sender, Calculator.AlertRequestEventArgs e)
    {
        DisplayAlert(e.Title, e.Message, e.Cancel);
    }

    private void OnEntryFocused(object sender, FocusEventArgs e)
    {
        Calculator.IsVisible = true;
        creamy_butt_Image.IsVisible = false;
    }

    private void OnEntryUnfocused(object sender, FocusEventArgs e)
    {
        Calculator.IsVisible = false;
        creamy_butt_Image.IsVisible = true;
    }

    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        RecordModel record = SetRecordModel();
        StateMessage.Text = App.ServiceRepo.UpdateTodo(record);


    }

    private void DeleteButton_Clicked(object sender, EventArgs e)
    {
        RecordModel record = SetRecordModel();
        record.IsDelete = true;
        StateMessage.Text = App.ServiceRepo.UpdateTodo(record);
    }

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
            int amount = Convert.ToInt32(Amount_Editor.Text.Replace(",", ""));
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

    private void CategoryModelToPicker() 
    {
        if (App.CachedCategorys == null) 
        {
            App.CachedCategorys = App.ServiceRepo.GetCategoryOrderBySequence();
        }
        Category.ItemsSource = App.CachedCategorys;
        Category.ItemDisplayBinding = new Binding("Name");
        
    }

    private void AccountModelToPicker()
    {
        if (App.CachedAccounts == null)
        {
            App.CachedAccounts = App.ServiceRepo.GetAccountOrderBySequence();
        }
        Account.ItemsSource = App.CachedAccounts;
        Account.ItemDisplayBinding = new Binding("Name");
        
    }

    private void OnOKButtonClicked(object sender, Calculator.OKButtonClickedEventArgs e)
    {
        Amount_Editor.Text = e.Total.ToString(); 
    }

    private void SetRecordInfo(int id) 
    {
        RecordModel info = App.ServiceRepo.GetRecordById(id);
        RecordDay.Date = info.RecordDay.Date;
        RecordTime.Time = info.RecordTime;
        Item.Text = info.Item;
        if (info.IsExpenses == true)
        {
            Expense.IsChecked = true;
            Amount_Editor.Text = ((info.Amount)*-1).ToString("N0");
        }
        else 
        {
            Revenues.IsChecked = true;
            Amount_Editor.Text = info.Amount.ToString("N0");
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

    private void BackButton_Clicked(object sender, EventArgs e)
    {
        Shell.Current.CurrentItem.CurrentItem.Items.Add(new HomePage(RecordDay.Date));
        Shell.Current.CurrentItem.CurrentItem.Items.RemoveAt(0);
    }
}
