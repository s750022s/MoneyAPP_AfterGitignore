using SQLite;
using ZMoney.Models;

namespace ZMoney.Services
{
    /// <summary>
    /// 實作資料庫串接。
    /// </summary>
    public class SqliteService : IDbService
    {
        private SQLiteConnection _connection = null!;
        private LocalFileLogger _logger;
        private string _dbPath;

        public SqliteService(string dbPath, LocalFileLogger localFileLogger)
        {
            _dbPath = dbPath;
            _logger = localFileLogger;
            Open();
        }

        /// <summary>
        /// 實作開啟資料庫。
        /// </summary>
        public void Open()
        {
            if (File.Exists(_dbPath) == false)
            {
                SQLiteDbInitializationData();
                return;
            }
            _connection = new SQLiteConnection(_dbPath);
        }

        /// <summary>
        /// 創建預設值。
        /// </summary>
        private void SQLiteDbInitializationData()
        {
            _connection = new SQLiteConnection(_dbPath);

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
                    "娛樂","薪資","其他"
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
                List<string> accountInitializationData = new List<string>() { "現金" };
                for (int i = 0; i < accountInitializationData.Count; i++)
                {
                    _connection.Insert(new AccountModel { Name = accountInitializationData[i], Sequence = i, CurrentTotal = 0 });
                }
            }

            //寫入log
            _logger.Log("初始化資料庫。");
        }

        /// <summary>
        /// 檢查連線
        /// </summary>
        public void Init()
        {
            if (_connection != null) return;
            Open();
            _logger.Log("資料庫連線失敗。");
        }

        /// <summary>
        /// 實作關閉資料庫。
        /// </summary>
        public void Close()
        {
            if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
            }
        }

        /// <summary>
        /// 實作取得種類table資料。
        /// </summary>
        public IEnumerable<CategoryModel> GetCategories()
        {
            Init();
            return _connection.Table<CategoryModel>();
        }

        /// <summary>
        /// 實作取得帳戶table資料。
        /// </summary>
        public IEnumerable<AccountModel> GetAccounts()
        {
            Init();
            return _connection.Table<AccountModel>();
        }

        /// <summary>
        /// 實作取得紀錄table資料。
        /// </summary>
        public IEnumerable<RecordModel> GetRecords()
        {
            Init();
            return _connection.Table<RecordModel>();
        }


        /// <summary>
        /// 實作新增種類table資料。
        /// </summary>
        public void AddCategory(CategoryModel category)
        {
            Init();
            _connection.Insert(category);
            _logger.Log(string.Format("The category of name:{0} added.", category.Name));
        }

        /// <summary>
        /// 實作新增帳戶table資料。
        /// </summary>
        public void AddAccount(AccountModel account)
        {
            Init();
            _connection.Insert(account);
            _logger.Log(string.Format("The account of name:{0} added.", account.Name));
        }

        /// <summary>
        /// 實作新增紀錄table資料。
        /// </summary>
        public void AddRecord(RecordModel record)
        {
            Init();
            _connection.Insert(record);
            _logger.Log(string.Format("The record of Amount:${0} added.", record.AmountOfMoney));
        }


        /// <summary>
        /// 實作修改種類table資料。
        /// </summary>
        public void UpdateCategory(CategoryModel category, bool IsDelete = false)
        {
            Init();
            _connection.Update(category);
            var action = IsDelete == false ? "updated" : "deleted";
            _logger.Log(string.Format("The category of name:{0} {1}.", category.Name, action));
        }

        /// <summary>
        /// 實作修改帳戶table資料。
        /// </summary>
        public void UpdateAccount(AccountModel account, bool IsDelete = false)
        {
            Init();
            _connection.Update(account);
            var action = IsDelete == false ? "updated" : "deleted";
            _logger.Log(string.Format("The account of name:{0} {1}.", account.Name, action));
        }

        /// <summary>
        /// 實作修改紀錄table資料。
        /// </summary>
        public void UpdateRecord(RecordModel record, bool IsDelete = false)
        {
            Init();
            _connection.Update(record);
            var action = IsDelete == false ? "updated" : "deleted";
            _logger.Log(string.Format("The record of Amount:${0} {1}.", record.AmountOfMoney, action));
        }
    }
}
