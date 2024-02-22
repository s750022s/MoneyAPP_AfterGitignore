
using ZMoney.Models;

namespace ZMoney.Services
{
    /// <summary>
    /// 與CategoryManager、AccountManager共同實作處理資料的邏輯，本檔案存放與紀錄Table相關之方法。
    /// </summary>
    public partial class DbManager
    {
        //前情提要 private IDbService _dbManager;

        /// <summary>
        /// 增加Record表的一筆資料
        /// </summary>
        /// <param name="record">Record資料Model</param>
        public void AddRecord(RecordModel record)
        {
            _dbService.AddRecord(record);
        }

        /// <summary>
        /// 修改Record表的一筆資料
        /// </summary>
        /// <param name="record">Record資料Model</param>
        public void UpdateRecord(RecordModel record)
        {
            _dbService.UpdateRecord(record);
        }

        /// <summary>
        /// 刪除Record表的一筆資料
        /// </summary>
        /// <param name="id">要刪除的資料id</param>
        public void DeleteRecord(int id)
        {
            var record = GetRecordById(id);
            record.IsDelete = true;
            _dbService.UpdateRecord(record,true);
        }


        /// <summary>
        /// 按照Id取得單筆紀錄
        /// </summary>
        /// <param name="id">紀錄Id</param>
        /// <returns>單筆紀錄</returns>
        public RecordModel GetRecordById(int id)
        {
            var recordById = (from record in _dbService.GetRecords()
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
            int allRecordCount = _dbService.GetRecords()
                                  .Where(record => record.IsDelete == false)
                                  .Count();
            return allRecordCount;
        }

        /// <summary>
        /// 取得特定日期的不刪除紀錄;
        /// IsDelete == false
        /// </summary>
        /// <param name="recordday">要查詢的日期</param>
        /// <returns>首頁資料List</returns>
        public List<HomePageData> GetHomePageData(DateTime recordday)
        {
            var homePageData = from record in _dbService.GetRecords()
                               join category in _dbService.GetCategories() on record.CategoryId equals category.Id
                               where record.RecordDateTime.Date == recordday.Date && record.IsDelete == false
                               orderby record.RecordDateTime.TimeOfDay descending
                               select new HomePageData(
                                   record.Id, 
                                   category.Name, 
                                   record.Description == null ? "" : record.Description, 
                                   record.AmountOfMoney
                                   );
            return homePageData.ToList();
        }

        /// <summary>
        /// 取得指定日期區間的類別分組的紀錄清單，並以日期時間為標準由新到舊排列。
        /// </summary>
        /// <param name="startDate">起始日</param>
        /// <param name="endDate">截止日</param>
        /// <param name="Id">類別Id</param>
        /// <returns></returns>
        public List<RecordsFromGroup> GetRecordsFromCategoryGroup(DateTime startDate, DateTime endDate, int Id, bool IsCategory)
        {
            var recordsFromGroups = from record in _dbService.GetRecords()
                                    where (record.RecordDateTime.Date >= startDate)
                                       && (record.RecordDateTime.Date <= endDate)
                                       && (record.IsDelete == false)
                                       && ((IsCategory && record.CategoryId == Id)
                                           || (!IsCategory && record.AccountId == Id))
                                    orderby record.RecordDateTime.Date descending, record.RecordDateTime.TimeOfDay descending
                                    select new RecordsFromGroup
                                    {
                                        Id = record.Id,
                                        RecordDateTime = record.RecordDateTime,
                                        Description = record.Description == null ? "" : record.Description,
                                        AmountOfMoney = record.AmountOfMoney,
                                    };
            return recordsFromGroups.ToList();
        }

        /// <summary>
        /// 取得指定日期區間的總收入/總支出/總金額。
        /// </summary>
        /// <param name="startDate">起始日</param>
        /// <param name="endDate">截止日</param>
        /// <returns>[總收入,總支出,總金額]</returns>
        public Array GetTotalByIsExpenses(DateTime startDate, DateTime endDate)
        {
            var recordGroup = from record in _dbService.GetRecords()
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

        
        //public DateTime GetRecordFirstday()
        //{
        //    var firstday = _dbManager.GetRecords()
        //                   .OrderBy(record => record.RecordDateTime)
        //                   .FirstOrDefault();
        //    if (firstday == null)
        //    {
        //        return default(DateTime);
        //    }
        //    return firstday.RecordDateTime;
        //}
    }
}