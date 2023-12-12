using SQLite;


namespace MoneyAPP.Models
{
    /// <summary>
    /// 用來記錄收入與支出之相關訊息，暫時不實作。
    /// 未來可能用途：收入支出樣式
    /// </summary>
    [Table("isExpenses")]
    public class IsExpensesModel
    {
        /// <summary>
        /// 是否為支出，Pkey
        /// </summary>
        [PrimaryKey]
        public bool IsExpenses { get; set; }

        /// <summary>
        /// 正負代表符號
        /// </summary>
        [MaxLength(1)]
        public string PlusMinus { get; set; }

        /// <summary>
        /// 套用樣式
        /// </summary>
        public byte Style { get; set; }
    }
}