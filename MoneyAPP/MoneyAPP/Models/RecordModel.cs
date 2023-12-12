using SQLite;

namespace MoneyAPP.Models
{
    /// <summary>
    /// 紀錄資料結構
    /// </summary>
    [Table("records")]
    public class RecordModel
    {
        /// <summary>
        /// 帳戶ID，PKey，會自動帶入不重複的值
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int RecordID { get; set; }

        /// <summary>
        /// 紀錄輸入的日期
        /// </summary>
	    public DateTime RecordDay { get; set; }

        /// <summary>
        /// 紀錄輸入的時間(HH:mm)
        /// </summary>
	    public TimeSpan RecordTime { get; set; }

        /// <summary>
        /// 是否為支出，計算時需*-1
        /// </summary>
	    public bool IsExpenses { get; set; }

        /// <summary>
        /// 帳戶ID，用以查找AccountModel
        /// </summary>
        public int AccountID { get; set; }

        /// <summary>
        /// 類別ID，用以查找AccountModel
        /// </summary>
        public int CategoryID { get; set; }

        /// <summary>
        /// 紀錄項目
        /// </summary>
        [MaxLength(30)]
        public string? Item { get; set; }

        /// <summary>
        /// 紀錄金額
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// 紀錄新增時間
        /// </summary>
        public DateTime? LastUpdatedTime { get; set; }

        /// <summary>
        /// 是否已刪除
        /// </summary>
        public bool IsDelete { get; set; }
    }
}