using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyAPP.Models
{
    [Table("isExpenses")]
    public class IsExpensesModel
    {
        [PrimaryKey]
        public bool IsExpenses { get; set; }

        [MaxLength(1)]
        public string PlusMinus { get; set; }
        public byte Style { get; set; }
    }
}