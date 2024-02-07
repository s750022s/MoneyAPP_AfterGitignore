
namespace ZMoney.Services
{
    public static class SharedMethod
    {
        public static void CheckAppCached(DbManager dbManager) 
        {
            if (App.CachedCategorys == null || App.CachedAccounts == null)
            {
                SetAppCached(dbManager);
            }
        }

        public static void SetAppCached(DbManager dbManager) 
        {
            App.CachedCategorys = dbManager.GetCategoryOrderBySequence();
            App.CachedAccounts = dbManager.GetAccountOrderBySequence();
        }
    }
}
