namespace ZMoney.Services
{
    /// <summary>
    /// Log紀錄模組，儲存於路徑/Log下，以日期為檔名。
    /// </summary>
    public class LocalFileLogger
    {
        /// <summary>
        /// 檔名生成
        /// </summary>
        private string FileName
        {
            get
            {
                return DateTime.Today.ToString("yyyy-MM-dd") + ".log";
            }
        }

        /// <summary>
        /// 路徑指派
        /// </summary>
        public string LogPath
        {
            get
            {
                return Directory.CreateDirectory(FileAccessHelper.GetLocalFilePath("Log")).FullName;
            }
        }

        /// <summary>
        /// 紀錄log，若為當日第一筆將自動生成新檔案。
        /// </summary>
        /// <param name="message">log訊息</param>
        public void Log(string message)
        {
            string content = string.Join("", "[", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), "]  ", message, "\n");
            File.AppendAllText(Path.Combine(LogPath, FileName), content);
        }
    }
}
