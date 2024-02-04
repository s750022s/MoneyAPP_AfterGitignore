using System;
using System.Collections.Generic;
using System.Linq;
using ZMoney.Models;

namespace ZMoney.Services
{
    /// <summary>
    /// 定義了資料庫操作的各種方法
    /// </summary>
    public interface IDbServices
    {
        //SqliteServicesShared 初始化

        /// <summary>
        /// 檢查連線
        /// </summary>
        void Init();

        //CategoriesServices 類別

        /// <summary>
        /// 取的以順序排序的類別清單，以製作類別選單
        /// </summary>
        /// <returns>類別List</returns>
        List<CategoryModel> GetCategoryOrderBySequence();

        /// <summary>
        /// 新增類別
        /// </summary>
        /// <param name="category">要新增的完整資料</param>
        void AddCategory(CategoryModel category);

        /// <summary>
        /// 修改類別
        /// </summary>
        /// <param name="category">要更新的完整資料</param>
        void UpdateCategory(CategoryModel category);

        /// <summary>
        /// 取得指定日期區間的類別分組的總金額、百分比
        /// </summary>
        /// <param name="startDate">起始日</param>
        /// <param name="endDate">截止日</param>
        /// <returns>分組總額List</returns>
        List<TotalAndPercentFromGroup> GetToatlFromCategoryGroup(DateTime startDate, DateTime endDate);

        /// <summary>
        /// 取得指定日期區間的類別分組的紀錄清單，並以日期時間為標準由新到舊排列。
        /// </summary>
        /// <param name="startDate">起始日</param>
        /// <param name="endDate">截止日</param>
        /// <param name="categoryId">類別Id</param>
        /// <returns></returns>
        List<RecordsFromGroup> GetRecordsFromCategoryGroup(DateTime startDate, DateTime endDate, int categoryId);



        //AccountsServices 帳戶

        /// <summary>
        /// 取的以順序排序的"帳戶"清單，以製作類別選單
        /// </summary>
        /// <returns>帳戶List</returns>
        List<AccountModel> GetAccountOrderBySequence();

        /// <summary>
        /// 新增帳戶
        /// </summary>
        /// <param name="account">要新增的完整資料</param>
        void AddAccount(AccountModel account);

        /// <summary>
        /// 修改帳戶名稱及順序
        /// </summary>
        /// <param name="account">要更新的完整資料</param>
        void UpdateAccount(AccountModel account);

        /// <summary>
        /// 修改帳戶當前總額
        /// </summary>
        /// <param name="id">帳戶Id</param>
        /// <param name="difference">帳戶修改差額</param>
        void UpdateCurrentTotal(int id, int difference);

        /// <summary>
        /// 取得指定日期區間的帳戶分組的總金額、百分比
        /// </summary>
        /// <param name="startDate">起始日</param>
        /// <param name="endDate">截止日</param>
        /// <returns>分組總額List</returns>
        List<TotalAndPercentFromGroup> GetToatlFromAccountGroup(DateTime startDate, DateTime endDate);

        /// <summary>
        /// 取得指定日期區間的帳戶分組的紀錄清單，並以日期時間為標準由新到舊排列。
        /// </summary>
        /// <param name="startDate">起始日</param>
        /// <param name="endDate">截止日</param>
        /// <param name="accountId">帳戶Id</param>
        /// <returns></returns>
        List<RecordsFromGroup> GetRecordsFromAccountGroup(DateTime startDate, DateTime endDate, int accountId);



        //RecordsServices 紀錄

        /// <summary>
        /// 增加Record表的一筆資料
        /// </summary>
        /// <param name="record">Record資料Model</param>
        void AddRecord(RecordModel record);

        /// <summary>
        /// 修改Record表的一筆資料
        /// </summary>
        /// <param name="record">Record資料Model</param>
        public void UpdateRecord(RecordModel record);

        /// <summary>
        /// 按照Id取得單筆紀錄
        /// </summary>
        /// <param name="id">紀錄Id</param>
        /// <returns>單筆紀錄</returns>
        RecordModel GetRecordById(int id);

        /// <summary>
        /// 取得總資料筆數
        /// </summary>
        /// <returns>總筆數</returns>
        int GetAllRecordCount();

        /// <summary>
        /// 取得第一筆資料日期
        /// </summary>
        /// <returns>Firstday</returns>
        DateTime GetRecordFirstday();

        /// <summary>
        /// 取得特定日期的不刪除紀錄;
        /// IsDelete == false
        /// </summary>
        /// <param name="recordday">要查詢的日期</param>
        /// <returns>首頁資料List</returns>
        List<HomePageData> GetHomePageData(DateTime recordday);

        /// <summary>
        /// 取得指定日期區間的總收入/總支出/總金額。
        /// </summary>
        /// <param name="startDate">起始日</param>
        /// <param name="endDate">截止日</param>
        /// <returns>[總收入,總支出,總金額]</returns>
        Array GetTotalByIsExpenses(DateTime startDate, DateTime endDate);
    }
}
