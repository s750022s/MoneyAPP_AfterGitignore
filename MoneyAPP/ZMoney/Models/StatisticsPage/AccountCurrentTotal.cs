using System.ComponentModel;

namespace ZMoney.Models
{
    /// <summary>
    /// 當前帳戶總額結構；顯示與修改當前帳戶總額。
    /// </summary>
    public class AccountCurrentTotal : INotifyPropertyChanged
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

        private int _id, _currentTotal;
        private string _name = "";

        /// <summary>
        /// 帳戶Id
        /// </summary>
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        /// <summary>
        /// 帳戶名稱
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        /// <summary>
        /// 帳戶當前總額
        /// </summary>
        public int CurrentTotal
        {
            get => _currentTotal;
            set
            {
                _currentTotal = value;
                OnPropertyChanged(nameof(CurrentTotal));
            }
        }

        /// <summary>
        /// 檢查輸入資料並建構當前帳戶總額結構。
        /// </summary>
        /// <param name="id">帳戶Id</param>
        /// <param name="name">帳戶名稱</param>
        /// <param name="currentTotalStr">帳戶當前總額</param>
        /// <exception cref="ArgumentException"></exception>
        public AccountCurrentTotal(int id, string name, string currentTotalStr)
        {
            Id = id;
            Name = name;

            string currentTotalClear = currentTotalStr.Replace(",", "");
            if (currentTotalClear == "")
            {
                throw new ArgumentException("金額請不要空白");
            }

            try
            {
                if (!int.TryParse(currentTotalClear, out int currentTotal))
                {
                    throw new ArgumentException("請不要輸入小數或文字，Money是不會有小數的喔!");
                }
                CurrentTotal = currentTotal;
            }
            catch (OverflowException)
            {
                throw new ArgumentException("想要輸入的金額已超過20億上限。");
            }
        }
    }
}
