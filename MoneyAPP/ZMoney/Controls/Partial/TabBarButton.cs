namespace ZMoney.Controls;

/// <summary>
/// 獨屬TabBar的按鈕=>
/// 在原有Butten功能上，加上要綁定於哪一個頁面，讓系統切換時能夠讀取
/// </summary>
public class TabBarButton : Button
{
    public static readonly BindableProperty RouteBindProperty =
            BindableProperty.Create(nameof(RouteBind), typeof(string), typeof(TabBarButton), null);

    /// <summary>
    /// 標註綁定於哪一個頁面，對應Dictionary，以執行切換頁面
    /// </summary>
    public string RouteBind
    {
        get { return (string)GetValue(RouteBindProperty); }
        set { SetValue(RouteBindProperty, value); }
    }
}