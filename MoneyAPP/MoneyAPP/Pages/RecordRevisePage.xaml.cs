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
    private int cachedId = 0;


    private RecordByContent _record;

    /// <summary>
    /// �غc�����üg�J���
    /// </summary>
    /// <param name="id">����ID</param>
    public RecordRevisePage(int id)
    {
        InitializeComponent();
        _record = (RecordByContent)BindingContext;
        cachedId = id;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        //���
        AccountModelToPicker();
        CategoryModelToPicker();

        _record.GetRecordByRecordModel(App.ServiceRepo.GetRecordById(cachedId));

        //�p���
        Calculator.AlertRequest += Calculator_AlertRequest;
        Calculator.OKButtonClicked += OnOKButtonClicked;

    }

    //��������



    /// <summary>
    /// �x�s���s
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
                DisplayAlert("��J���~", arg, "OK");
            });
        }
    }

    /// <summary>
    /// �R�����s;
    /// IsDelete = true
    /// </summary>

    private async void DeleteButton_Clicked(object sender, EventArgs e)
    {
        var ans = await DisplayAlert("�T�w�n�R����?", "�ק�N���x�s�A�õL�k��_�Ӹ��", "�R��", "����");
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
        Category.SelectedIndex = 0;
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
        Account.SelectedIndex = 0;
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

}
