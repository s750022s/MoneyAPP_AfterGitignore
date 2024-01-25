
namespace MoneyAPP.Models
{
    /// <summary>
    /// 首頁客製化的紀錄類別
    /// </summary>
    public class DataByCategory
    {
        /// <summary>
        /// 紀錄Id，用於回頭尋找完整資料
        /// </summary>
        public int RecordID { get; set; }

        /// <summary>
        /// 記錄日期
        /// </summary>
        public DateTime RecordDate { get; set; }

        private TimeSpan _recordTime;

        /// <summary>
        /// 記錄時間
        /// </summary>
        public TimeSpan RecordTime
        { get { return _recordTime; }
          set 
            {
                if (_recordTime != value)
                {
                    _recordTime = value;
                    RecordTimeToString();
                }
            }
        }

        /// <summary>
        /// 紀錄物件
        /// </summary>
        public string Item { get; set; }

        /// <summary>
        /// 紀錄金額(收入正數，支出負數)
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// 時間格式化
        /// </summary>
        public string RecordTime_str { get; set; }


        public void RecordTimeToString()
        {
            string amPm = RecordTime.TotalHours < 12 ? "AM" : "PM";
            string hh = amPm == "AM" ? RecordTime.Hours.ToString("00") : (RecordTime.Hours - 12).ToString("00");
            RecordTime_str = string.Format("{0}:{1} {2}", hh, RecordTime.Minutes.ToString("00"), amPm);
        }
    }
}
