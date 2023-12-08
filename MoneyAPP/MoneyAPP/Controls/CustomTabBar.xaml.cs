using MoneyAPP.Pages;

namespace MoneyAPP.Controls;

public partial class CustomTabBar : ContentView
{
    public CustomTabBar()
    {
        InitializeComponent();
    }

    /// <summary>
    ///以Butten的PageBind為辨認對象，觸發切換對應的頁面
    /// </summary>
    public void ToPage_Clicked(object sender, EventArgs e)
    {
        //將物件sender轉為Button,方便後面拿屬性
        TabBarButton button = (TabBarButton)sender;
        string page = button.PageBind;


        //Butten對應pageFunc，Key是PageBind
        var getPageDict = new Dictionary<string, Func<Page>>()
        {
            { "HomePage", () => new HomePage() },
            { "RecordAddPage", () => new RecordAddPage() },
            { "StatisticsPage", () => new StatisticsPage() },
            { "SetCategoryListPage", () => new SetCategoryListPage() }, 
            { "SettingsPage", () => new SettingsPage() }
        };

        Page newPage = getPageDict[page]();

        //利用Shell導覽到新頁面，並刪除原本頁面
        Shell.Current.CurrentItem.CurrentItem.Items.Add((ShellContent)newPage);
        Shell.Current.CurrentItem.CurrentItem.Items.RemoveAt(0);
    }
}