using System.Collections.ObjectModel;
using ZMoney.Models;
using ZMoney.Services;
using ZMoney.Controls;
using ZMoney.ViewModels;
using System.Globalization;

namespace ZMoney.Pages;

public partial class StatisticsPage : ContentPage
{
    private DbManager _dbManager;
    private bool IsCategory;
    private DateTime start = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
    private DateTime end = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1).AddDays(-1);


    public StatisticsPage(DbManager dbManager)
	{
		InitializeComponent();
        _dbManager = dbManager;
        SharedMethod.CheckAppCached(_dbManager);

    }

    protected override void OnAppearing() 
    {
        CategoryButton.IsChecked = true;
        StartDate.Date = start;
        EndDate.Date = end;
        GetDateTotal();
    }


    private void RadioButton_Clicked(object sender, CheckedChangedEventArgs e) 
	{
        RadioButton radioButton = (RadioButton)sender;

        switch (radioButton.Content)
        {
            case "���O�έp":
                CategoryOrAccount.IsVisible = radioButton.IsChecked;
                IsCategory = true;
                GetDateTotal();
                break;

            case "�b��έp":
                CategoryOrAccount.IsVisible = radioButton.IsChecked;
                IsCategory = false;
                GetDateTotal();
                break;

            case "�b���e���A":
                IsCategory = false;
                AccountCurrentTotal.IsVisible = radioButton.IsChecked;
                AccountCurrentTotalCollectionView.ItemsSource = new ObservableCollection<AccountModel>(App.CachedAccounts);
                break;
           }
        }


    // ���O/�b��έp�϶�

   
    private void Button_Clicked(object sender, EventArgs e)
    {
        if (EndDate.Date >= StartDate.Date)
        {
            start = StartDate.Date;
            end = EndDate.Date;
            GetDateTotal();
        }
        else 
        {
            DisplayAlert("�����d��p��_�l�d��A�Э��s�d�ߡC", "", "OK");
        }
    }

    private void GetDateTotal() 
    {
        var info = _dbManager.GetTotalByIsExpenses(start, end);
        Total_Label.Text = "$" + info[2].ToString("N0");
        TotalIncome_Label.Text = "$" + info[0].ToString("N0");
        TotalExpense_Label.Text = "$" + info[1].ToString("N0");

        if (IsCategory == true)
        {
            DatasCollectionView.ItemsSource = _dbManager.GetToatlFromCategoryGroup(start, end);
        }
        else 
        {
            DatasCollectionView.ItemsSource = _dbManager.GetToatlFromAccountGroup(start, end);
        }



    }
    private void OnBorderTapped(object sender, TappedEventArgs e) 
    {
        try
        {
            CustomBorder border = (CustomBorder)sender;
            int id = border.DataId;
            var navParam = new Dictionary<string, object>()
            {
                {"Start",start },
                {"End",end},
                {"DataId", id},
                {"IsCategory",IsCategory }
            };
            Shell.Current.GoToAsync("Statistics/RecordsByClass", navParam);
        }
        catch (Exception ex)
        {
            LocalFileLogger logger = new LocalFileLogger();
            logger.Log("RecordUpdateError:" + ex);
            DisplayAlert("�X�{���`���~�A�Э��s����", "�L�k�ư����pô�}�o��", "OK");
        }
    }



    // ��e�b���`�B�϶�
    private int assetsTotal;
    private void EyeClose_Clicked(object sender, EventArgs e)
    {
        eyeClose_ImageBTN.IsVisible = false;
        eye_ImageBTN.IsVisible = true;
        assetsTotal = App.CachedAccounts.Sum(x => x.CurrentTotal);
        Total_LB.Text = "$ " + assetsTotal.ToString("N0");
    }
    private void Eye_Clicked(object sender, EventArgs e)
    {
        eyeClose_ImageBTN.IsVisible = true;
        eye_ImageBTN.IsVisible = false;
        Total_LB.Text = "$ ------";
    }

    private int CheckInput(string currentTotalStr)
    {
        var currentTotalStr2 = currentTotalStr.Replace(",", "");
        if (currentTotalStr2 == "")
        {
            throw new ArgumentException("���B�Ф��n�ť�");
        }

        try
        {
            if (!int.TryParse(currentTotalStr2, out int currentTotal))
            {
                throw new ArgumentException("�Ф��n��J�p�ƩΤ�r�AMoney�O���|���p�ƪ���!");
            }
            return currentTotal;
        }
        catch (OverflowException)
        {
            throw new ArgumentException("�Q�n��J�����B�w�W�L20���W���C");
        } 
    }

    private void PencilButton_Clicked(object sender, EventArgs e)
    {
        ImageButton pencilButton = (ImageButton)sender;
        Grid parentGrid = (Grid)pencilButton.Parent;
        Entry amountEntry = parentGrid.FindByName<Entry>("Amount_Entry");

        // �ˬd�O�_��� Amount_Entry ����
        if (amountEntry != null)
        {
            // �ק� Amount_Entry ���ݩ�
            var strToint = int.TryParse(amountEntry.Text.Replace(",", ""), out originalValue);
            amountEntry.IsReadOnly = false;
            amountEntry.Focus();
        }

        ImageButton checkButton = parentGrid.FindByName<ImageButton>("check_ImageBTN");
        checkButton.IsVisible = true;
        pencilButton.IsVisible = false;
    }

    private int originalValue;
    private void CheckButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            ImageButton checkButton = (ImageButton)sender;
            Grid parentGrid = (Grid)checkButton.Parent;
            CustomBorder parentBorder = (CustomBorder)parentGrid.Parent;
            Entry amountEntry = parentGrid.FindByName<Entry>("Amount_Entry");

            // �ˬd�O�_��� Amount_Entry ����
            if (amountEntry != null)
            {
                var intTotal = CheckInput(amountEntry.Text);

                // �ק� Amount_Entry ���ݩ�
                _dbManager.UpdateCurrentTotal(parentBorder.DataId, intTotal - originalValue);
                SharedMethod.SetAppCached(_dbManager);
                amountEntry.Text = App.CachedAccounts.First(x => x.Id == parentBorder.DataId).CurrentTotal.ToString("N0"); 
                amountEntry.IsReadOnly = true;
                int newVaule = eye_ImageBTN.IsVisible == true ? assetsTotal - originalValue + intTotal : assetsTotal;
                Total_LB.Text = "$ " + newVaule.ToString("N0");
            }

            ImageButton pencilButton = parentGrid.FindByName<ImageButton>("pencil_ImageBTN");
            pencilButton.IsVisible = true;
            checkButton.IsVisible = false;
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
}

public class StringLengthToBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value > 5;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return !((int)value > 5);
    }
}