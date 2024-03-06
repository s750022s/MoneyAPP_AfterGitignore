namespace ZMoney.Models
{
    /// <summary>
    /// 統計頁第一層客製化資料，包含分組總額、百分比。
    /// </summary>
    public class TotalAndPercentFromGroup
    {
        /// <summary>
        /// 分組名稱
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// 分組id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 分組總金額
        /// </summary>
        public int GroupTatalAmount { get; set; }

        /// <summary>
        /// 分組百分比
        /// </summary>
        public double Percent { get; set; }
    }
}
