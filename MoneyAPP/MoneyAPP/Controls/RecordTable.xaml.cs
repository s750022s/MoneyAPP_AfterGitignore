using System.Collections.ObjectModel;
using MoneyAPP.Models;


namespace MoneyAPP.Controls;

/// <summary>
/// �u�ʬ����A�|�H��J��ƼW��
/// </summary>
public partial class RecordTable : ContentView
{

    /// <summary>
    /// ���ô��
    /// </summary>
    ObservableCollection<HomePageData> datas = new ObservableCollection<HomePageData>();
    public ObservableCollection<HomePageData> Datas { get { return datas; } }

    public RecordTable()
    {
        InitializeComponent();
    }

    /// <summary>
    /// ���o����Ѽƪ��Ҧ����(���]�t�w�R����)
    /// 1. ���ô����CollectionView
    /// 2. �p�����`�B
    /// </summary>
    /// <param name="recordDay">�n�d�ߪ����</param>
    /// <returns>����`�B</returns>
    public int GetOnedayInfo(DateTime recordDay)
    {
        var infos = App.ServiceRepo.GetHomePageData(recordDay);

        //���ô����CollectionView
        DatasCollectionView.ItemsSource = infos;

        //�p�����`�B
        int totalAmount = 0;
        foreach (var item in infos)
        {
            totalAmount += item.Amount;
        }
        return totalAmount;
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