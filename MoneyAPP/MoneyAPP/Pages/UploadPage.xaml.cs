using MoneyAPP.Controls;

namespace MoneyAPP.Pages;

public partial class UploadPage : ContentPage
{
	public UploadPage()
	{
        InitializeComponent();
        
        //暫時寫死，實作時須修正
        DateTime sDate = Convert.ToDateTime("2023-11-07 15:50:39");
        
        ISShowFullTips(IShowTips.ISShowFullTips(sDate));
    }

    /// <summary>
    /// 當收到不顯示時，調整fullMoonTips與hotdog_Image的屬性
    /// </summary>
    /// <param name="show">是否顯示Tips</param>
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