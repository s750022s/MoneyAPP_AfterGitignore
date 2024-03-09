using ZMoney.Models;

namespace ZMoney.Services
{
    /// <summary>
    /// 定義資料庫串接。
    /// </summary>
    public interface IDbService
    {
        /// <summary>
        /// 定義開啟資料庫。
        /// </summary>
        void Open();

        /// <summary>
        /// 定義檢查連線。
        /// </summary>
        void Init();

        /// <summary>
        /// 定義關閉資料庫。
        /// </summary>
        void Close();


        /// <summary>
        /// 定義取得種類table資料。
        /// </summary>
        IEnumerable<CategoryModel> GetCategories();

        /// <summary>
        /// 定義取得帳戶table資料。
        /// </summary>
        IEnumerable<AccountModel> GetAccounts();

        /// <summary>
        /// 定義取得紀錄table資料。
        /// </summary>
        IEnumerable<RecordModel> GetRecords();



        /// <summary>
        /// 定義新增種類table資料。
        /// </summary>
        void AddCategory(CategoryModel category);

        /// <summary>
        /// 定義新增帳戶table資料。
        /// </summary>
        void AddAccount(AccountModel account);

        /// <summary>
        /// 定義新增紀錄table資料。
        /// </summary>
        void AddRecord(RecordModel record);

        /// <summary>
        /// 定義修改種類table資料。
        /// </summary>
        void UpdateCategory(CategoryModel category, bool IsDelete = false);

        /// <summary>
        /// 定義修改帳戶table資料。
        /// </summary>
        void UpdateAccount(AccountModel account, bool IsDelete = false);

        /// <summary>
        /// 定義修改紀錄table資料。
        /// </summary>
        void UpdateRecord(RecordModel Record, bool IsDelete = false);
    }
}
