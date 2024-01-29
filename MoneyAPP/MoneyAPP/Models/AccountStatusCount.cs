using System.ComponentModel;


namespace MoneyAPP.Models
{
    public class AccountStatusCount: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public int Id { get; set; }
        public string Name { get; set; }
        public int CurrentStatus { get; set; }

        public AccountStatusCount(int id, string name, string currentStatusStr)
        {
            Id = id;
            Name = name;

            var currentStatusStr2 = currentStatusStr.Replace(",", "");
            if (currentStatusStr2 == "")
            {
                throw new ArgumentException("金額請不要空白");
            }

            try
            {
                if(!int.TryParse(currentStatusStr2, out int currentStatus))
                {
                    throw new ArgumentException("請不要輸入小數或文字，Money是不會有小數的喔!");
                }
                CurrentStatus = currentStatus;
            }
            catch (OverflowException)
            {
                throw new ArgumentException("想要輸入的金額已超過20億上限。");
            }
        }
    }
}
