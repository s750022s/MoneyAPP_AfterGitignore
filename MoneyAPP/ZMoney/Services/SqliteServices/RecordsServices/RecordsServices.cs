using ZMoney.Models;

namespace ZMoney.Services
{
    /// <summary>
    /// 實作一個連接資料庫的服務，該檔案存放與紀錄Table相關之方法。
    /// </summary>
    public partial class SqliteServices : IDbServices
    {
        /// <summary>
        /// 增加Record表的一筆資料
        /// </summary>
        /// <param name="record">Record資料Model</param>
        public void AddRecord(RecordModel record)
        {
            Init();
            try
            {
                _connection.Insert(record);
                LocalFileLogger.Log(string.Format("1 record added(AmountOfMoney: {0}).", record.AmountOfMoney));
            }
            catch (Exception ex)
            {
                LocalFileLogger.Log(string.Format("Error，錯誤訊息：{0}", ex));
            }
        }

        /// <summary>
        /// 修改Record表的一筆資料
        /// </summary>
        /// <param name="record">Record資料Model</param>
        public void UpdateRecord(RecordModel record)
        {
            Init();
            try
            {
                _connection.Update(record);
                LocalFileLogger.Log(string.Format("1 record updated(AmountOfMoney: {0}).", record.AmountOfMoney));
            }
            catch (Exception ex)
            {
                LocalFileLogger.Log(string.Format("Error，錯誤訊息：{0}", ex));
            }
        }


        //各種取得資料


        /// <summary>
        /// 按照Id取得單筆紀錄
        /// </summary>
        /// <param name="id">紀錄Id</param>
        /// <returns>單筆紀錄</returns>
        public RecordModel GetRecordById(int id)
        {
            var recordById = (from record in _connection.Table<RecordModel>()
                              where record.Id == id && record.IsDelete == false
                              select record).ToList();
            return recordById[0];
        }

        /// <summary>
        /// 取得總資料筆數
        /// </summary>
        /// <returns>總筆數</returns>
        public int GetAllRecordCount()
        {
            Init();
            int allRecordCount = _connection.Table<RecordModel>()
                                  .Where(record => record.IsDelete == false)
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
                           .OrderBy(record => record.RecordDateTime)
                           .FirstOrDefault();
            if (firstday == null)
            {
                return default(DateTime);
            }
            return firstday.RecordDateTime;
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
            var homePageData = from record in _connection.Table<RecordModel>()
                               join category in _connection.Table<CategoryModel>() on record.CategoryId equals category.Id
                               where record.RecordDateTime.Date == recordday.Date && record.IsDelete == false
                               orderby record.RecordDateTime.TimeOfDay descending
                               select new HomePageData
                               {
                                   Id = record.Id,
                                   CategoryName = category.Name,
                                   Description = record.Description == null?"": record.Description,
                                   AmountOfMoney = record.AmountOfMoney,
                               };
            return homePageData.ToList();
        }

        /// <summary>
        /// 取得指定日期區間的總收入/總支出/總金額。
        /// </summary>
        /// <param name="startDate">起始日</param>
        /// <param name="endDate">截止日</param>
        /// <returns>[總收入,總支出,總金額]</returns>
        public Array GetTotalByIsExpenses(DateTime startDate, DateTime endDate) 
        {
            var recordGroup = from record in _connection.Table<RecordModel>()
                              where (record.RecordDateTime.Date >= startDate) 
                                 && (record.RecordDateTime.Date <= endDate) 
                                 && (record.IsDelete == false)
                              group record by record.IsExpenses into recordGroupByIsExpenses
                              select new
                              {
                                  recordGroupByIsExpenses.Key,
                                  total = recordGroupByIsExpenses.Sum(x => x.AmountOfMoney)
                              };
            int[] totalArray = new int[3];
            foreach (var record in recordGroup) 
            {
                var result = record.Key == true ? totalArray[1] = record.total : totalArray[0] = record.total;
            }
            totalArray[2] = totalArray[0] + totalArray[1];
            return totalArray;
        }
    }
}
