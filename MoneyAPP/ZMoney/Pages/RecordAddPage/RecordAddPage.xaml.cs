using ZMoney.ViewModels;
using ZMoney.Services;
using ZMoney.Controls;

namespace ZMoney.Pages;

public partial class RecordAddPage : ContentPage
{
    private RecordBindingModel _record;
    private DbManager _dbManager;
    public RecordAddPage(DbManager dbManager)
	{
		InitializeComponent();
        _record = (RecordBindingModel)BindingContext;
        _dbManager = dbManager;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        GetPicker();
        _record.AddDefault();
    }

    /// <summary>
    /// �s�@���
    /// </summary>
    private void GetPicker()
    {
        SharedMethod.CheckAppCached(_dbManager);

        Account.ItemsSource = App.CachedAccounts;
        Account.ItemDisplayBinding = new Binding("Name");

        Category.ItemsSource = App.CachedCategorys;
        Category.ItemDisplayBinding = new Binding("Name");
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

    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            _record.CheckEntry();
            var recordModel = _record.SetRecordToRecordModel();
            _dbManager.AddRecord(recordModel);
            _dbManager.UpdateCurrentTotal(recordModel.AccountId, recordModel.AmountOfMoney);
            SharedMethod.SetAppCached(_dbManager);
            var navParam = new Dictionary<string, object>() { { "Date", recordModel.RecordDateTime } };

            Shell.Current.GoToAsync($"Home", navParam);
        }
        catch (ArgumentException ex)
        {
            DisplayAlert(ex.Message, "", "OK");
        }
        catch (Exception ex) 
        {
            LocalFileLogger logger = new LocalFileLogger();
            logger.Log("RecordUpdateError:" + ex);
            DisplayAlert("�X�{���`���~�A�Э��s����", "�L�k�ư����pô�}�o��", "OK");
        }
    }

    /// <summary>
    /// ���x�s�A��^����
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CloseButton_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync($"Home");
    }
}