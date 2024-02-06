using ZMoney.Models;

namespace ZMoney.Services
{
    public interface IDbService
    {
        /// <summary>
        /// 檢查連線
        /// </summary>
        void Init();


        //取得資料
        IEnumerable<CategoryModel> GetCategories();
        IEnumerable<AccountModel> GetAccounts();
        IEnumerable<RecordModel> GetRecords();

        //新增資料
        void AddCategory(CategoryModel category);
        void AddAccount(AccountModel account);
        void AddRecord(RecordModel record);

        //修改資料
        void UpdateCategory(CategoryModel category, bool IsDelete = false);
        void UpdateAccount(AccountModel account, bool IsDelete = false);
        void UpdateRecord(RecordModel Record, bool IsDelete = false);


    }
}
