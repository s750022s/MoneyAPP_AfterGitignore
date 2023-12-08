using MoneyAPP.Pages;

namespace MoneyAPP.Controls;

public partial class CustomTabBar : ContentView
{
    public CustomTabBar()
    {
        InitializeComponent();
    }

    /// <summary>
    ///�HButten��PageBind����{��H�AĲ�o��������������
    /// </summary>
    public void ToPage_Clicked(object sender, EventArgs e)
    {
        //�N����sender�ରButton,��K�᭱���ݩ�
        TabBarButton button = (TabBarButton)sender;
        string page = button.PageBind;


        //Butten����pageFunc�AKey�OPageBind
        var getPageDict = new Dictionary<string, Func<Page>>()
        {
            { "HomePage", () => new HomePage() },
            { "RecordAddPage", () => new RecordAddPage() },
            { "StatisticsPage", () => new StatisticsPage() },
            { "SetCategoryListPage", () => new SetCategoryListPage() }, 
            { "SettingsPage", () => new SettingsPage() }
        };

        Page newPage = getPageDict[page]();

        //�Q��Shell������s�����A�çR���쥻����
        Shell.Current.CurrentItem.CurrentItem.Items.Add((ShellContent)newPage);
        Shell.Current.CurrentItem.CurrentItem.Items.RemoveAt(0);
    }
}