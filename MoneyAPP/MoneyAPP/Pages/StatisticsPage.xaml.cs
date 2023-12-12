namespace MoneyAPP.Pages;

public partial class StatisticsPage : ContentPage
{
	public StatisticsPage()
	{
		InitializeComponent();
    }

    /// <summary>
    /// 選擇菜單，確定選擇跟取消選擇都會觸發。
    /// </summary>
    private void RadioButton_Clicked(object sender, CheckedChangedEventArgs e)
    {
        RadioButton radioButton = sender as RadioButton;
        switch (radioButton.Content)
        {
            case "類別統計":
                CategorysView.IsVisible = radioButton.IsChecked;
                break;

            case "帳戶統計":
                AccountsView.IsVisible = radioButton.IsChecked;
                break;

            case "帳戶當前狀態":
                AccountStatusView.IsVisible = radioButton.IsChecked;
                break;
        }
    }
}
