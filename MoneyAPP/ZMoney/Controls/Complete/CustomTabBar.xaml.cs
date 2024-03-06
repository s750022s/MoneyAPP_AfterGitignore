namespace ZMoney.Controls;

public partial class CustomTabBar : ContentView
{
	public CustomTabBar()
	{
		InitializeComponent();
	}

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