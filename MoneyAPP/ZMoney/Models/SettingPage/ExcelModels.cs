namespace ZMoney.Models
{
    public class ExcelModels
    {
        /// <summary>
        /// 紀錄輸入的日期及時間
        /// </summary>
	    public DateTime RecordDateTime { get; set; }

        /// <summary>
        /// 支出或收入
        /// </summary>
	    public string RevenueOrExpenses { get; set; } = "";

        /// <summary>
        /// 帳戶
        /// </summary>
        public string AccountName { get; set; } = "";

        /// <summary>
        /// 類別
        /// </summary>
        public string CategoryName { get; set; } = "";


        /// <summary>
        /// 紀錄項目
        /// </summary>
        public string Description { get; set; } = "";


        /// <summary>
        /// 紀錄金額
        /// </summary>
        public int AmountOfMoney { get; set; }

        public ExcelModels(DateTime recordDateTime, 
                        string revenueOrExpenses, 
                        string account, 
                        string category, 
                        string description, 
                        int amountOfMoney) 
        {
            RecordDateTime = recordDateTime;
            RevenueOrExpenses = revenueOrExpenses;
            AccountName = account;
            CategoryName = category;
            Description = description;
            AmountOfMoney = amountOfMoney;

        }

    }
}
