using MoneyAPP.Controls;

namespace MoneyAPP.Pages;

/// <summary>
/// �غc����
/// </summary>
public partial class HomePage : ContentPage
{
    /// <summary>
    /// �h���غc�l�A���a����ѼơA�ͦ���Ѹ��
    /// </summary>
	public HomePage()
	{
		InitializeComponent();
        int totalAmount = RecordTable.GetOnedayInfo(DateTime.Now);
        TotalAmount_LB.Text = totalAmount.ToString("N0"); //�榡�Ʀr��A�a���d���
    }

    /// <summary>
    /// �h���غc�l�A�a������ѼƷ|�d�߰ѼƸ��
    /// </summary>
    /// <param name="date">�d�ߤ��</param>
    public HomePage(DateTime date)
    {
        InitializeComponent();
        RecordDate_DatePicker.Date = date;
        int totalAmount = RecordTable.GetOnedayInfo(date);
        TotalAmount_LB.Text = totalAmount.ToString("N0");
    }

    /// <summary>
    /// �����ܾ��o�Ϳ���ƥ�A��������Ѹ��
    /// </summary>
    /// <param name="sender">Datepicker�����ܾ�</param>
    /// <param name="e">�����ƥ�</param>
    private void RecordDate_DateSelected(object sender, DateChangedEventArgs e)
    {
        int totalAmount = RecordTable.GetOnedayInfo(e.NewDate);
        TotalAmount_LB.Text = totalAmount.ToString("N0");
    }
}

