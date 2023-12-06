using MoneyAPP.Controls;
using MoneyAPP.Models;
using MoneyAPP.Services;


namespace MoneyAPP.Pages;

public partial class RecordAddPage : ContentPage
{
    public RecordAddPage()
    {
        InitializeComponent();
        AccountModelToPicker();
        CategoryModelToPicker();
        Calculator.AlertRequest += Calculator_AlertRequest;
        Calculator.OKButtonClicked += OnOKButtonClicked;
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
        TabBar.IsVisible = false;
    }

    private void OnEntryUnfocused(object sender, FocusEventArgs e)
    {
        Calculator.IsVisible = false;
        TabBar.IsVisible = true;
    }

    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        RecordModel record = SetRecordModel();
        int result = App.ServiceRepo.AddTodo(record);
        Close_BTN.Focus();
        Shell.Current.CurrentItem.CurrentItem.Items.Add((ShellContent)new HomePage());
        Shell.Current.CurrentItem.CurrentItem.Items.RemoveAt(0);
    }

    private void CloseButton_Clicked(object sender, EventArgs e)
    {
        Shell.Current.CurrentItem.CurrentItem.Items.Add((ShellContent)new HomePage());
        Shell.Current.CurrentItem.CurrentItem.Items.RemoveAt(0);
    }

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

    private void OnOKButtonClicked(object sender, Calculator.OKButtonClickedEventArgs e)
    {
        Amount_Editor.Text = e.Total.ToString(); 
    }


}