using ZMoney.Services;
using ZMoney.ViewModels;
using ZMoney.Controls;
using System.Collections.ObjectModel;

namespace ZMoney.Pages;

public partial class ListSetting : ContentPage
{
    private DbManager _dbManager;
    private ListSetViewModel _dataManager;

    

    public ListSetting(DbManager dbManager)
	{
		InitializeComponent();
        _dbManager = dbManager;
        _dataManager = new ListSetViewModel(_dbManager);

    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        DatasCollectionView.ItemsSource = _dataManager.SetPageData();
        
    }

    /// <summary>
    /// 返回上一頁
    /// </summary>
    private void BackBTN_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("..");
    }

    /// <summary>
    /// 點擊Border，會跳出該Border的資料
    /// </summary>
    private void OnBorderTapped(object sender, TappedEventArgs e)
    {
        CustomBorder customBorder =(CustomBorder) sender;
        Revise_Grid.IsVisible = true;
        Blank_BoxView.IsVisible = false;

        if (customBorder.DataId == 999)
        {
            Name_Entry.Text = "";
            Sequence_Entry.Text = "";
            return;
        }
        var data = _dataManager.GetContent(customBorder.DataId);
        Name_Entry.Text = data.Name;
        Sequence_Entry.Text = data.Sequence.ToString();
    }

    private void Save_BTN_Clicked(object sender, EventArgs e) 
    {
        try
        {



        }
        catch (ArgumentException ex)
        {
            DisplayAlert(ex.ToString(), "", "OK");
        }

    }

    private  void Delete_BTN_Clicked(object sender, EventArgs e) { }

    private void CategoryChangeAccount_Clicked(object sender, EventArgs e) 
    {
        MenuToggle_BTN.Text = _dataManager.CategoryChangeAccount_Clicked();
        DatasCollectionView.ItemsSource = _dataManager.SetPageData();
    }



}