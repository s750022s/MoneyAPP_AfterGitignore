

namespace MoneyAPP.Models
{
    public class AccountStatusCount
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserDefault { get; set; }
        public DateTime UserDefault_DateTime { get; set; }

        //1. select Sequence > -1 的資料
        //2. select recordDay > UserDefault_DateTime.day,recordTime>UserDefault_DateTime.time的資料依照Id Groupby
        //
    }
}
