namespace ZMoney.Services
{
    /// <summary>
    /// Log紀錄模組，儲存於路徑/Log下，以日期為檔名。靜態類別可直接調用。
    /// </summary>
    public static class LocalFileLogger
    {
        /// <summary>
        /// 檔名生成
        /// </summary>
        private static string FileName 
        {
            get
            {
                return DateTime.Today.ToString("yyyy-MM-dd") + ".log";
            }
        }

        /// <summary>
        /// 路徑指派
        /// </summary>
        private static string LogPath 
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
        public static void Log(string message) 
        {
            string content = string.Join("","[", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), "]  ", message,"\n");
            File.AppendAllText(Path.Combine(LogPath, FileName), content);
        }
    }
}
