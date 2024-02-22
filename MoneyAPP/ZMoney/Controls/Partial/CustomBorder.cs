namespace ZMoney.Controls;

/// <summary>
/// �Ȼs��Border�A�s�W�ݩ�DataId
/// </summary>
public class CustomBorder : Border
{
    public static readonly BindableProperty DataIdProperty =
        BindableProperty.Create(nameof(Id), typeof(int), typeof(CustomBorder), null);

    /// <summary>
    /// �x�s�^�Y�d�߮ɪ�ID�A��i��ORecordId/AccountId/CategoryId
    /// </summary>
    public int DataId
    {
        get { return (int)GetValue(DataIdProperty); }
        set { SetValue(DataIdProperty, value); }
    }
}