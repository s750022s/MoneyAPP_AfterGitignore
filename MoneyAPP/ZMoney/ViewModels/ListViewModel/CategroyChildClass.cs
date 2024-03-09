using System.Collections;
using System.Collections.ObjectModel;
using ZMoney.Models;
using ZMoney.Services;


namespace ZMoney.ViewModels
{
    /// <summary>
    /// 實作種類子項目，繼承至ListBaseClass。
    /// 用途:ListSettingPage選單編輯器的邏輯切換。
    /// </summary>
    public class CategroyChildClass : ListBaseClass
    {
        private DbManager _dbManager;

        /// <summary>
        /// 第一次生成須建立全域變數CachedCategroy。
        /// </summary>
        /// <param name="dbManager"></param>
        public CategroyChildClass(DbManager dbManager)
        {
            SharedMethod.CheckAppCached(dbManager);
            _dbManager = dbManager;
        }

        /// <summary>
        /// 取得資料集合。
        /// </summary>
        public override IEnumerable SetPageData()
        {
            ObservableCollection<CategoryModel> data = new ObservableCollection<CategoryModel>(App.CachedCategorys);
            data.Add(new CategoryModel { Id = 999, Name = "+", Sequence = 999 });
            return data;
        }

        /// <summary>
        /// 取得指定Id的名稱及順序。
        /// </summary>
        /// <param name="id">要查詢的id</param>
        /// <param name="sequence">out順序</param>
        /// <returns></returns>
        public override string GetContent(int id, out string sequence)
        {
            var data = App.CachedCategorys.First(x => x.Id == id);
            sequence = data.Sequence.ToString();
            return data.Name;
        }

        /// <summary>
        /// 判定情況:id=999，新增項目；
        /// 情境1：Sequence大於目前項目總數 => 加在最後，Sequence=項目總數;
        /// 情境2：項目總數小於Sequence，代表要插在中間 => 將Sequence鎖定項目之後全數Sequence+1。
        /// </summary>
        /// <param name="name">名稱</param>
        /// <param name="sequenceStr">順序str</param>
        public override void AddSave(string name, string sequenceStr)
        {
            InputCheck(name, sequenceStr, out int sequence);
            var GetSequenceExits = App.CachedCategorys.Find(Category => Category.Sequence == sequence);
            if (GetSequenceExits != null)
            {
                foreach (var item in App.CachedCategorys)
                {
                    if (item.Sequence >= sequence && item.Sequence < 100)
                    {
                        item.Sequence += 1;
                    }
                }

                foreach (var item in App.CachedCategorys)
                {
                    _dbManager.UpdateCategory(item);
                }
            }
            else
            {
                sequence = Math.Min(sequence, App.CachedCategorys.Count);
            }
            _dbManager.AddCategory(new CategoryModel { Name = name, Sequence = sequence });
            SharedMethod.SetAppCached(_dbManager);
        }

        /// <summary>
        /// 判定情況:修改現值;
        /// 情境1：往前移 => endIndex 大於 startIndex => 修改位置之後所有項目位置-1
        /// 情境2：往後移 => endIndex 小於 startIndex => 修改位置之後所有項目位置+1
        /// </summary>
        /// <param name="name">名稱</param>
        /// <param name="sequenceStr">順序str</param>
        /// <param name="cacheID">修改項目Id</param>
        public override void UpdateSave(string name, string sequenceStr, int cacheID)
        {
            InputCheck(name, sequenceStr, out int sequence);
            var updateData = App.CachedCategorys.First(x => x.Id == cacheID);

            int endIndex = Math.Min(sequence, App.CachedCategorys.Count - 1);
            int startIndex = updateData.Sequence;

            //處理被干擾的項目
            if (endIndex != startIndex)
            {
                foreach (var item in App.CachedCategorys.Where(x => x.Id != cacheID))
                {
                    item.Sequence = UpdateSaveRule(startIndex, endIndex, item.Sequence);
                }
            }
            foreach (var item in App.CachedCategorys)
            {
                _dbManager.UpdateCategory(item);
            }

            //處理指定修改的項目
            updateData.Name = name;
            updateData.Sequence = endIndex;
            _dbManager.UpdateCategory(updateData);

            SharedMethod.SetAppCached(_dbManager);
        }

        /// <summary>
        /// 刪除項目。
        /// </summary>
        /// <param name="id">刪除Id</param>
        public override void Delete(int id)
        {
            var startSequence = App.CachedCategorys.First(x => x.Id == id).Sequence;
            App.CachedCategorys = App.CachedCategorys.OrderBy(item => item.Sequence).ToList();
            for (int i = startSequence; i < App.CachedCategorys.Count; i++)
            {
                App.CachedCategorys[i].Sequence = i == startSequence ? -1 : i - 1;
            }
            foreach (var item in App.CachedCategorys)
            {
                _dbManager.UpdateCategory(item);
            }
            SharedMethod.SetAppCached(_dbManager);
        }
    }
}
