namespace ZMoney.Services
{
    /// <summary>
    /// 開啟與關閉資料庫。
    /// </summary>
    public partial class DbManager
    {
        //前情提要 private IDbService _dbService;

        /// <summary>
        /// 關閉資料庫。
        /// </summary>
        public void Close()
        {
            _dbService.Close();
        }

        /// <summary>
        /// 開啟資料庫。
        /// </summary>
        public void Open()
        {
            _dbService.Open();
        }
    }
}
