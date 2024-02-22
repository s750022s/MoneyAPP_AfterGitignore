using ZMoney.Models;
using ZMoney.Services;

namespace ZMoney.ViewModels
{
    public class HomeViewModel
    {

        private DbManager _dbManager;

        public HomeViewModel(DbManager dbManager) 
        {
            _dbManager = dbManager;
        }

        /// <summary>
        /// 取得日期參數的所有資料(不包含已刪除者)
        /// 1. 資料繫結至CollectionView
        /// 2. 計算當天總額
        /// </summary>
        /// <param name="recordDay">要查詢的日期</param>
        /// <returns>當天總額</returns>
        public List<HomePageData> GetOnedayData(DateTime recordDay,out int totalAmount) 
        {
            var data = _dbManager.GetHomePageData(recordDay);

            //計算當天總額
            totalAmount = 0;
            foreach (var item in data)
            {
                totalAmount += item.AmountOfMoney;
            }
            return data;
        }


    }
}
