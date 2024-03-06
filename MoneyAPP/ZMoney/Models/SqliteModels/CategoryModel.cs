using SQLite;

namespace ZMoney.Models
{
    /// <summary>
    /// 種類資料結構
    /// </summary>
    [Table("categories")]
    public class CategoryModel
    {
        /// <summary>
        /// 種類ID，PKey，會自動帶入不重複的值
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// 種類名稱，最長6個字
        /// </summary>
        [MaxLength(6)]
        public string Name { get; set; } = "";

        /// <summary>
        /// 種類順序，用以選單排序，值為0者為預設選項，-1者已被刪除
        /// </summary>
        public int Sequence { get; set; }
    }
}