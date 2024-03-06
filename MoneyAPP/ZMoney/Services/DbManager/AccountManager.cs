using ZMoney.Models;

namespace ZMoney.Services
{
    /// <summary>
    /// 與CategoryManager、RecordManager共同實作處理資料的邏輯，本檔案存放與帳戶Table相關之方法。
    /// </summary>
    public partial class DbManager
    {
        //前情提要 private IDbService _dbManager;

        /// <summary>
        /// 新增帳戶
        /// </summary>
        /// <param name="account">要新增的完整資料</param>
        public void AddAccount(AccountModel account)
        {
            _dbService.AddAccount(account);
        }

        /// <summary>
        /// 修改帳戶名稱及順序
        /// </summary>
        /// <param name="account">要更新的完整資料</param>
        public void UpdateAccount(AccountModel account)
        {
            account.CurrentTotal = (from raw in _dbService.GetAccounts()
                               where raw.Id == account.Id
                               select raw.CurrentTotal).First();
            _dbService.UpdateAccount(account);
        }

        /// <summary>
        /// 修改帳戶當前總額
        /// </summary>
        /// <param name="id">帳戶Id</param>
        /// <param name="difference">帳戶修改差額</param>
        public void UpdateCurrentTotal(int id, int difference)
        {
            var data = _dbService.GetAccounts().FirstOrDefault(a => a.Id == id);
            if (data != null)
            {
                data.CurrentTotal += difference;
                _dbService.UpdateAccount(data);
            }
        }

        /// <summary>
        /// 刪除帳戶
        /// </summary>
        /// <param name="id">要刪除的資料id</param>
        public void DeleteAccount(int id)
        {
            var account = _dbService.GetAccounts().FirstOrDefault(a => a.Id == id);
            if (account != null)
            {
                account.Sequence = -1;
                _dbService.UpdateAccount(account, true);
            }
        }

        /// <summary>
        /// 取的以順序排序的"帳戶"清單，以製作類別選單
        /// </summary>
        /// <returns>帳戶List</returns>
        public List<AccountModel> GetAccountOrderBySequence()
        {
            var accountOrderBySequence = from account in _dbService.GetAccounts()
                                         where account.Sequence > -1
                                         orderby account.Sequence
                                         select account;
            return accountOrderBySequence.ToList();
        }

        /// <summary>
        /// 取得指定日期區間的帳戶分組的總金額、百分比
        /// </summary>
        /// <param name="startDate">起始日</param>
        /// <param name="endDate">截止日</param>
        /// <returns>分組總額List</returns>
        public List<TotalAndPercentFromGroup> GetToatlFromAccountGroup(DateTime startDate, DateTime endDate, bool isExpenses)
        {
            //取得[有紀錄]的帳戶分組及總額
            var recordGroups = from recordWhere in (from record in _dbService.GetRecords()
                                                    where (record.RecordDateTime.Date >= startDate)
                                                       && (record.RecordDateTime.Date <= endDate)
                                                       && (record.IsExpenses == isExpenses)
                                                       && (record.IsDelete == false)
                                                    select record)
                               join account in _dbService.GetAccounts()
                               on recordWhere.AccountId equals account.Id
                               select new
                               {
                                   name = account.Name,
                                   id = recordWhere.AccountId,
                                   amount = recordWhere.AmountOfMoney
                               } into recordWithName
                               group recordWithName by recordWithName.name into accountGroup
                               select new
                               {
                                   name = accountGroup.Key,
                                   id = accountGroup.First().id,
                                   accountTotal = accountGroup.Sum(x => x.amount)
                               };

            //取得紀錄總金額
            var totalAmount = recordGroups.Sum(x => x.accountTotal);

            //計算[有紀錄]的百分比
            var accountJoinRecord = from accountTotal in recordGroups
                                    select new
                                    {
                                        name = accountTotal.name,
                                        id = accountTotal.id,
                                        groupTatalAmount = accountTotal.accountTotal,
                                        percent = accountTotal.accountTotal / (totalAmount * 1.00)
                                    } into result
                                    orderby result.percent descending
                                    select new TotalAndPercentFromGroup
                                    {
                                        Name = result.name,
                                        Id = result.id,
                                        GroupTatalAmount = result.groupTatalAmount,
                                        Percent = result.percent
                                    };

            //取得未被刪除的帳戶Id清單
            var accountExcept_ID = (from account in _dbService.GetAccounts()
                                    where account.Sequence > -1
                                    select account.Id).Except(accountJoinRecord.Select(x => x.Id));

            //種類Id清單中，剩餘帳戶代入預設值
            var accountExcept = from accountExceptID in accountExcept_ID
                                join account in _dbService.GetAccounts()
                                on accountExceptID equals account.Id
                                select new TotalAndPercentFromGroup
                                {
                                    Name = account.Name,
                                    Id = account.Id,
                                    GroupTatalAmount = 0,
                                    Percent = 0.0
                                };
            //串接List
            var results = accountJoinRecord.Concat(accountExcept);

            return results.ToList();
        }


    }
}