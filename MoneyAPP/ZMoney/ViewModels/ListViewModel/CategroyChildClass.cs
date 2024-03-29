﻿
using System.Collections;
using System.Collections.ObjectModel;
using ZMoney.Models;
using ZMoney.Services;


namespace ZMoney.ViewModels
{
    public class CategroyChildClass:ListBaseClass
    {
        private DbManager _dbManager;
        public CategroyChildClass(DbManager dbManager) 
        {
            SharedMethod.CheckAppCached(dbManager);
            _dbManager = dbManager;
        }

        public override IEnumerable SetPageData() 
        {
            ObservableCollection<CategoryModel>  data= new ObservableCollection<CategoryModel>(App.CachedCategorys);
            data.Add(new CategoryModel { Id = 999, Name = "+", Sequence = 999 });
            return data;
        }

        public override string GetContent(int id ,out string sequence) 
        {
            var data =  App.CachedCategorys.First(x => x.Id == id);
            sequence = data.Sequence.ToString();
            return data.Name;
        }

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
