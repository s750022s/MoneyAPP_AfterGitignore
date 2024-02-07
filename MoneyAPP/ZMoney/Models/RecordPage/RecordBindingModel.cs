using System.ComponentModel;

namespace ZMoney.Models
{
    /// <summary>
    /// 中繼資料結構；綁定Add/Revise Pages提供的資訊且進行檢查；
    /// </summary>
    public class RecordBindingModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// 紀錄Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 紀錄輸入的日期，綁定日期選擇器
        /// </summary>
	    public DateTime RecordDay { get; set; }

        /// <summary>
        /// 紀錄輸入的時間，綁定時間選擇器
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
        public string Description { get; set; } = "";

        /// <summary>
        /// 紀錄金額
        /// </summary>
        public string StrAmountOfMoney { get; set; } = "";

        /// <summary>
        /// 新增時的預設值
        /// </summary>
        public RecordBindingModel() 
        {
            Id = -1;
            RecordDay = DateTime.Today;
            RecordTime = DateTime.Now.TimeOfDay;
            IsExpense = true;
            IsIncome = false;
            AccountIndex = 0;
            CategoryIndex = 0;
            Description = "";
            StrAmountOfMoney = "";
        }

        public RecordBindingModel(RecordModel record)
        {
            Id = record.Id;
            RecordDay = record.RecordDateTime;
            RecordTime = record.RecordDateTime.TimeOfDay;
            IsExpense = record.IsExpenses;
            IsIncome = !record.IsExpenses;
            AccountIndex = App.CachedAccounts.FirstOrDefault(x => record.Id == x.Id).Sequence;
            CategoryIndex = App.CachedCategorys.FirstOrDefault(x => record.Id == x.Id).Sequence;
            Description = record.Description;
            StrAmountOfMoney = (record.IsExpenses? record.AmountOfMoney * -1: record.AmountOfMoney).ToString("N0");
        }

        public void CheckEntry() 
        {
            if (Description.Length > 30) 
            {
                throw new ArgumentException("為了方便辨認，請不要超過30個字");
            }

            if (StrAmountOfMoney == "")
            {
                throw new ArgumentException("金額請不要空白");
            }
            else if (StrAmountOfMoney[0] == '-')
            {
                throw new ArgumentException("金額請填正數，支出將自動換算");
            }

            try
            {
                if (!int.TryParse(StrAmountOfMoney.Replace(",", ""), out int result))
                {
                    throw new ArgumentException("請不要輸入小數或文字，Money是不會有小數的喔!");
                }
            }
            catch (OverflowException)
            {
                throw new ArgumentException("想要輸入的金額已超過20億上限。");
            }
        }


        public RecordModel SetRecordToRecordModel() 
        {
            int.TryParse(StrAmountOfMoney.Replace(",", ""), out int result);

            RecordModel record = new RecordModel
            {
                Id = Id,
                RecordDateTime = RecordDay + RecordTime, //需要檢查是否可行
                IsExpenses = IsExpense,
                AccountId = App.CachedAccounts.FirstOrDefault(x => AccountIndex == x.Sequence).Id,
                CategoryId = App.CachedCategorys.FirstOrDefault(x => CategoryIndex == x.Sequence).Id,
                Description = Description,
                AmountOfMoney = IsExpense ? result * -1 : result,
                IsDelete = false
            };
            return record;
        }
    }
}
