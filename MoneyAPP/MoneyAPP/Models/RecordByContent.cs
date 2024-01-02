using System.ComponentModel;

namespace MoneyAPP.Models
{
    /// <summary>
    /// 中繼資料結構；綁定Add/Revise Pages提供的資訊且進行逐項檢查；
    /// </summary>
    public class RecordByContent:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 帳戶ID，當新增時為-1，當修改時為所屬ID
        /// </summary>
        public int RecordID { get; set; }

        /// <summary>
        /// 紀錄輸入的日期
        /// </summary>
	    public DateTime RecordDay { get; set; }

        /// <summary>
        /// 紀錄輸入的時間
        /// </summary>
	    public TimeSpan RecordTime { get; set; }

        /// <summary>
        /// 是否為收入，綁定Income的IsChecked
        /// </summary>
	    public bool IsIncome { get; set; }

        /// <summary>
        /// 是否為支出，綁定Expense的IsChecked
        /// </summary>
	    public bool IsExpense { get; set; }

        /// <summary>
        /// 帳戶選單位置，等同於Sequence，需進行比對找出ID
        /// </summary>
        public int AccountIndex { get; set; }

        /// <summary>
        /// 類別選單位置，等同於Sequence，需進行比對找出ID
        /// </summary>
        public int CategoryIndex { get; set; }

        /// <summary>
        /// 紀錄項目
        /// </summary>
        public string Item { get; set; }

        /// <summary>
        /// 紀錄金額
        /// </summary>
        public string Amount { get; set; }


        public void AddDefault()
        {
            RecordID = -1;
            RecordDay = DateTime.Today;
            RecordTime = DateTime.Now.TimeOfDay;
            IsExpense = true;
            IsIncome = false;
            AccountIndex = 0;
            CategoryIndex = 0;
            Item = "";
            Amount = "";
        }

        public bool FiledCheck() 
        {
            if (Item.Length > 30) 
            {
                MessagingCenter.Send<object,string>(this, "Error","為了方便辨認，請不要超過30個字");
                return false;
            };

            if (Amount == "") 
            {
                MessagingCenter.Send<object, string>(this, "Error", "金額請不要空白");
                return false;
            };

            try
            {
                Amount = Amount.Replace(",", "");
                int amount = Convert.ToInt32(Amount);
                return true;
            }
            catch (OverflowException) 
            {
                MessagingCenter.Send<object, string>(this, "Error", "想要輸入的金額已超過20億上限。");
                return false;
            }
            catch (Exception)
            {
                MessagingCenter.Send<object, string>(this, "Error", "請不要輸入小數或文字，Money是不會有小數的喔!");
                return false;
            }
            
        }

        public RecordModel SetRecordToRecordModel() 
        {
            int amount;

            if (IsExpense == true)
            {
               amount = Convert.ToInt32(Amount) * -1;
            }
            else
            {
               amount = Convert.ToInt32(Amount);
            }


            RecordModel record = new RecordModel
            {
                RecordDay = RecordDay,
                RecordTime = RecordTime,
                IsExpenses = IsExpense,
                Item = Item,
                IsDelete = false,
                AccountID = App.CachedAccounts.FirstOrDefault(a => AccountIndex == a.Sequence).AccountID,
                CategoryID = App.CachedCategorys.FirstOrDefault(c=> CategoryIndex == c.Sequence).CategoryID,
                Amount = amount
            }; 

            return record;
        }

        public void GetRecordByRecordModel(RecordModel info) 
        {
            RecordID = info.RecordID;
            RecordDay = info.RecordDay;
            RecordTime = info.RecordTime;
            IsExpense = info.IsExpenses;
            IsIncome = !info.IsExpenses;
            Item = info.Item;

            if (IsExpense == true) 
            {
                Amount = ((info.Amount) * -1).ToString("N0");
            }
            else 
            { 
                Amount = info.Amount.ToString("N0"); 
            }

            AccountIndex = App.CachedAccounts.FirstOrDefault(a => info.AccountID == a.AccountID).Sequence;
            CategoryIndex = App.CachedCategorys.FirstOrDefault(c => info.CategoryID == c.CategoryID).Sequence;
        }
    }
}
