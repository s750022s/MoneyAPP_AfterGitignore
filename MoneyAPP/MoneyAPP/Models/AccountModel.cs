using SQLite;
namespace MoneyAPP.Models
{
    [Table("accounts")]
    public class AccountModel
    {
        [PrimaryKey, AutoIncrement]
        public int AccountID { get; set; }

        [MaxLength(6)]
        public string Name { get; set; }
        public int Sequence { get; set; }
    }
}