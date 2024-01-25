namespace MoneyAPP.Controls;

public partial class StatisticsPage_Categorys : ContentView
{
    DateTime start, end;
    public StatisticsPage_Categorys()
    {
        InitializeComponent();
    }

    public void SetDataInPage() 
    {
        YearToPicker();
        MonthToPicker();
        GetCategorysGroup();
    }

    private void YearToPicker()
    {
        int nowYear = DateTime.Now.Year;
        for (int start = 2023; start <= nowYear+1; start++)
        {
            Year_Picker.Items.Add(start.ToString());
        }
        Year_Picker.SelectedItem = nowYear.ToString();
    }

    private void MonthToPicker()
    {
        Month_Picker.Items.Add("全年");
        for (int start = 1; start <= 12; start++)
        {
            Month_Picker.Items.Add(start.ToString("00"));
        }

        int nowMonth = DateTime.Now.Month;
        Month_Picker.SelectedItem = nowMonth.ToString("00");
    }

    private (DateTime start, DateTime end) GetDateRange() 
    {
        int year = Convert.ToInt32(Year_Picker.SelectedItem);
        int month = Month_Picker.SelectedIndex;

        int daysInMonth = DateTime.DaysInMonth(year, month == 0 ? 12 : month);

        start = new DateTime(year, month == 0 ? 1 : month, 1);
        end = new DateTime(year, month == 0 ? 12 : month, daysInMonth);
        return (start, end);
    }


    private void GetCategorysGroup() 
    {
        GetDateRange();

        var info = App.ServiceRepo.GetTotalByMonth(start, end);
        Total_Label.Text = "$"+ info.Total.ToString("N0");
        TotalIncome_Label.Text = "$" + info.TotalIncome.ToString("N0");
        TotalExpense_Label.Text = "$" + info.TotalExpense.ToString("N0");

        DatasCollectionView.ItemsSource = App.ServiceRepo.GetCategoryTotal(start, end);
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        GetCategorysGroup();
    }

    /// <summary>
    /// 每個類別都可被點擊，進入該類別紀錄清單
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnBorderTapped(object sender, TappedEventArgs e)
    {
        RecordBorder recordBorder = (RecordBorder)sender;
        int id = recordBorder.RecordId;
        Shell.Current.CurrentItem.CurrentItem.Items.Add(new Pages.StatisticsPage_SelectByCategory(start, end, id));
        Shell.Current.CurrentItem.CurrentItem.Items.RemoveAt(0);
    }
}