namespace ZMoney.Controls;

public partial class SettingBorder : ContentView
{
    public static readonly BindableProperty TitleProperty =
      BindableProperty.Create(nameof(Title), typeof(string), typeof(SettingBorder), "");

    public string Title
    {
        get { return (string)GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }

    public static readonly BindableProperty DescriptionProperty =
       BindableProperty.Create(nameof(Description), typeof(string), typeof(SettingBorder), "");

    public string Description
    {
        get { return (string)GetValue(DescriptionProperty); }
        set { SetValue(DescriptionProperty, value); }
    }

    public event EventHandler Clicked;
   



    public SettingBorder()
    {
        InitializeComponent();
        InitializeGestureRecognizer();
    }
    /// <summary>
    /// 定義Click流程;
    /// User點擊Border -> 手勢識別器 識別並調用OnTapped -> 通知Clicked事件發生 -> 觸發頁面設定的Clicked方法 -> 產生結果(換頁)
    /// </summary>
    void InitializeGestureRecognizer()
    {
        //手勢識別器,用於識別點擊手勢
        var tapGestureRecognizer = new TapGestureRecognizer();

        //當 Tapped 事件觸發時，將調用 OnTapped 方法。
        tapGestureRecognizer.Tapped += (sender, e) => OnTapped();
        GestureRecognizers.Add(tapGestureRecognizer);
    }

    void OnTapped()
    {
        Clicked?.Invoke(this, EventArgs.Empty);
    }
}