using MoneyAPP.Controls;
using MoneyAPP.Models;


namespace MoneyAPP.Pages;

/// <summary>
///  �����W�[��
/// </summary>
public partial class RecordAddPage : ContentPage
{
    private RecordByContent _record;

    public RecordAddPage()
    {
        InitializeComponent();
        _record = (RecordByContent)BindingContext;

        //�s�@���
        AccountModelToPicker();
        CategoryModelToPicker();

        //�M�έp�������������OK��
        Calculator.AlertRequest += Calculator_AlertRequest;
        Calculator.OKButtonClicked += OnOKButtonClicked;

    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        _record.AddDefault();



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
    /// ���U�x�s��A��Ʈw�s�W�@����ơA�ê�^����
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
                DisplayAlert("��J���~", arg, "OK");
            });
        };
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