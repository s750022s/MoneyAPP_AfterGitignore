namespace ZMoney.Controls;

/// <summary>
/// �W��TabBar�����s=>
/// �b�즳Butten�\��W�A�[�W�n�j�w����@�ӭ����A���t�Τ����ɯ��Ū��
/// </summary>
public class TabBarButton : Button
{
    public static readonly BindableProperty RouteBindProperty =
            BindableProperty.Create(nameof(RouteBind), typeof(string), typeof(TabBarButton), null);

    /// <summary>
    /// �е��j�w����@�ӭ����A����Dictionary�A�H�����������
    /// </summary>
    public string RouteBind
    {
        get { return (string)GetValue(RouteBindProperty); }
        set { SetValue(RouteBindProperty, value); }
    }
}