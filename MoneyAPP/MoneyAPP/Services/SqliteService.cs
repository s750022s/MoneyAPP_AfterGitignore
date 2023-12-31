﻿using SQLite;
using MoneyAPP.Models;


namespace MoneyAPP.Services
{
    /// <summary>
    /// 實作一個連接資料庫的服務
    /// </summary>
    public class SqliteService:ISqliteService
    {
        /// <summary>
        /// 初始化Sqlite
        /// </summary>
        private SQLiteConnection _connection;

        /// <summary>
        /// DB檔的路徑(/storage/emulated/0/Android/data/com.companyname.moneyapp/files/record.db3)
        /// 來源：MauiProgram
        /// </summary>
        string _dbPath;

        /// <summary>
        /// 執行狀態訊息，寫於log(/TOT/待補)
        /// </summary>
        public string StatusMessage { get; set; }

        /// <summary>
        /// 初始化服務，如果路徑下無檔案，將創建預設值。
        /// </summary>
        /// <param name="dbPath">DB檔的存放位置：外部專屬目錄</param>
        public SqliteService(string dbPath)
        {
            _dbPath = dbPath;

            _connection = new SQLiteConnection(_dbPath);
            _connection.CreateTable<RecordModel>();
            _connection.CreateTable<CategoryModel>();
            _connection.CreateTable<AccountModel>();

            if (_connection.Table<CategoryModel>().Count() == 0)
            {
                _connection.Insert(new CategoryModel { Name = "早餐", Sequence = 0 });
                _connection.Insert(new CategoryModel { Name = "午餐", Sequence = 1 });
                _connection.Insert(new CategoryModel { Name = "晚餐", Sequence = 2 });
            }

            if (_connection.Table<AccountModel>().Count() == 0)
            {
                _connection.Insert(new AccountModel { Name = "現金", Sequence = 0 });
                _connection.Insert(new AccountModel { Name = "信用卡", Sequence = 1 });
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

        /// <summary>
        /// 增加Record表的一筆資料
        /// </summary>
        /// <param name="addRecord">Record資料Model</param>
        public void AddRecord(RecordModel addRecord)
        {
            int result = 0;
            Init();
            try
            {
                result += _connection.Insert(new RecordModel
                {
                    RecordDay = addRecord.RecordDay,
                    RecordTime = addRecord.RecordTime,
                    IsExpenses = addRecord.IsExpenses,
                    AccountID = addRecord.AccountID,
                    CategoryID = addRecord.CategoryID,
                    Item = addRecord.Item,
                    Amount = addRecord.Amount,
                    LastUpdatedTime = addRecord.LastUpdatedTime,
                    IsDelete = addRecord.IsDelete
                });
                StatusMessage = string.Format("{0} record(s) added (Name: {1})", result, addRecord.Item);
                return;

            }
            catch (Exception ex) { Console.WriteLine(ex); return; }
        }

        /// <summary>
        /// 修改Record表的一筆資料
        /// </summary>
        /// <param name="updateRecord">Record資料Model</param>
        public void UpdateRecord(RecordModel updateRecord)
        {
            int result = 0;
            Init();
            try
            {
                result += _connection.Update(new RecordModel
                {
                    RecordID = updateRecord.RecordID,
                    RecordDay = updateRecord.RecordDay,
                    RecordTime = updateRecord.RecordTime,
                    IsExpenses = updateRecord.IsExpenses,
                    AccountID = updateRecord.AccountID,
                    CategoryID = updateRecord.CategoryID,
                    Item = updateRecord.Item,
                    Amount = updateRecord.Amount,
                    LastUpdatedTime = updateRecord.LastUpdatedTime,
                    IsDelete = updateRecord.IsDelete
                });
                StatusMessage = string.Format("{0} 筆已修改 (Name: {1})", result, updateRecord.Item);
                return;

            }
            catch (Exception ex) { Console.WriteLine(ex); return; }
        }

        /// <summary>
        /// 取的以順序排序的類別清單，以製作類別選單
        /// </summary>
        /// <returns>類別List</returns>
        public List<CategoryModel> GetCategoryOrderBySequence()
        {
            Init();
            var orderBySequence = from c in _connection.Table<CategoryModel>()
                                  where c.Sequence > -1
                                  orderby c.Sequence
                                  select c;
            return orderBySequence.ToList();
        }

        /// <summary>
        /// 取的以順序排序的"帳戶"清單，以製作類別選單
        /// </summary>
        /// <returns>帳戶List</returns>
        public List<AccountModel> GetAccountOrderBySequence()
        {
            Init();
            var orderBySequence = from a in _connection.Table<AccountModel>()
                                  where a.Sequence > -1
                                  orderby a.Sequence
                                  select a;
            return orderBySequence.ToList();
        }

        /// <summary>
        /// 取得特定日期的不刪除紀錄;
        /// IsDelete == false
        /// </summary>
        /// <param name="recordday">要查詢的日期</param>
        /// <returns>首頁資料List</returns>
        public List<HomePageData> GetHomePageData(DateTime recordday)
        {
            Init();
            var homePageData = from r in _connection.Table<RecordModel>()
                               join c in _connection.Table<CategoryModel>() on r.CategoryID equals c.CategoryID
                               where r.RecordDay.Date == recordday.Date && r.IsDelete == false
                               orderby r.RecordTime descending
                               select new HomePageData
                               {
                                   RecordID = r.RecordID,
                                   Category = c.Name,
                                   Item = r.Item,
                                   Amount = r.Amount,
                               };

            return homePageData.ToList();
        }

        /// <summary>
        /// 按照Id取得單筆紀錄
        /// </summary>
        /// <param name="id">紀錄Id</param>
        /// <returns>單筆紀錄</returns>
        public RecordModel GetRecordById(int id)
        {
            var recordById = (from r in _connection.Table<RecordModel>()
                              where r.RecordID == id && r.IsDelete == false
                              select r).ToList();
            return recordById[0];
        }




        /// <summary>
        /// 修改類別
        /// </summary>
        /// <param name="category">要更新的完整資料</param>
        public void UpdateCategory(CategoryModel category)
        {
            int result = 0;
            Init();
            try
            {
                result += _connection.Update(new CategoryModel
                {
                    CategoryID = category.CategoryID,
                    Name = category.Name,
                    Sequence = category.Sequence
                });
                StatusMessage = string.Format("{0} 筆已修改 (Name: {1})", result, category.Name);
                return ;

            }
            catch (Exception ex) { Console.WriteLine(ex); return ; }
        }

        /// <summary>
        /// 新增類別
        /// </summary>
        /// <param name="category">要新增的完整資料</param>
        public void AddCategory(CategoryModel category)
        {
            int result = 0;
            Init();
            try
            {
                result += _connection.Insert(new CategoryModel
                {
                    Name = category.Name,
                    Sequence = category.Sequence
                });
                StatusMessage = string.Format("{0} record(s) added (Name: {1})", result, category.Name);
                return;

            }
            catch (Exception ex) { Console.WriteLine(ex); return ; }

        }

        /// <summary>
        /// 修改帳戶
        /// </summary>
        /// <param name="account">要更新的完整資料</param>
        public void UpdateAccount(AccountModel account)
        {
            int result = 0;
            Init();
            try
            {
                result += _connection.Update(new AccountModel
                {
                    AccountID = account.AccountID,
                    Name = account.Name,
                    Sequence = account.Sequence
                });
                StatusMessage = string.Format("{0} 筆已修改 (Name: {1})", result, account.Name);
                return;

            }
            catch (Exception ex) { Console.WriteLine(ex); return; }
        }

        /// <summary>
        /// 新增類別
        /// </summary>
        /// <param name="account">要新增的完整資料</param>
        public void AddAccount(AccountModel account)
        {
            int result = 0;
            Init();
            try
            {
                result += _connection.Insert(new AccountModel
                {
                    Name = account.Name,
                    Sequence = account.Sequence
                });
                StatusMessage = string.Format("{0} record(s) added (Name: {1})", result, account.Name);
                return;

            }
            catch (Exception ex) { Console.WriteLine(ex); return; }
        }





        /// <summary>
        /// 取得總資料筆數
        /// </summary>
        /// <returns>總筆數</returns>
        public int GetAllRecordCount() 
        {
            Init();
            int allRecordCount =  _connection.Table<RecordModel>()
                                  .Where (r => r.IsDelete == false)
                                  .Count();
            return allRecordCount;
        }

        /// <summary>
        /// 取得第一筆資料日期
        /// </summary>
        /// <returns>Firstday</returns>
        public DateTime GetRecordFirstday()
        {
            Init();
            var firstday = _connection.Table<RecordModel>()
                           .OrderBy(r => r.RecordDay)
                           .FirstOrDefault();
            if (firstday == null) 
            {
                return default(DateTime);
            }
            return firstday.RecordDay;
        }




        /// <summary>
        /// 除錯工具，取得Record表的所有資料
        /// </summary>
        /// <returns>所有Record的清單</returns>
        public List<RecordModel> GetRecords()
        {
            return _connection.Table<RecordModel>().ToList();
        }
    }
}

