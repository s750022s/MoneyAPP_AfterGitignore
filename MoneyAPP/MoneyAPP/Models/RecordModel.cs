using SQLite;

namespace MoneyAPP.Models
{
    [Table("records")]
    public class RecordModel
    {
        [PrimaryKey, AutoIncrement]
        public int RecordID { get; set; }
	    public DateTime RecordDay { get; set; }
	    public TimeSpan RecordTime { get; set; }
	    public bool IsExpenses { get; set; }
        public int AccountID { get; set; }
        public int CategoryID { get; set; }

        [MaxLength(30)]
        public string? Item { get; set; }
        public int Amount { get; set; }
        public DateTime? LastUpdatedTime { get; set; }
        public bool IsDelete { get; set; }
    }
}