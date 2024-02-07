using System.ComponentModel;

namespace ZMoney.Models
{
    public class AccountCurrentTotal: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int CurrentStatus { get; set; }

        public void CheckEntry(string currentTotalStr) 
        {
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
                CurrentStatus = currentTotal;
            }
            catch (OverflowException)
            {
                throw new ArgumentException("想要輸入的金額已超過20億上限。");
            }
        }
    }
}
