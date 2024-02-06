using ZMoney.Models;

namespace ZMoney.Services
{
    /// <summary>
    /// 與AccountManager、RecordManager共同實作處理資料的邏輯，本檔案存放與類別Table相關之方法。
    /// </summary>
    public partial class DbManager
    {
        private IDbService _dbService;

        public DbManager(IDbService dbService) 
        {
            _dbService = dbService;
        }

        /// <summary>
        /// 新增類別
        /// </summary>
        /// <param name="category">要新增的完整資料</param>
        public void AddCategory(CategoryModel category)
        {
            _dbService.AddCategory(category);
        }

        /// <summary>
        /// 修改類別
        /// </summary>
        /// <param name="category">要更新的完整資料</param>
        public void UpdateCategory(CategoryModel category) 
        {
            _dbService.UpdateCategory(category);
        }

        /// <summary>
        /// 刪除類別
        /// </summary>
        /// <param name="id">要刪除的資料id</param>
        public void DeleteCategory(int id)
        {
            var category = _dbService.GetCategories().FirstOrDefault(a => a.Id == id);
            if (category != null)
            {
                category.Sequence = -1;
                _dbService.UpdateCategory(category, true);
            }
        }

        /// <summary>
        /// 取得以順序排序的類別清單，以製作類別選單
        /// </summary>
        /// <returns>類別List</returns>
        public List<CategoryModel> GetCategoryOrderBySequence() 
        {
            var categories = from category in _dbService.GetCategories()
                             where category.Sequence > -1
                             orderby category.Sequence ascending
                             select category;
            return categories.ToList();
        }

        /// <summary>
        /// 取得指定日期區間的類別分組的總金額、百分比
        /// </summary>
        /// <param name="startDate">起始日</param>
        /// <param name="endDate">截止日</param>
        /// <returns>分組總額List</returns>
        public List<TotalAndPercentFromGroup> GetToatlFromCategoryGroup(DateTime startDate, DateTime endDate)
        {
            //取得[有紀錄]的類別分組及總額
            var recordGroups = from recordWhere in (from record in _dbService.GetRecords()
                                                    where (record.RecordDateTime.Date >= startDate)
                                                       && (record.RecordDateTime.Date <= endDate)
                                                       && (record.IsDelete == false)
                                                    select record)
                               join category in _dbService.GetCategories()
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
            var categoryExcept_ID = (from category in _dbService.GetCategories()
                                     where category.Sequence > -1
                                     select category.Id).Except(categoryJoinRecord.Select(x => x.Id));

            //種類Id清單中，剩餘種類代入預設值
            var categoryExcept = from categoryExceptID in categoryExcept_ID
                                 join category in _dbService.GetCategories()
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
    }
}
