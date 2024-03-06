using System.Collections.ObjectModel;
using MoneyAPP.Models;
using MoneyAPP.Controls;

namespace MoneyAPP.Pages;

public partial class StatisticsPage_SelectByCategory : ContentPage
{
    /// <summary>
    /// 資料繫結
    /// </summary>
    ObservableCollection<DataByCategory> datas = new ObservableCollection<DataByCategory>();
    public ObservableCollection<DataByCategory> Datas { get { return datas; } }
    private DateTime startDate, endDate;
    private int id;

    public StatisticsPage_SelectByCategory(DateTime start, DateTime end, int categoryId)
	{
		InitializeComponent();
        id = categoryId;
        startDate = start;
        endDate = end;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        GetDataByCategory();
        GetNameByCategory();
    }

        /// <summary>
        /// 返回上一頁
        /// </summary>
        private void BackButton_Clicked(object sender, EventArgs e)
    {
        Shell.Current.CurrentItem.CurrentItem.Items.Add(new StatisticsPage());
        Shell.Current.CurrentItem.CurrentItem.Items.RemoveAt(0);
    }

    /// <summary>
    /// 取得日期參數的所有資料(不包含已刪除者)
    /// </summary>
    public void GetDataByCategory()
    {
        var infos = App.ServiceRepo.GetDataByCategory(startDate, endDate, id);
        foreach (var info in infos) 
        {
            info.RecordTimeToString();
        }

        if (infos.Count != 0)
        {
            //資料繫結至CollectionView
            DatasCollectionView.ItemsSource = infos;
        }
        else 
        {
            NoRecord.IsVisible = true;
        }

    }

    public void GetNameByCategory()
    {
        if (App.CachedCategorys == null)
        {
            App.CachedCategorys = App.ServiceRepo.GetCategoryOrderBySequence();
        }
        var name = from category in App.CachedCategorys
                  where category.CategoryID == id
                  select category.Name;
        CategoryName_Label.Text = name.First();
    }


    /// <summary>
    /// 每筆資料都可被點擊，進入修改資料頁
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnBorderTapped(object sender, TappedEventArgs e)
    {
        RecordBorder recordBorder = (RecordBorder)sender;
        int id = recordBorder.RecordId;
        Shell.Current.CurrentItem.CurrentItem.Items.Add(new Pages.RecordRevisePage(id));
        Shell.Current.CurrentItem.CurrentItem.Items.RemoveAt(0);
    }


}