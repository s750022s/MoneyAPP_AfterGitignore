using MoneyAPP.Controls;

namespace MoneyAPP.Pages;

public partial class UploadPage : ContentPage
{
	public UploadPage()
	{
        InitializeComponent();
        
        //�Ȯɼg���A��@�ɶ��ץ�
        DateTime sDate = Convert.ToDateTime("2023-11-07 15:50:39");
        
        ISShowFullTips(IShowTips.ISShowFullTips(sDate));
    }

    /// <summary>
    /// ���줣��ܮɡA�վ�fullMoonTips�Photdog_Image���ݩ�
    /// </summary>
    /// <param name="show">�O�_���Tips</param>
    protected void ISShowFullTips(bool show)
    { 
        if (show == false) 
        {
            FullMoonTips_LB.IsEnabled = false;
            FullMoonTips_LB.IsVisible = false;
            Hotdog_Image.Margin = new Thickness(0, 0, 0, 30);
        }
    }
}