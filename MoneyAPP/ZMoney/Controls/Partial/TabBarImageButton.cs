namespace ZMoney.Controls;

/// <summary>
/// 獨屬TabBar的按鈕=>
/// 在原有ImageButton功能上，加上要綁定於哪一個頁面，讓系統切換時能夠讀取
/// </summary>
public class TabBarImageButton : ImageButton
{
    public static readonly BindableProperty RouteBindProperty =
            BindableProperty.Create(nameof(RouteBind), typeof(string), typeof(TabBarImageButton), null);

    /// <summary>
    /// 標註綁定於哪一個頁面，對應Dictionary，以執行切換頁面
    /// </summary>
    public string RouteBind
    {
        get { return (string)GetValue(RouteBindProperty); }
        set { SetValue(RouteBindProperty, value); }
    }
}