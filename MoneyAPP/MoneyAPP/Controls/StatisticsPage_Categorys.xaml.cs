namespace MoneyAPP.Controls;

public partial class StatisticsPage_Categorys : ContentView
{
    public StatisticsPage_Categorys()
    {
        InitializeComponent();
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
        Month_Picker.Items.Add("¥þ¦~");
        for (int start = 1; start <= 12; start++)
        {
            Month_Picker.Items.Add(start.ToString("00"));
        }

        int nowMonth = DateTime.Now.Month;
        Month_Picker.SelectedItem = nowMonth.ToString("00");
    }

    private void GetCategorysGroup() 
    {
        int year = Convert.ToInt32(Year_Picker.SelectedItem);
        int month = Month_Picker.SelectedIndex;

        int daysInMonth = DateTime.DaysInMonth(year, month == 0 ? 12 : month);

        DateTime start = new DateTime(year, month == 0 ? 1 : month, 1);
        DateTime end = new DateTime(year, month == 0 ? 12 : month, daysInMonth);

        DatasCollectionView.ItemsSource = App.ServiceRepo.GetCategoryTotal(start, end);
    }

    private void OnPickerSelectedIndexChanged(object sender, EventArgs e) 
    {
        GetCategorysGroup();
    }
}