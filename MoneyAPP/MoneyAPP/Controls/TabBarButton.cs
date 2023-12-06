namespace MoneyAPP.Controls;

/// <summary>
/// �W��TabBar�����s=>
/// �b�즳Butten�\��W�A�[�W�n�j�w����@�ӭ����A���t�Τ����ɯ��Ū��
/// </summary>
public class TabBarButton : Button
{
    public static readonly BindableProperty PageBindProperty =
            BindableProperty.Create(nameof(PageBind), typeof(string), typeof(TabBarButton), null);

    /// <summary>
    /// �е��j�w����@�ӭ����A����Dictionary�A�H�����������
    /// </summary>
    public string PageBind
    {
        get { return (string)GetValue(PageBindProperty); }
        set { SetValue(PageBindProperty, value); }
    }
}