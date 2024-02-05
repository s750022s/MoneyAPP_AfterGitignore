using SQLite;
using ZMoney.Models;

namespace ZMoney.Services
{
    /// <summary>
    /// 實作一個連接資料庫的服務，該檔案存放初始化之方法。
    /// </summary>
    public partial class SqliteServices : IDbServices
    {
        /// <summary>
        /// 初始化Sqlite
        /// </summary>
        private SQLiteConnection _connection;

        private LocalFileLogger _logger;

        /// <summary>
        /// DB檔的路徑(/storage/emulated/0/Android/data/com.companyname.Zmoney/files/ZMoney.db)
        /// 來源：MauiProgram
        /// </summary>
        private string _dbPath;

        /// <summary>
        /// 初始化服務，如果路徑下無檔案，將創建預設值。
        /// </summary>
        /// <param name="dbPath">DB檔的存放位置：外部專屬目錄</param>
        public SqliteServices(string dbPath, LocalFileLogger localFileLogger)
        {
            _dbPath = dbPath;

            if (File.Exists(_dbPath) == false)
            {
                SQLiteDbInitializationData();
                return;
            }
            _connection = new SQLiteConnection(_dbPath);
            _logger = localFileLogger;
        }

        /// <summary>
        /// 創建預設值
        /// </summary>
        private void SQLiteDbInitializationData()
        {
            _connection = new SQLiteConnection(_dbPath);
            try
            {
                //創建Table_records
                _connection.CreateTable<RecordModel>();

                //創建Table_categories
                _connection.CreateTable<CategoryModel>();
                if (_connection.Table<CategoryModel>().Count() == 0)
                {
                    List<string> categoriesInitializationData = new List<string>()
                    {
                        "早餐","午餐","晚餐","飲料",
                        "生活用品","衣飾","交通","醫療",
                        "娛樂","其他"
                    };

                    for (int i = 0; i < categoriesInitializationData.Count; i++)
                    {
                        _connection.Insert(new CategoryModel { Name = categoriesInitializationData[i], Sequence = i });
                    }
                }


                //創建Table_account
                _connection.CreateTable<AccountModel>();
                if (_connection.Table<AccountModel>().Count() == 0)
                {
                    List<string> accountInitializationData = new List<string>() { "現金", "信用卡", "銀行帳戶" };
                    for (int i = 0; i < accountInitializationData.Count; i++)
                    {
                        _connection.Insert(new AccountModel { Name = accountInitializationData[i], Sequence = i, CurrentTotal = 0 });
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 檢查連線
        /// </summary>
        public void Init()
        {
            if (_connection != null)
                return;
        }
    }
}
