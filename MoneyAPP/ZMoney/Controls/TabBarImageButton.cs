namespace ZMoney.Controls;

/// <summary>
/// �W��TabBar�����s=>
/// �b�즳ImageButton�\��W�A�[�W�n�j�w����@�ӭ����A���t�Τ����ɯ��Ū��
/// </summary>
public class TabBarImageButton : ImageButton
{
    public static readonly BindableProperty RouteBindProperty =
            BindableProperty.Create(nameof(RouteBind), typeof(string), typeof(TabBarImageButton), null);

    /// <summary>
    /// �е��j�w����@�ӭ����A����Dictionary�A�H�����������
    /// </summary>
    public string RouteBind
    {
        get { return (string)GetValue(RouteBindProperty); }
        set { SetValue(RouteBindProperty, value); }
    }
}