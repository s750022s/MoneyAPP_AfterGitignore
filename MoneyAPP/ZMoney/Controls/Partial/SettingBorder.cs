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
}