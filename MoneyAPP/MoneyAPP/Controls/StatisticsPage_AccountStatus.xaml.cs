
namespace MoneyAPP.Controls;

public partial class StatisticsPage_AccountStatus : ContentView
{
	public StatisticsPage_AccountStatus()
	{
		InitializeComponent();
        GetAccountStatus();

    }

    private void GetAccountStatus()
    {
        DatasCollectionView.ItemsSource = App.ServiceRepo.GetAccountOrderBySequence();
    }

    private void EyeClose_Clicked(object sender, EventArgs e)
    {
        eyeClose_ImageBTN.IsVisible = false;
        eye_ImageBTN.IsVisible = true;
        Total_LB.Text = "$ 0";
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
            // 修改 Amount_Entry 的屬性
            amountEntry.IsReadOnly = true;
            string input = amountEntry.Text;

            bool check = Checkinput(input);
            if (check == true) 
            {
                App.ServiceRepo.UpdateAccountStatus(parentBorder.RecordId, Convert.ToInt32(input));
                GetAccountStatus();
            }
        }

        ImageButton pencilButton = parentGrid.FindByName<ImageButton>("pencil_ImageBTN");
        pencilButton.IsVisible = true;
        checkButton.IsVisible = false;


    }

    private bool Checkinput(string input) 
    {
        input = input.Replace(",", "");

        if (input == "")
        {
            MessagingCenter.Send<object, string>(this, "Error", "金額請不要空白");
            return false;
        }

        try
        {
            int amount = Convert.ToInt32(input);
            return true;
        }
        catch (OverflowException)
        {
            MessagingCenter.Send<object, string>(this, "Error", "想要輸入的金額已超過20億上限。");
            return false;
        }
        catch (Exception)
        {
            MessagingCenter.Send<object, string>(this, "Error", "請不要輸入小數或文字，Money是不會有小數的喔!");
            return false;
        }
    }
}