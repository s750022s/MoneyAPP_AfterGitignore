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

    //頁面生成時預設種類，IsCategory = true
    protected override void OnAppearing()
    {
        base.OnAppearing();
        DatasCollectionView.ItemsSource = behavior.SetPageData();
    }

    /// <summary>
    /// 返回上一頁
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
            MenuToggle_BTN.Text = "種類";
            DatasCollectionView.ItemsSource = behavior.SetPageData();
            Revise_Grid.IsVisible = false;
        }
        else
        {
            behavior = new AccountChlidClass(_dbManager);
            MenuToggle_BTN.Text = "帳戶";
            DatasCollectionView.ItemsSource = behavior.SetPageData();
            Revise_Grid.IsVisible = false;
        }
    }


    /// <summary>
    /// 點擊Border，會跳出該Border的資料
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
            DisplayAlert("出現異常錯誤，請重新嘗試", "無法排除請聯繫開發者", "OK");
        }
    }

    private async void Delete_BTN_Clicked(object sender, EventArgs e) 
    {
        try
        {
            var ans = await DisplayAlert("刪除無法復原，確定要刪除嗎?", "刪除後歷史資料會繼續顯示，\n但無法在新增/修改中再次選取", "刪除", "取消");
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
            DisplayAlert("出現異常錯誤，請重新嘗試", "無法排除請聯繫開發者", "OK");
        }
    }




}