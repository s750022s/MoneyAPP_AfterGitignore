
namespace MoneyAPP.Models
{
    /// <summary>
    /// 首頁客製化的紀錄類別
    /// </summary>
    public class HomePageData
    {
        /// <summary>
        /// 紀錄Id，用於回頭尋找完整資料
        /// </summary>
        public int RecordID { get; set; }

        /// <summary>
        /// 種類名稱
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 紀錄物件
        /// </summary>
        public string Item { get; set; }

        /// <summary>
        /// 紀錄金額(收入正數，支出負數)
        /// </summary>
        public int Amount { get; set; }
    }
}
