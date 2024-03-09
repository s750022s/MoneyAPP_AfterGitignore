namespace ZMoney.Services
{
    /// <summary>
    /// 存放全域共用的靜態方法。
    /// </summary>
    public static class SharedMethod
    {
        /// <summary>
        /// 檢查CachedCategorys是否已生成。
        /// </summary>
        /// <param name="dbManager"></param>
        public static void CheckAppCached(DbManager dbManager)
        {
            if (App.CachedCategorys == null || App.CachedAccounts == null)
            {
                SetAppCached(dbManager);
            }
        }

        /// <summary>
        /// 生成全域Cached變數
        /// </summary>
        /// <param name="dbManager"></param>
        public static void SetAppCached(DbManager dbManager)
        {
            App.CachedCategorys = dbManager.GetCategoryOrderBySequence();
            App.CachedAccounts = dbManager.GetAccountOrderBySequence();
        }
    }
}
