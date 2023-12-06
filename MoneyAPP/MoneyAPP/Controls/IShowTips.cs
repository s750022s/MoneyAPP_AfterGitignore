namespace MoneyAPP.Controls;

/// <summary>
/// �P�_�����O�_��ܩ󭶭������O
/// </summary>
public class IShowTips
{
    /// <summary>
    /// ��e����P��Ʈw�Ĥ@����Ƥ���ۮt31�ѡA�N�ݭn���Tips
    /// </summary>
    /// <param name="firstday">��Ʈw�Ĥ@����Ƥ��(DateTime���A)</param>
    /// <returns>���true/�����false</returns>
    public static bool ISShowFullTips(DateTime firstday) 
    {
        if ((DateTime.Now - firstday).TotalDays > 30) 
        {
            return true;
        }
        return false;
    }
}