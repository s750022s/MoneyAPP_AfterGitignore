using ZMoney.Controls;
using ZMoney.Services;
using ZMoney.ViewModels;


namespace ZMoney.Pages;

public partial class ListSetting : ContentPage
{
    private DbManager _dbManager;
    private ListBaseClass behavior;
    private bool IsCategory = true;
    private int cacheID;

    public ListSetting(DbManager dbManager)
	{
		InitializeComponent();
        _dbManager = dbManager;
        behavior = new CategroyChildClass(_dbManager);
    }

    //�����ͦ��ɹw�]�����AIsCategory = true
    protected override void OnAppearing()
    {
        base.OnAppearing();
        DatasCollectionView.ItemsSource = behavior.SetPageData();
    }

    /// <summary>
    /// ��^�W�@��
    /// </summary>
    private void BackBTN_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("..");
    }

    private void CategoryOrAccount_Clicked(object sender, EventArgs e)
    {
        IsCategory = !IsCategory;
        if (IsCategory) 
        {
            behavior = new CategroyChildClass(_dbManager);
            MenuToggle_BTN.Text = "����";
            DatasCollectionView.ItemsSource = behavior.SetPageData();
            Revise_Grid.IsVisible = false;
        }
        else
        {
            behavior = new AccountChlidClass(_dbManager);
            MenuToggle_BTN.Text = "�b��";
            DatasCollectionView.ItemsSource = behavior.SetPageData();
            Revise_Grid.IsVisible = false;
        }
    }


    /// <summary>
    /// �I��Border�A�|���X��Border�����
    /// </summary>
    private void OnBorderTapped(object sender, TappedEventArgs e)
    {
        CustomBorder customBorder =(CustomBorder) sender;
        Revise_Grid.IsVisible = true;
        Blank_BoxView.IsVisible = false;
        cacheID = customBorder.DataId;

        if (cacheID == 999)
        {
            Name_Entry.Text = "";
            Sequence_Entry.Text = "0";
            return;
        }
        Name_Entry.Text = behavior.GetContent(cacheID , out string sequenceStr);
        Sequence_Entry.Text = sequenceStr;
    }

    private void Save_BTN_Clicked(object sender, EventArgs e) 
    {
        try
        {
            if (cacheID == 999) 
            {
                behavior.AddSave(Name_Entry.Text, Sequence_Entry.Text);
            }
            else 
            {
                behavior.UpdateSave(Name_Entry.Text, Sequence_Entry.Text, cacheID);
            }
            DatasCollectionView.ItemsSource = behavior.SetPageData();
            Revise_Grid.IsVisible = false;
        }
        catch (ArgumentException ex) 
        {
            DisplayAlert(ex.ToString(), "", "OK");
        }

        catch(Exception ex) 
        {
            LocalFileLogger logger = new LocalFileLogger();
            logger.Log("RecordUpdateError:" + ex);
            DisplayAlert("�X�{���`���~�A�Э��s����", "�L�k�ư����pô�}�o��", "OK");
        }
    }

    private async void Delete_BTN_Clicked(object sender, EventArgs e) 
    {
        try
        {
            var ans = await DisplayAlert("�R���L�k�_��A�T�w�n�R����?", "�R������v��Ʒ|�~����ܡA\n���L�k�b�s�W/�ק襤�A�����", "�R��", "����");
            if (ans == true)
            {
                behavior.Delete(cacheID);
                DatasCollectionView.ItemsSource = behavior.SetPageData();
                Revise_Grid.IsVisible = false;
            }
        }
        catch (Exception ex) 
        {
            LocalFileLogger logger = new LocalFileLogger();
            logger.Log("RecordUpdateError:" + ex);
            DisplayAlert("�X�{���`���~�A�Э��s����", "�L�k�ư����pô�}�o��", "OK");
        }
    }




}