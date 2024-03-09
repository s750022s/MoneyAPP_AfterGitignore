namespace ZMoney.Controls;

public partial class CustomTabBar : ContentView
{
    /// <summary>
    /// �۩w�q��TabBar
    /// </summary>
	public CustomTabBar()
    {
        InitializeComponent();
    }

    /// <summary>
    /// �I�����ʹ����ĪG�C
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	public void ToPage_Clicked(object sender, EventArgs e)
    {
        string route;

        //�N����sender�ରButton,��K�᭱���ݩ�
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