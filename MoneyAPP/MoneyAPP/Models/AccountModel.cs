using SQLite;
namespace MoneyAPP.Models
{
    /// <summary>
    /// 帳戶資料結構
    /// </summary>
    [Table("accounts")]
    public class AccountModel
    {
        /// <summary>
        /// 帳戶ID，PKey，會自動帶入不重複的值
        /// </summary>
        [PrimaryKey, AutoIncrement] 
        public int AccountID { get; set; }

        /// <summary>
        /// 帳戶名稱，最長6個字
        /// </summary>
        [MaxLength(6)]
        public string Name { get; set; }

        /// <summary>
        /// 帳戶順序，用以選單排序，值為0者為預設選項，-1者已被刪除
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// 使用者自訂總額
        /// </summary>
        public int UserDefault { get; set; }

        /// <summary>
        /// 使用者自訂總額輸入時間
        /// </summary>
        public int UserDefault_DateTime { get; set; }
    }
}