using SQLite;
using MoneyAPP.Models;


namespace MoneyAPP.Services
{
    public class SqliteService
    {
        private SQLiteConnection _connection;
        string _dbPath;
        public string StatusMessage { get; set; }

        public SqliteService(string dbPath)
        {
            _dbPath = dbPath;
            _connection = new SQLiteConnection(_dbPath);
            _connection.CreateTable<RecordModel>();
            _connection.CreateTable<CategoryModel>();
            _connection.CreateTable<AccountModel>();
            //_connection.CreateTable<IsExpensesModel>();

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

            //if (_connection.Table<IsExpensesModel>().Count() == 0)
            //{
            //_connection.Insert(new IsExpensesModel { IsExpenses = false, PlusMinus="+", Style = 1 });
            //_connection.Insert(new IsExpensesModel { IsExpenses = true, PlusMinus = "-", Style = 1 });
            //}

        }

        public void Init()
        {

            if (_connection != null)
                return;
        }

        public int AddTodo(RecordModel addRecord)
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
                return result;

            }
            catch (Exception ex) { Console.WriteLine(ex); return result; }
        }

        public string UpdateTodo(RecordModel updateRecord)
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
                return StatusMessage;

            }
            catch (Exception ex) { Console.WriteLine(ex); return ""; }
        }


        public List<CategoryModel> GetCategoryOrderBySequence()
        {
            Init();
            var orderBySequence = from c in _connection.Table<CategoryModel>()
                                  where c.Sequence > -1
                                  orderby c.Sequence
                                  select c;
            return orderBySequence.ToList();
        }

        public List<AccountModel> GetAccountOrderBySequence()
        {
            Init();
            var orderBySequence = from a in _connection.Table<AccountModel>()
                                  where a.Sequence > -1
                                  orderby a.Sequence
                                  select a;
            return orderBySequence.ToList();
        }

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

        public List<RecordModel> GetRecords()
        {
            return _connection.Table<RecordModel>().ToList();
        }

        public RecordModel GetRecordById(int id)
        {
            var recordById = (from r in _connection.Table<RecordModel>()
                              where r.RecordID == id && r.IsDelete == false
                              select r).ToList();
            return recordById[0];
        }

        public string UpdateCategory(CategoryModel category)
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
                return StatusMessage;

            }
            catch (Exception ex) { Console.WriteLine(ex); return StatusMessage; }
        }

        public string AddCategory(CategoryModel category)
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
                return StatusMessage;

            }
            catch (Exception ex) { Console.WriteLine(ex); return StatusMessage; }

        }

        public string UpdateAccount(AccountModel account)
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
                return StatusMessage;

            }
            catch (Exception ex) { Console.WriteLine(ex); return StatusMessage; }
        }

        public string AddAccount(AccountModel account)
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
                return StatusMessage;

            }
            catch (Exception ex) { Console.WriteLine(ex); return StatusMessage; }
        }

        public int GetAllRecordCount() 
        {
            Init();
            int allRecordCount =  _connection.Table<RecordModel>()
                                  .Where (r => r.IsDelete == false)
                                  .Count();
            return allRecordCount;
        }

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
    }
}

