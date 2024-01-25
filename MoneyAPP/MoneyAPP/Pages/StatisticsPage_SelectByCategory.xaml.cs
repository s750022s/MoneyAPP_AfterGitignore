using System.Collections.ObjectModel;
using MoneyAPP.Models;
using MoneyAPP.Controls;

namespace MoneyAPP.Pages;

public partial class StatisticsPage_SelectByCategory : ContentPage
{
    /// <summary>
    /// ���ô��
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
        /// ��^�W�@��
        /// </summary>
        private void BackButton_Clicked(object sender, EventArgs e)
    {
        Shell.Current.CurrentItem.CurrentItem.Items.Add(new StatisticsPage());
        Shell.Current.CurrentItem.CurrentItem.Items.RemoveAt(0);
    }

    /// <summary>
    /// ���o����Ѽƪ��Ҧ����(���]�t�w�R����)
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
            //���ô����CollectionView
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
    /// �C����Ƴ��i�Q�I���A�i�J�ק��ƭ�
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