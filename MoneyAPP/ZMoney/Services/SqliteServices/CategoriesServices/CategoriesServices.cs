using ZMoney.Models;

namespace ZMoney.Services
{
    /// <summary>
    /// 實作一個連接資料庫的服務，該檔案存放與類別Table相關之方法。
    /// </summary>
    public partial class SqliteServices : IDbServices
    {
        /// <summary>
        /// 新增類別
        /// </summary>
        /// <param name="category">要新增的完整資料</param>
        public void AddCategory(CategoryModel category)
        {
            Init();
            try
            {
                _connection.Insert(category);
                LocalFileLogger.Log(string.Format("1 category added(Name: {0}).", category.Name));
            }
            catch (Exception ex)
            {
                LocalFileLogger.Log(string.Format("Error，錯誤訊息：{0}", ex));
            }
        }

        /// <summary>
        /// 修改類別
        /// </summary>
        /// <param name="category">要更新的完整資料</param>
        public void UpdateCategory(CategoryModel category)
        {
            Init();
            try
            {
                _connection.Update(category);
                LocalFileLogger.Log(string.Format("1 category updated(Name: {0}).", category.Name));
                return;
            }
            catch (Exception ex)
            {
                LocalFileLogger.Log(string.Format("Error，錯誤訊息：{0}", ex));
            }
        }

        /// <summary>
        /// 取得以順序排序的類別清單，以製作類別選單
        /// </summary>
        /// <returns>類別List</returns>
        public List<CategoryModel> GetCategoryOrderBySequence()
        {
            Init();
            var categoryOrderBySequence = from category in _connection.Table<CategoryModel>()
                                          where category.Sequence > -1
                                          orderby category.Sequence
                                          select category;
            return categoryOrderBySequence.ToList();
        }

        /// <summary>
        /// 取得指定日期區間的類別分組的總金額、百分比
        /// </summary>
        /// <param name="startDate">起始日</param>
        /// <param name="endDate">截止日</param>
        /// <returns>分組總額List</returns>
        public List<TotalAndPercentFromGroup> GetToatlFromCategoryGroup(DateTime startDate, DateTime endDate)
        {
            Init();

            //取得[有紀錄]的類別分組及總額
            var recordGroups = from recordWhere in (from record in _connection.Table<RecordModel>()
                                                    where (record.RecordDateTime.Date >= startDate)
                                                       && (record.RecordDateTime.Date <= endDate)
                                                       && (record.IsDelete == false)
                                                    select record)
                               join category in _connection.Table<CategoryModel>()
                               on recordWhere.CategoryId equals category.Id
                               select new
                               {
                                   name = category.Name,
                                   id = recordWhere.CategoryId,
                                   amount = recordWhere.AmountOfMoney
                               } into recordWithName
                               group recordWithName by recordWithName.name into categoryGroup
                               select new
                               {
                                   name = categoryGroup.Key,
                                   id = categoryGroup.First().id,
                                   categoryTotal = categoryGroup.Sum(x => x.amount)
                               };

            //取得紀錄總金額
            var totalAmount = recordGroups.Sum(x => x.categoryTotal);

            //計算[有紀錄]的百分比
            var categoryJoinRecord = from categoryTotal in recordGroups
                                     select new
                                     {
                                         name = categoryTotal.name,
                                         id = categoryTotal.id,
                                         groupTatalAmount = categoryTotal.categoryTotal,
                                         percent = categoryTotal.categoryTotal / (totalAmount * 1.00)
                                     } into result
                                     orderby result.percent descending
                                     select new TotalAndPercentFromGroup
                                     {
                                         Name = result.name,
                                         Id = result.id,
                                         GroupTatalAmount = result.groupTatalAmount,
                                         Percent = result.percent
                                     };

            //取得未被刪除的種類Id清單
            var categoryExcept_ID = (from category in _connection.Table<CategoryModel>()
                                     where category.Sequence > -1
                                     select category.Id).Except(categoryJoinRecord.Select(x => x.Id));

            //種類Id清單中，剩餘種類代入預設值
            var categoryExcept = from categoryExceptID in categoryExcept_ID
                                 join category in _connection.Table<CategoryModel>()
                                 on categoryExceptID equals category.Id
                                 select new TotalAndPercentFromGroup
                                 {
                                     Name = category.Name,
                                     Id = category.Id,
                                     GroupTatalAmount = 0,
                                     Percent = 0.0
                                 };
            //串接List
            var results = categoryJoinRecord.Concat(categoryExcept);

            return results.ToList();
        }

        /// <summary>
        /// 取得指定日期區間的類別分組的紀錄清單，並以日期時間為標準由新到舊排列。
        /// </summary>
        /// <param name="startDate">起始日</param>
        /// <param name="endDate">截止日</param>
        /// <param name="categoryId">類別Id</param>
        /// <returns></returns>
        public List<RecordsFromGroup> GetRecordsFromCategoryGroup(DateTime startDate, DateTime endDate, int categoryId)
        {
            Init();
            var recordsFromGroups = from record in _connection.Table<RecordModel>()
                                 where (record.RecordDateTime.Date >= startDate) 
                                    && (record.RecordDateTime.Date <= endDate)
                                    && (record.IsDelete == false) 
                                    && (record.CategoryId == categoryId)
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

    }
}
