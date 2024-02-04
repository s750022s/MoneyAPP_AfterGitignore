namespace ZMoney.Models
{
    /// <summary>
    /// 統計頁第二層客製化資料，包含各分組的紀錄顯示資料。
    /// </summary>
    public class RecordsFromGroup
    {
        /// <summary>
        /// 紀錄Id，用於回頭尋找完整資料
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 記錄日期時間
        /// </summary>
        public DateTime RecordDateTime { get; set; }

        /// <summary>
        /// 紀錄物件
        /// </summary>
        public string Description { get; set; } = "";

        /// <summary>
        /// 紀錄金額(收入正數，支出負數)
        /// </summary>
        public int AmountOfMoney { get; set; }
    }
}
