using SQLite;
namespace MoneyAPP.Models
{
    [Table("categorys")]
    public class CategoryModel
    {
        [PrimaryKey, AutoIncrement]
        public int CategoryID { get; set; }

        [MaxLength(6)]
        public string Name { get; set; }
        public int Sequence { get; set; }
    }
}