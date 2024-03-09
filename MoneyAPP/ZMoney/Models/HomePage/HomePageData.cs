namespace ZMoney.Models
{
    /// <summary>
    /// 首頁客製化的紀錄類別
    /// </summary>
    public class HomePageData
    {
        /// <summary>
        /// 紀錄Id，用於回頭尋找完整資料
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 種類名稱
        /// </summary>
        public string CategoryName { get; set; } = "";

        /// <summary>
        /// 紀錄物件
        /// </summary>
        public string Description { get; set; } = "";

        /// <summary>
        /// 紀錄金額(收入正數，支出負數)
        /// </summary>
        public int AmountOfMoney { get; set; }

        public HomePageData(int id, string categoryName, string description, int amountOfMoney)
        {
            Id = id;
            CategoryName = categoryName;
            Description = description;
            AmountOfMoney = amountOfMoney;
        }
    }
}
