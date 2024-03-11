using System.Collections.ObjectModel;
using System.Globalization;
using ZMoney.Controls;
using ZMoney.Models;
using ZMoney.Services;

namespace ZMoney.Pages;

/// <summary>
/// 統計頁面
/// </summary>
public partial class StatisticsPage : ContentPage
{
    private DbManager _dbManager;

    /// <summary>
    /// 是否為種類
    /// </summary>
    private bool IsCategory;

    /// <summary>
    /// 起始日
    /// </summary>
    private DateTime start = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

    /// <summary>
    /// 結束日
    /// </summary>
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


    /// <summary>
    /// 實作RadioButton對應的行動，用以切換顯示資料。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void RadioButton_Clicked(object sender, CheckedChangedEventArgs e)
    {
        //鎖定RadioButton
        RadioButton radioButton = (RadioButton)sender;

        switch (radioButton.Content)
        {
            case "類別統計":
                CategoryOrAccount.IsVisible = radioButton.IsChecked;
                IsCategory = true;
                GetDateTotal();
                break;

            case "帳戶統計":
                CategoryOrAccount.IsVisible = radioButton.IsChecked;
                IsCategory = false;
                GetDateTotal();
                break;

            case "帳戶當前狀態":
                IsCategory = false;
                AccountCurrentTotal.IsVisible = radioButton.IsChecked;
                AccountCurrentTotalCollectionView.ItemsSource = new ObservableCollection<AccountModel>(App.CachedAccounts);
                break;
        }
    }


    // 類別/帳戶統計區塊

    /// <summary>
    /// 查詢按鈕
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
            DisplayAlert("結束範圍小於起始範圍，請重新查詢。", "", "OK");
        }
    }

    /// <summary>
    /// 取得總額資料
    /// </summary>
    private void GetDateTotal()
    {
        var info = _dbManager.GetTotalByIsExpenses(start, end);
        Total_Label.Text = "$" + info[2].ToString("N0");
        TotalIncome_Label.Text = "$" + info[0].ToString("N0");
        TotalExpense_Label.Text = "$" + info[1].ToString("N0");

        if (IsCategory == true)
        {
            DatasCollectionView.ItemsSource = _dbManager.GetToatlFromCategoryGroup(start, end, GetExpense.IsChecked);
        }
        else
        {
            DatasCollectionView.ItemsSource = _dbManager.GetToatlFromAccountGroup(start, end, GetExpense.IsChecked);
        }
    }

    /// <summary>
    /// 點擊Border進入特定項目特定區間的紀錄清單
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
            DisplayAlert("出現異常錯誤，請重新嘗試", "無法排除請聯繫開發者", "OK");
        }
    }



    // 當前帳戶總額區塊
    private int assetsTotal;

    /// <summary>
    /// 顯示總額
    /// </summary>
    private void EyeClose_Clicked(object sender, EventArgs e)
    {
        eyeClose_ImageBTN.IsVisible = false;
        eye_ImageBTN.IsVisible = true;
        assetsTotal = App.CachedAccounts.Sum(x => x.CurrentTotal);
        Total_LB.Text = "$ " + assetsTotal.ToString("N0");
    }

    /// <summary>
    /// 隱藏總額
    /// </summary>
    private void Eye_Clicked(object sender, EventArgs e)
    {
        eyeClose_ImageBTN.IsVisible = true;
        eye_ImageBTN.IsVisible = false;
        Total_LB.Text = "$ ------";
    }


    /// <summary>
    /// 檢查輸入資料的正確性
    /// </summary>
    /// <param name="currentTotalStr">輸入當前總額</param>
    /// <returns>int版當前總額</returns>
    /// <exception cref="ArgumentException"></exception>
    private int CheckInput(string currentTotalStr)
    {
        var currentTotalStr2 = currentTotalStr.Replace(",", "");
        if (currentTotalStr2 == "")
        {
            throw new ArgumentException("金額請不要空白");
        }

        try
        {
            if (!int.TryParse(currentTotalStr2, out int currentTotal))
            {
                throw new ArgumentException("請不要輸入小數或文字，Money是不會有小數的喔!");
            }
            return currentTotal;
        }
        catch (OverflowException)
        {
            throw new ArgumentException("想要輸入的金額已超過20億上限。");
        }
    }

    /// <summary>
    /// 編輯按鈕
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PencilButton_Clicked(object sender, EventArgs e)
    {
        ImageButton pencilButton = (ImageButton)sender;
        Grid parentGrid = (Grid)pencilButton.Parent;
        Entry amountEntry = parentGrid.FindByName<Entry>("Amount_Entry");

        // 檢查是否找到 Amount_Entry 控件
        if (amountEntry != null)
        {
            // 修改 Amount_Entry 的屬性
            var strToint = int.TryParse(amountEntry.Text.Replace(",", ""), out originalValue);
            amountEntry.IsReadOnly = false;
            amountEntry.Focus();
        }

        ImageButton checkButton = parentGrid.FindByName<ImageButton>("check_ImageBTN");
        checkButton.IsVisible = true;
        pencilButton.IsVisible = false;
    }

    /// <summary>
    /// 原始數值，用以計算差額。
    /// </summary>
    private int originalValue;

    /// <summary>
    /// 儲存按鈕
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CheckButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            ImageButton checkButton = (ImageButton)sender;
            Grid parentGrid = (Grid)checkButton.Parent;
            CustomBorder parentBorder = (CustomBorder)parentGrid.Parent;
            Entry amountEntry = parentGrid.FindByName<Entry>("Amount_Entry");

            // 檢查是否找到 Amount_Entry 控件
            if (amountEntry != null)
            {
                var intTotal = CheckInput(amountEntry.Text);

                // 修改 Amount_Entry 的屬性
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
            DisplayAlert("出現異常錯誤，請重新嘗試", "無法排除請聯繫開發者", "OK");
        }
    }
}


/// <summary>
/// 項目字數為6時，縮小文字已顯示全部。
/// 不會大於7個字。
/// </summary>
public class StringLengthToBoolConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value != null ? (int)value > 5 : null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value != null ? !((int)value > 5) : null;
    }
}