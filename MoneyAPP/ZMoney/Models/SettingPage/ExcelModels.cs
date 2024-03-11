using System.ComponentModel;

namespace ZMoney.Models
{
    public class ExcelModels
    {
        /// <summary>
        /// 紀錄輸入的日期及時間
        /// </summary>
        [DisplayName("記錄日期時間")]
        public DateTime RecordDateTime { get; set; }

        /// <summary>
        /// 支出或收入
        /// </summary>
        [DisplayName("收入/支出")]
	    public string RevenueOrExpenses { get; set; } = "";

        /// <summary>
        /// 帳戶
        /// </summary>
        [DisplayName("帳戶名稱")]
        public string AccountName { get; set; } = "";

        /// <summary>
        /// 類別
        /// </summary>
        [DisplayName("類別名稱")]
        public string CategoryName { get; set; } = "";


        /// <summary>
        /// 紀錄項目
        /// </summary>
        [DisplayName("紀錄項目")]
        public string Description { get; set; } = "";


        /// <summary>
        /// 紀錄金額
        /// </summary>
        [DisplayName("紀錄金額")]
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
