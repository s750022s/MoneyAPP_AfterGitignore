namespace MoneyAPP.Controls;

/// <summary>
/// 判斷提醒是否顯示於頁面的類別
/// </summary>
public class IShowTips
{
    /// <summary>
    /// 當前日期與資料庫第一筆資料日期相差31天，就需要顯示Tips
    /// </summary>
    /// <param name="firstday">資料庫第一筆資料日期(DateTime型態)</param>
    /// <returns>顯示true/不顯示false</returns>
    public static bool ISShowFullTips(DateTime firstday) 
    {
        if ((DateTime.Now - firstday).TotalDays > 30) 
        {
            return true;
        }
        return false;
    }
}