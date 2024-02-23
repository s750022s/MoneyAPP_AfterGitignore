using ZMoney.Controls;
using ZMoney.ViewModels;
using ZMoney.Services;

namespace ZMoney.Pages;

[QueryProperty(nameof(DataId), "DataId")]
public partial class RecordUpdatePage : ContentPage
{
    int id;
    public int DataId 
    {
        get => id;
        set
        {
            id = value;
            OnPropertyChanged();
        }
    }

    private RecordBindingModel _record;
    private DbManager _dbManager;
    public Dictionary<string, object> navParam = [];


    public RecordUpdatePage(DbManager dbManager)
	{
		InitializeComponent();
        _record = (RecordBindingModel)BindingContext;
        _dbManager = dbManager;
    }

    protected override void OnAppearing() 
    {
        base.OnAppearing();
        GetPicker();
        _record.GetRecordByRecordModel(_dbManager.GetRecordById(DataId));
        navParam["Date"]= _record.RecordDay;
    }

    protected override bool OnBackButtonPressed()
    {
        Shell.Current.GoToAsync("Home", navParam);
        return base.OnBackButtonPressed();
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


    //�p�������

    /// <summary>
    /// ��Focused����H���w�����B��A���X�p���
    /// </summary>
    private void OnEntryFocused(object sender, FocusEventArgs e)
    {
        Calculator.IsVisible = true;
        Creamy_butt_Image.IsVisible = false;
    }

    /// <summary>
    /// ��Focused����H���}���B��A�����p���
    /// </summary>
    private void OnEntryUnfocused(object sender, FocusEventArgs e)
    {
        Calculator.IsVisible = false;
        Creamy_butt_Image.IsVisible = true;
    }

    private void BackBTN_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("..", navParam);
    }

    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        try 
        {
            _record.CheckEntry();
            var recordModel = _record.SetRecordToRecordModel();
            var originalRecord = _dbManager.GetRecordById(recordModel.Id);
            _dbManager.UpdateRecord(recordModel);
            if (originalRecord.AccountId != recordModel.AccountId)
            {
                _dbManager.UpdateCurrentTotal(recordModel.AccountId, recordModel.AmountOfMoney);
                _dbManager.UpdateCurrentTotal(originalRecord.AccountId, 0 - originalRecord.AmountOfMoney);
            }
            else 
            {
                _dbManager.UpdateCurrentTotal(recordModel.AccountId, recordModel.AmountOfMoney - originalRecord.AmountOfMoney);
            }
            SharedMethod.SetAppCached(_dbManager);
            navParam["Date"] = recordModel.RecordDateTime;
            Shell.Current.GoToAsync($"Home", navParam);
        }
        catch (ArgumentException ex)
        {
            DisplayAlert(ex.Message, "", "OK");
        }
        catch(Exception ex) 
        {
            LocalFileLogger logger = new LocalFileLogger();
            logger.Log("RecordUpdateError:" + ex);
            DisplayAlert("�X�{���`���~�A�Э��s����", "�L�k�ư����pô�}�o��", "OK");
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
            try 
            {
                var recordModel = _dbManager.GetRecordById(DataId);
                _dbManager.DeleteRecord(DataId);
                _dbManager.UpdateCurrentTotal(recordModel.AccountId, 0 - recordModel.AmountOfMoney);
                Shell.Current.GoToAsync($"Home", navParam);
            }
            catch (Exception ex)
            {
                LocalFileLogger logger = new LocalFileLogger();
                logger.Log("RecordUpdateError:" + ex);
                DisplayAlert("�X�{���`���~�A�Э��s����", "�L�k�ư����pô�}�o��", "OK");
            }
        }
    }
}

