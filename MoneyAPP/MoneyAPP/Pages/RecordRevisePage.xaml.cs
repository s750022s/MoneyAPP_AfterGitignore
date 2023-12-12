using MoneyAPP.Controls;
using MoneyAPP.Models;


namespace MoneyAPP.Pages;

/// <summary>
/// �����קﭶ
/// </summary>
public partial class RecordRevisePage : ContentPage
{
    /// <summary>
    /// �ȦsID�A��K�d��P�ק�
    /// </summary>
    int CachedId = 0;

    /// <summary>
    /// �غc�����üg�J���
    /// </summary>
    /// <param name="id">����ID</param>
    public RecordRevisePage(int id)
    {
        InitializeComponent();

        //���
        AccountModelToPicker();
        CategoryModelToPicker();

        //�g�J���
        CachedId = id;
        SetRecordInfo(id);

        //�p���
        Calculator.AlertRequest += Calculator_AlertRequest;
        Calculator.OKButtonClicked += OnOKButtonClicked;

        //�w�]Focus
        Category.Focus();
    }
    
    
  
    //��������
    
    
    
    /// <summary>
    /// �x�s���s
    /// </summary>
    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        RecordModel record = SetRecordModel();
        App.ServiceRepo.UpdateRecord(record);
    }

    /// <summary>
    /// �R�����s;
    /// IsDelete = true
    /// </summary>

    private void DeleteButton_Clicked(object sender, EventArgs e)
    {
        RecordModel record = SetRecordModel();
        record.IsDelete = true;
        App.ServiceRepo.UpdateRecord(record);
    }

    /// <summary>
    /// �������O���
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
    /// ���ͱb����
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
    /// ��^�W�@��
    /// </summary>
    private void BackButton_Clicked(object sender, EventArgs e)
    {
        Shell.Current.CurrentItem.CurrentItem.Items.Add(new HomePage(RecordDay.Date));
        Shell.Current.CurrentItem.CurrentItem.Items.RemoveAt(0);
    }
    
    
    
    //�p�������
    
    
    
    /// <summary>
    /// �p���������������
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void Calculator_AlertRequest(object sender, Calculator.AlertRequestEventArgs e)
    {
        DisplayAlert(e.Title, e.Message, e.Cancel);
    }

    /// <summary>
    /// ��Focused����H���w�����B��A���X�p���
    /// </summary>
    private void OnEntryFocused(object sender, FocusEventArgs e)
    {
        Calculator.IsVisible = true;
        creamy_butt_Image.IsVisible = false;
    }

    /// <summary>
    /// ��Focused����H���}���B��A�����p���
    /// </summary>
    private void OnEntryUnfocused(object sender, FocusEventArgs e)
    {
        Calculator.IsVisible = false;
        creamy_butt_Image.IsVisible = true;
    }

    /// <summary>
    /// �p���OK��^��
    /// </summary>
    private void OnOKButtonClicked(object sender, Calculator.OKButtonClickedEventArgs e)
    {
        Amount.Text = e.Total.ToString();
    }
    
    
    
    //��Ƭ���
    
    
    
    /// <summary>
    /// ��z��Ƽƾ�
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
    /// ��������
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
