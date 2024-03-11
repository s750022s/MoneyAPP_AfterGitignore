using System.Collections;
using System.Collections.ObjectModel;
using ZMoney.Models;
using ZMoney.Services;

namespace ZMoney.ViewModels
{
    public class AccountChlidClass: ListBaseClass
    {
        private DbManager _dbManager;
        public AccountChlidClass(DbManager dbManager)
        {
            SharedMethod.CheckAppCached(dbManager);
            _dbManager = dbManager;
        }

        public override IEnumerable SetPageData()
        {
            ObservableCollection<AccountModel> data = new ObservableCollection<AccountModel>(App.CachedAccounts);
            data.Add(new AccountModel { Id = 999, Name = "+", Sequence = 999 });
            return data;
        }

        public override string GetContent(int id, out string sequence)
        {
            var data = App.CachedAccounts.First(x => x.Id == id);
            sequence = data.Sequence.ToString();
            return data.Name;
        }

        public override void AddSave(string name, string sequenceStr) 
        {
            InputCheck(name, sequenceStr, out int sequence);
            var GetSequenceExits = App.CachedAccounts.Find(Account => Account.Sequence == sequence);
            if (GetSequenceExits != null)
            {
                foreach (var item in App.CachedAccounts)
                {
                    if (item.Sequence >= sequence && item.Sequence < 100)
                    {
                        item.Sequence += 1;
                    }
                }

                foreach (var item in App.CachedAccounts)
                {
                    _dbManager.UpdateAccount(item);
                }
            }
            else
            {
                sequence = Math.Min(sequence, App.CachedAccounts.Count);
            }
            _dbManager.AddAccount(new AccountModel { Name = name, Sequence = sequence });
            SharedMethod.SetAppCached(_dbManager);
        }

        public override void UpdateSave(string name, string sequenceStr, int cacheID)
        {
            InputCheck(name, sequenceStr, out int sequence);
            var updateData = App.CachedAccounts.First(x => x.Id == cacheID);

            int endIndex = Math.Min(sequence, App.CachedAccounts.Count - 1);
            int startIndex = updateData.Sequence;

            //處理被干擾的項目
            if (endIndex != startIndex)
            {
                foreach (var item in App.CachedAccounts.Where(x => x.Id != cacheID))
                {
                    item.Sequence = UpdateSaveRule(startIndex, endIndex, item.Sequence);
                }
            }
            foreach (var item in App.CachedAccounts)
            {
                _dbManager.UpdateAccount(item);
            }

            //處理指定修改的項目
            updateData.Name = name;
            updateData.Sequence = endIndex;
            _dbManager.UpdateAccount(updateData);

            SharedMethod.SetAppCached(_dbManager);
        }


        public override void Delete(int id)
        {
            var startSequence = App.CachedAccounts.First(x => x.Id == id).Sequence;
            App.CachedAccounts = App.CachedAccounts.OrderBy(item => item.Sequence).ToList();
            for (int i = startSequence; i < App.CachedAccounts.Count; i++)
            {
                App.CachedAccounts[i].Sequence = i == startSequence ? -1 : i - 1;
            }
            foreach (var item in App.CachedAccounts)
            {
                _dbManager.UpdateAccount(item);
            }
            SharedMethod.SetAppCached(_dbManager);
        }


    }
}
