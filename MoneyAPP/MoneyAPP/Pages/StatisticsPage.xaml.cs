namespace MoneyAPP.Pages;

public partial class StatisticsPage : ContentPage
{
	public StatisticsPage()
	{
		InitializeComponent();
    }

    private void RadioButton_Clicked(object sender, CheckedChangedEventArgs e)
    {
        RadioButton radioButton = sender as RadioButton;
        switch (radioButton.Content)
        {
            case "���O�έp":
                CategorysView.IsVisible = radioButton.IsChecked;
                break;

            case "�b��έp":
                AccountsView.IsVisible = radioButton.IsChecked;
                break;

            case "�b���e���A":
                AccountStatusView.IsVisible = radioButton.IsChecked;
                break;

        }
    }
}
