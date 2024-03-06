namespace ZMoney.Controls;

/// <summary>
/// 客製化Border，新增屬性DataId
/// </summary>
public class CustomBorder : Border
{
    public static readonly BindableProperty DataIdProperty =
        BindableProperty.Create(nameof(Id), typeof(int), typeof(CustomBorder), null);

    /// <summary>
    /// 儲存回頭查詢時的ID，其可能是RecordId/AccountId/CategoryId
    /// </summary>
    public int DataId
    {
        get { return (int)GetValue(DataIdProperty); }
        set { SetValue(DataIdProperty, value); }
    }
}