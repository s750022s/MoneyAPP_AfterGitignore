using System.Collections.ObjectModel;
using ZMoney.Models;
using ZMoney.Services;

namespace ZMoney.Controls;

public partial class AccountCurrentTotalControls : ContentView
{
    int originalValue;

    public static readonly BindableProperty ManagerProperty =
       BindableProperty.Create(nameof(Manager), typeof(DbManager), typeof(AccountCurrentTotalControls), null);
    public DbManager Manager
    {
        get { return (DbManager)GetValue(ManagerProperty); }
        set { SetValue(ManagerProperty, value); }
    }

    public AccountCurrentTotalControls()
	{
		InitializeComponent();

        DatasCollectionView.ItemsSource = new ObservableCollection<AccountModel>(App.CachedAccounts);
    }

    private void EyeClose_Clicked(object sender, EventArgs e) 
	{
        eyeClose_ImageBTN.IsVisible = false;
        eye_ImageBTN.IsVisible = true;
        var total = App.CachedAccounts.Sum(x => x.CurrentTotal);
        Total_LB.Text = "$ " + total.ToString("N0");
    }
	private void Eye_Clicked(object sender, EventArgs e) 
    {
        eyeClose_ImageBTN.IsVisible = true;
        eye_ImageBTN.IsVisible = false;
        Total_LB.Text = "$ ------";
    }

    private int CheckInput(string currentTotalStr)
    {
        var currentTotalStr2 = currentTotalStr.Replace(",", "");
        if (currentTotalStr2 == "")
        {
            throw new ArgumentException("���B�Ф��n�ť�");
        }

        try
        {
            if (!int.TryParse(currentTotalStr2, out int currentTotal))
            {
                throw new ArgumentException("�Ф��n��J�p�ƩΤ�r�AMoney�O���|���p�ƪ���!");
            }
            return currentTotal;
        }
        catch (OverflowException)
        {
            throw new ArgumentException("�Q�n��J�����B�w�W�L20���W���C");
        }
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
                var strToint = int.TryParse(amountEntry.Text.Replace(",", ""), out originalValue);
                amountEntry.IsReadOnly = false;
                amountEntry.Focus();
            }

            ImageButton checkButton = parentGrid.FindByName<ImageButton>("check_ImageBTN");
            checkButton.IsVisible = true;
            pencilButton.IsVisible = false;
    }

	private void CheckButton_Clicked(object sender, EventArgs e) 
    {
        try
        {
            ImageButton checkButton = (ImageButton)sender;
            Grid parentGrid = (Grid)checkButton.Parent;
            CustomBorder parentBorder = (CustomBorder)parentGrid.Parent;
            Entry amountEntry = parentGrid.FindByName<Entry>("Amount_Entry");

            // �ˬd�O�_��� Amount_Entry ����
            if (amountEntry != null)
            {
                amountEntry.IsReadOnly = true;
                var intTotal = CheckInput(amountEntry.Text);

                // �ק� Amount_Entry ���ݩ�
                Manager.UpdateCurrentTotal(parentBorder.DataId, intTotal - originalValue);
                SharedMethod.SetAppCached(Manager);
            }

            ImageButton pencilButton = parentGrid.FindByName<ImageButton>("pencil_ImageBTN");
            pencilButton.IsVisible = true;
            checkButton.IsVisible = false;
        }
        catch (ArgumentException ex)
        {
            App.Current.MainPage.DisplayAlert(ex.Message, "", "OK");
        }
        catch (Exception ex)
        {
            LocalFileLogger logger = new LocalFileLogger();
            logger.Log("RecordUpdateError:" + ex);
            App.Current.MainPage.DisplayAlert("�X�{���`���~�A�Э��s����", "�L�k�ư����pô�}�o��", "OK");
        }
    }

}