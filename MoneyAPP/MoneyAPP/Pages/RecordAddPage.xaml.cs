using MoneyAPP.Controls;
using MoneyAPP.Models;


namespace MoneyAPP.Pages;

/// <summary>
///  �����W�[��
/// </summary>
public partial class RecordAddPage : ContentPage
{
    public RecordAddPage()
    {
        InitializeComponent();

        //TimePicker����U�ɶ�
        RecordTime.Time = DateTime.Now.TimeOfDay;

        //�s�@���
        AccountModelToPicker();
        CategoryModelToPicker();

        //�M�έp�������������OK��
        Calculator.AlertRequest += Calculator_AlertRequest;
        Calculator.OKButtonClicked += OnOKButtonClicked;

        //�]�wFocus�����O��ܾ�
        Category.Focus();
    }

    /// <summary>
    /// �s�@���O���
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
    /// �s�@�b����
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


    //��ƳB�z�{��



    /// <summary>
    /// ��z������ơA���U�x�s���Ĳ�o
    /// </summary>
    /// <returns>������Ƶ��c</returns>
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
            DisplayAlert("��J�����T", "�Ф��n��J�p�ƩΤ�r�AMoney�O���|���p�ƪ���!", "OK");
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
    /// ���U�x�s��A��Ʈw�s�W�@����ơA�ê�^����
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
    /// ���x�s�A��^����
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CloseButton_Clicked(object sender, EventArgs e)
    {
        Shell.Current.CurrentItem.CurrentItem.Items.Add((ShellContent)new HomePage());
        Shell.Current.CurrentItem.CurrentItem.Items.RemoveAt(0);
    }

    
 

    //�P�p��������\��



    /// <summary>
    /// ��������ܭp���������
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void Calculator_AlertRequest(object sender, Calculator.AlertRequestEventArgs e)
    {
        DisplayAlert(e.Title, e.Message, e.Cancel);
    }

    /// <summary>
    /// �p������Uok��A�Ʀr�ǰe��Amount_Editor
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnOKButtonClicked(object sender, Calculator.OKButtonClickedEventArgs e)
    {
        Amount_Editor.Text = e.Total.ToString();
    }

    /// <summary>
    /// ��Focus��p����ɡA�U��X�{�p��L
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnEntryFocused(object sender, FocusEventArgs e)
    {
        Calculator.IsVisible = true;
        TabBar.IsVisible = false;
    }

    /// <summary>
    /// ��Focus���}�p����ɡA�U��X�{���
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnEntryUnfocused(object sender, FocusEventArgs e)
    {
        Calculator.IsVisible = false;
        TabBar.IsVisible = true;
    }
}