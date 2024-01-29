using MoneyAPP.Models;


namespace MoneyAPP.Controls;

public partial class StatisticsPage_AccountStatus : ContentView
{
    int originalValue;
    private List<AccountStatusCount> statusList;

    public StatisticsPage_AccountStatus()
	{
		InitializeComponent();
        GetAccountStatus();
    }

    private void GetAccountStatus()
    {
        statusList = (from account in App.CachedAccounts
                     select new AccountStatusCount(account.AccountID, account.Name, account.CurrentStatus.ToString("N0"))
                     ).ToList();
        DatasCollectionView.ItemsSource = statusList;
    }

    private void EyeClose_Clicked(object sender, EventArgs e)
    {
        eyeClose_ImageBTN.IsVisible = false;
        eye_ImageBTN.IsVisible = true;
        try
        {
            var total = statusList.Sum(x => x.CurrentStatus);
            Total_LB.Text = "$ " + total.ToString("N0");
        }
        catch(ArgumentException ex)
        {
            MessagingCenter.Send<object, string>(this, "Error", ex.Message);
        }
    }

    private void Eye_Clicked(object sender, EventArgs e)
    {
        eyeClose_ImageBTN.IsVisible = true;
        eye_ImageBTN.IsVisible = false;
        Total_LB.Text = "$ ------";
    }

    private void PencilButton_Clicked(object sender, EventArgs e)
    {
        ImageButton pencilButton = (ImageButton)sender;
        Grid parentGrid = (Grid)pencilButton.Parent;
        Entry amountEntry = parentGrid.FindByName<Entry>("Amount_Entry");
        // 檢查是否找到 Amount_Entry 控件
        if (amountEntry != null)
        {
            // 修改 Amount_Entry 的屬性
            originalValue = Convert.ToInt32(amountEntry.Text.Replace(",", ""));
            amountEntry.IsReadOnly = false;
            amountEntry.Focus();
        }

        ImageButton checkButton = parentGrid.FindByName<ImageButton>("check_ImageBTN");
        checkButton.IsVisible = true;
        pencilButton.IsVisible = false;
    }

    private void CheckButton_Clicked(object sender, EventArgs e)
    {
        ImageButton checkButton = (ImageButton)sender;
        Grid parentGrid = (Grid)checkButton.Parent;
        RecordBorder parentBorder = (RecordBorder)parentGrid.Parent;
        Entry amountEntry = parentGrid.FindByName<Entry>("Amount_Entry");
        // 檢查是否找到 Amount_Entry 控件
        if (amountEntry != null)
        {
            AccountStatusCount statusItem = statusList.Where(x => x.Id == parentBorder.RecordId).First();
            // 修改 Amount_Entry 的屬性
            amountEntry.IsReadOnly = true;
            App.ServiceRepo.UpdateCurrentStatus(statusItem.Id, statusItem.CurrentStatus -originalValue);
            App.CachedAccounts = App.ServiceRepo.GetAccountOrderBySequence();
        }

        ImageButton pencilButton = parentGrid.FindByName<ImageButton>("pencil_ImageBTN");
        pencilButton.IsVisible = true;
        checkButton.IsVisible = false;


    }

    
}