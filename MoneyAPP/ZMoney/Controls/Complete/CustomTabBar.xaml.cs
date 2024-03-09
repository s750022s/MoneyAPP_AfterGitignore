namespace ZMoney.Controls;

public partial class CustomTabBar : ContentView
{
    /// <summary>
    /// 自定義的TabBar
    /// </summary>
	public CustomTabBar()
    {
        InitializeComponent();
    }

    /// <summary>
    /// 點擊產生換頁效果。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	public void ToPage_Clicked(object sender, EventArgs e)
    {
        string route;

        //將物件sender轉為Button,方便後面拿屬性
        if (sender.GetType().Name == "TabBarButton")
        {
            TabBarButton button = (TabBarButton)sender;
            route = button.RouteBind;
        }
        else
        {
            TabBarImageButton button = (TabBarImageButton)sender;
            route = button.RouteBind;
        }

        var currectRouteList = Shell.Current.CurrentState.Location.ToString().Split("/");
        var currectRoute = currectRouteList[currectRouteList.Length - 1];
        var currectRouteMain = currectRoute == "MainPage" ? "Home" : currectRoute;

        if (route != currectRouteMain)
        {
            Shell.Current.GoToAsync(route);
        }
    }
}