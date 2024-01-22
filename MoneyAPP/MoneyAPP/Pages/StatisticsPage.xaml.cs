using MoneyAPP.Controls;
namespace MoneyAPP.Pages;

public partial class StatisticsPage : ContentPage
{
    private StatisticsPage_Categorys _control;
    public StatisticsPage()
	{
		InitializeComponent();
        _control = CategorysView;
    }

    protected override void OnAppearing() 
    {
        base.OnAppearing();
        _control.SetDataInPage();
    }

    /// <summary>
    /// ��ܵ��A�T�w��ܸ������ܳ��|Ĳ�o�C
    /// </summary>
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
