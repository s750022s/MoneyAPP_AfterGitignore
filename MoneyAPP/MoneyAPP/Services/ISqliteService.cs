using MoneyAPP.Models;
namespace MoneyAPP.Services
{
    /// <summary>
    /// 定義一個連接資料庫的服務
    /// </summary>
    public interface ISqliteService
    {

        //與紀錄相關的SQL行為(歷史紀錄、增加紀錄、修改紀錄)


        /// <summary>
        /// 增加Record表的一筆資料
        /// </summary>
        /// <param name="addRecord">Record資料Model</param>
        public void AddRecord(RecordModel addRecord);

        /// <summary>
        /// 修改Record表的一筆資料
        /// </summary>
        /// <param name="updateRecord">Record資料Model</param>
        public void UpdateRecord(RecordModel updateRecord);

        /// <summary>
        /// 取的以順序排序的"類別"清單，以製作類別選單
        /// </summary>
        /// <returns>類別List</returns>
        public List<CategoryModel> GetCategoryOrderBySequence();

        /// <summary>
        /// 取的以順序排序的"帳戶"清單，以製作類別選單
        /// </summary>
        /// <returns>帳戶List</returns>
        public List<AccountModel> GetAccountOrderBySequence();

        /// <summary>
        /// 取得特定日期且沒刪除的客製紀錄，用於首頁;
        /// IsDelete == false
        /// </summary>
        /// <param name="recordday">要查詢的日期</param>
        /// <returns>首頁資料List</returns>
        public List<HomePageData> GetHomePageData(DateTime recordday);

        /// <summary>
        /// 按照Id取得單筆紀錄
        /// </summary>
        /// <param name="id">紀錄Id</param>
        /// <returns>單筆紀錄</returns>
        public RecordModel GetRecordById(int id);



        //選單更新(類別、帳戶)



        /// <summary>
        /// 修改類別
        /// </summary>
        /// <param name="category">要更新的完整資料</param>
        public void UpdateCategory(CategoryModel category);

        /// <summary>
        /// 新增類別
        /// </summary>
        /// <param name="category">要新增的完整資料</param>
        public void AddCategory(CategoryModel category);

        /// <summary>
        /// 修改帳戶
        /// </summary>
        /// <param name="account">要更新的完整資料</param>
        public void UpdateAccount(AccountModel account);

        /// <summary>
        /// 新增類別
        /// </summary>
        /// <param name="account">要新增的完整資料</param>
        public void AddAccount(AccountModel account);


        //系統資訊

        /// <summary>
        /// 取得總資料筆數
        /// </summary>
        /// <returns>總筆數</returns>
        public int GetAllRecordCount();

        /// <summary>
        /// 取得第一筆資料日期
        /// </summary>
        /// <returns>Firstday</returns>
        public DateTime GetRecordFirstday();


        //除錯工具


        /// <summary>
        /// 除錯工具，取得Record表的所有資料
        /// </summary>
        /// <returns>所有Record的清單</returns>
        public List<RecordModel> GetRecords();

        
    }
}
