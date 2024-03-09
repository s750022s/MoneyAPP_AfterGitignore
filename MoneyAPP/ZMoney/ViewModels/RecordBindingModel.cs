using System.ComponentModel;
using ZMoney.Models;

namespace ZMoney.ViewModels
{
    /// <summary>
    /// 中繼資料結構；綁定Add/Revise Pages提供的資訊且進行檢查；
    /// </summary>
    public class RecordBindingModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 屬性變更事件，實作INotifyPropertyChanged。
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// 屬性變更通知
        /// </summary>
        /// <param name="propertyName">更新屬性名稱</param>
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 紀錄Id
        /// </summary>
        public int Id { get; set; }


        private DateTime _recordDay;

        /// <summary>
        /// 紀錄輸入的日期，綁定日期選擇器
        /// </summary>
        public DateTime RecordDay
        {
            get => _recordDay;
            set
            {
                _recordDay = value;
                OnPropertyChanged(nameof(RecordDay));
            }
        }

        private TimeSpan _recordTime;

        /// <summary>
        /// 紀錄輸入的時間，綁定時間選擇器
        /// </summary>
	    public TimeSpan RecordTime
        {
            get => _recordTime;
            set
            {
                _recordTime = value;
                OnPropertyChanged(nameof(RecordTime));
            }
        }

        private bool _isIncome;

        /// <summary>
        /// 是否為收入，綁定Income的IsChecked
        /// </summary>
        public bool IsIncome
        {
            get => _isIncome;
            set
            {
                _isIncome = value;
                OnPropertyChanged(nameof(IsIncome));
            }
        }

        private bool _isExpense;

        /// <summary>
        /// 是否為支出，綁定Expense的IsChecked
        /// </summary>
	    public bool IsExpense
        {
            get => _isExpense;
            set
            {
                _isExpense = value;
                OnPropertyChanged(nameof(IsExpense));
            }
        }

        private int _accountIndex;

        /// <summary>
        /// 帳戶選單位置，等同於Sequence，需進行比對找出ID
        /// </summary>
        public int AccountIndex
        {
            get => _accountIndex;
            set
            {
                _accountIndex = value;
                OnPropertyChanged(nameof(AccountIndex));
            }
        }

        private int _categoryIndex;

        /// <summary>
        /// 類別選單位置，等同於Sequence，需進行比對找出ID
        /// </summary>
        public int CategoryIndex
        {
            get => _categoryIndex;
            set
            {
                _categoryIndex = value;
                OnPropertyChanged(nameof(CategoryIndex));
            }
        }

        private string _description = "";

        /// <summary>
        /// 紀錄項目
        /// </summary>
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        private string _strAmountOfMoney = "";

        /// <summary>
        /// 紀錄金額
        /// </summary>
        public string StrAmountOfMoney
        {
            get => _strAmountOfMoney;
            set
            {
                _strAmountOfMoney = value;
                OnPropertyChanged(nameof(StrAmountOfMoney));
            }
        }

        /// <summary>
        /// 新增時的預設值
        /// </summary>
        public void AddDefault()
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

        /// <summary>
        /// 取得的DB資料整理成對應欄位並通知UI
        /// </summary>
        /// <param name="record">紀錄SQLModel</param>
        public void GetRecordByRecordModel(RecordModel record)
        {
            Id = record.Id;
            RecordDay = record.RecordDateTime;
            RecordTime = record.RecordDateTime.TimeOfDay;
            IsExpense = record.IsExpenses;
            IsIncome = !record.IsExpenses;
            AccountIndex = App.CachedAccounts.FirstOrDefault(x => record.AccountId == x.Id)?.Sequence ?? 0;
            CategoryIndex = App.CachedCategorys.FirstOrDefault(x => record.CategoryId == x.Id)?.Sequence ?? 0;
            Description = record.Description ?? string.Empty;
            StrAmountOfMoney = (record.IsExpenses ? record.AmountOfMoney * -1 : record.AmountOfMoney).ToString("N0");
        }

        /// <summary>
        /// 檢查欄位資訊是否符合標準。
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public void CheckEntry()
        {
            //說明
            if (Description.Length > 30)
            {
                throw new ArgumentException("為了方便辨認，請不要超過30個字");
            }


            //金額
            if (StrAmountOfMoney == "")
            {
                throw new ArgumentException("金額請不要空白");
            }
            else if (StrAmountOfMoney[0] == '-')
            {
                throw new ArgumentException("金額請填正數，支出將自動換算");
            }

            //轉換成數字
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

        /// <summary>
        /// UI資料檢查後轉換成RecordModel，方便更新資料庫。
        /// </summary>
        /// <returns>紀錄SQLModel</returns>
        public RecordModel SetRecordToRecordModel()
        {
            CheckEntry();
            int.TryParse(StrAmountOfMoney.Replace(",", ""), out int result);

            RecordModel record = new RecordModel
            {
                Id = Id,
                RecordDateTime = RecordDay + RecordTime, //需要檢查是否可行
                IsExpenses = IsExpense,
                AccountId = App.CachedAccounts.FirstOrDefault(x => AccountIndex == x.Sequence)?.Id ?? 0,
                CategoryId = App.CachedCategorys.FirstOrDefault(x => CategoryIndex == x.Sequence)?.Id ?? 0,
                Description = Description == "" ? null : Description,
                AmountOfMoney = IsExpense ? result * -1 : result,
                IsDelete = false
            };
            return record;
        }
    }
}
