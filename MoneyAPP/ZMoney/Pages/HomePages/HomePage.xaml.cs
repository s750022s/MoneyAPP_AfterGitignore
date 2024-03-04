using ZMoney.Models;
using ZMoney.Controls;
using ZMoney.Services;

namespace ZMoney.Pages;

[QueryProperty(nameof(When),"Date")]
public partial class HomePage : ContentPage
{
    private DateTime when = DateTime.Today;
    public DateTime When 
    {
        get => when;
        set 
        {
            if (value != DateTime.MinValue)
            {
                when = value;
            }
            else 
            {
                when = DateTime.Today;
            }
            OnPropertyChanged();
        }
    }

    private DbManager _dbManager;



    public HomePage(DbManager dbManager)
    {
        InitializeComponent();
        _dbManager = dbManager;
    }

    protected override void OnAppearing() 
    {
        var data = GetOnedayData(when,out int totalAmount);
        RecordDate_DatePicker.Date = when;
        TotalAmount_LB.Text = totalAmount.ToString("N0");
        DatasCollectionView.ItemsSource = data;
    }

    /// <summary>
    /// �C����Ƴ��i�Q�I���A�i�J�ק��ƭ�
    /// </summary>
    private void OnBorderTapped(object sender, TappedEventArgs e)
    {
        CustomBorder customBorder = (CustomBorder)sender;
        var navParam = new Dictionary<string, Object>() { { "DataId", customBorder.DataId },{ "IsFromHome", true } };
        Shell.Current.GoToAsync("Home/RecordUpdate", navParam);
    }

    /// <summary>
    /// �����ܾ��o�Ϳ���ƥ�A��������Ѹ��
    /// </summary>
    private void RecordDate_DateSelected(object sender, DateChangedEventArgs e)
    {
        var data = GetOnedayData(e.NewDate, out int totalAmount);
        DatasCollectionView.ItemsSource = data;
        TotalAmount_LB.Text = totalAmount.ToString("N0");
    }


    private List<HomePageData> GetOnedayData(DateTime recordDay, out int totalAmount)
    {
        var data = _dbManager.GetHomePageData(recordDay);

        //�p�����`�B
        totalAmount = 0;
        foreach (var item in data)
        {
            totalAmount += item.AmountOfMoney;
        }
        return data;
    }


}

