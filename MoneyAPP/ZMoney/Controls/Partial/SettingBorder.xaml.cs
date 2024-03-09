namespace ZMoney.Controls;

/// <summary>
/// �W�ݳ]�w��Border=>
/// �b�즳Border�\��W�A�[�W�n�]�w�����D�B���СB�I���ĪG�C
/// </summary>
public partial class SettingBorder : ContentView
{
    public static readonly BindableProperty TitleProperty =
      BindableProperty.Create(nameof(Title), typeof(string), typeof(SettingBorder), "");

    /// <summary>
    /// ���D�ݩ�
    /// </summary>
    public string Title
    {
        get { return (string)GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }

    public static readonly BindableProperty DescriptionProperty =
       BindableProperty.Create(nameof(Description), typeof(string), typeof(SettingBorder), "");

    /// <summary>
    /// �����ݩ�
    /// </summary>
    public string Description
    {
        get { return (string)GetValue(DescriptionProperty); }
        set { SetValue(DescriptionProperty, value); }
    }

    /// <summary>
    /// �I���ƥ�
    /// </summary>
    public event EventHandler? Clicked;
   
    public SettingBorder()
    {
        InitializeComponent();
        InitializeGestureRecognizer();
    }

    /// <summary>
    /// �w�qClick�y�{;
    /// User�I��Border -> ����ѧO�� �ѧO�ýե�OnTapped -> �q��Clicked�ƥ�o�� -> Ĳ�o�����]�w��Clicked��k -> ���͵��G(����)
    /// </summary>
    void InitializeGestureRecognizer()
    {
        //����ѧO��,�Ω��ѧO�I�����
        var tapGestureRecognizer = new TapGestureRecognizer();

        //�� Tapped �ƥ�Ĳ�o�ɡA�N�ե� OnTapped ��k�C
        tapGestureRecognizer.Tapped += (sender, e) => OnTapped();
        GestureRecognizers.Add(tapGestureRecognizer);
    }

    /// <summary>
    /// �ƥ�q��
    /// </summary>
    void OnTapped()
    {
        Clicked?.Invoke(this, EventArgs.Empty);
    }
}