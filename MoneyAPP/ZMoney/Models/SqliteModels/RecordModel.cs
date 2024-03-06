using SQLite;

namespace ZMoney.Models
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
        public int Id { get; set; }

        /// <summary>
        /// 紀錄輸入的日期及時間
        /// </summary>
	    public DateTime RecordDateTime { get; set; }

        /// <summary>
        /// 是否為支出，計算時需*-1
        /// </summary>
	    public bool IsExpenses { get; set; }

        /// <summary>
        /// 帳戶ID，用以查找AccountModel
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// 類別ID，用以查找AccountModel
        /// </summary>
        public int CategoryId { get; set; }

#nullable enable
        /// <summary>
        /// 紀錄項目
        /// </summary>
        [MaxLength(30)]
        public string? Description { get; set; }
#nullable disable

        /// <summary>
        /// 紀錄金額
        /// </summary>
        public int AmountOfMoney { get; set; }

        /// <summary>
        /// 是否已刪除
        /// </summary>
        public bool IsDelete { get; set; }
    }
}