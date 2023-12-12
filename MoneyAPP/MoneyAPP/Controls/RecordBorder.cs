using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyAPP.Controls
{
    /// <summary>
    /// 客製化Border，新增屬性Id
    /// </summary>
    public class RecordBorder:Border
    {
        public static readonly BindableProperty RecordIdProperty =
            BindableProperty.Create(nameof(RecordId), typeof(int), typeof(RecordBorder), null);

        /// <summary>
        /// 標記紀錄ID，方便回頭查詢
        /// </summary>
        public int RecordId
        {
            get { return (int)GetValue(RecordIdProperty); }
            set { SetValue(RecordIdProperty, value); }
        }
    }
}
