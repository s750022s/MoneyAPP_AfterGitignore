
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
        // �ˬd�O�_��� Amount_Entry ����
        if (amountEntry != null)
        {
            // �ק� Amount_Entry ���ݩ�
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
        // �ˬd�O�_��� Amount_Entry ����
        if (amountEntry != null)
        {
            // �ק� Amount_Entry ���ݩ�
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
            MessagingCenter.Send<object, string>(this, "Error", "���B�Ф��n�ť�");
            return false;
        }

        try
        {
            int amount = Convert.ToInt32(input);
            return true;
        }
        catch (OverflowException)
        {
            MessagingCenter.Send<object, string>(this, "Error", "�Q�n��J�����B�w�W�L20���W���C");
            return false;
        }
        catch (Exception)
        {
            MessagingCenter.Send<object, string>(this, "Error", "�Ф��n��J�p�ƩΤ�r�AMoney�O���|���p�ƪ���!");
            return false;
        }
    }
}