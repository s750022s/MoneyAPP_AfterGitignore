using MoneyAPP.Controls;
using MoneyAPP.Models;

namespace MoneyAPP.Pages;

//如有修改bug，請一併修改SetAccountListPage！

/// <summary>
/// 帳戶選單設定頁
/// </summary>
public partial class SetAccountListPage : ContentPage
{
    /// <summary>
    /// 暫存帳戶ID
    /// </summary>
    int cacheID = 0;


    public SetAccountListPage()
	{
		InitializeComponent();
        AccountModelToPicker();
    }

    /// <summary>
    /// 帳戶項目生成
    /// </summary>
    private void AccountModelToPicker()
    {
        if (App.CachedAccounts == null)
        {
            App.CachedAccounts = App.ServiceRepo.GetAccountOrderBySequence();
            App.CachedAccounts = App.ServiceRepo.GetAccountOrderBySequence();
        }

        var accounts = App.CachedAccounts.ToList();
        accounts.Add(new AccountModel { AccountID = 99, Name = "+", Sequence = 99 });

        DatasCollectionView.ItemsSource = accounts;
    }

    /// <summary>
    /// 點擊Border，會跳出該Border的資料
    /// </summary>
    private void OnBorderTapped(object sender, TappedEventArgs e) 
    {
        RecordBorder recordBorder = (RecordBorder)sender;
        cacheID = recordBorder.RecordId;
        Revise_Grid.IsVisible = true;

        if (cacheID == 99)
        {
            Name_Entry.Text = "";
            Sequence_Entry.Text = "";
            return;
        }

        var account = App.CachedAccounts.Find(account => account.AccountID == cacheID);
        Name_Entry.Text = account.Name;
        Sequence_Entry.Text = account.Sequence.ToString();
    }

    /// <summary>
    /// 當id=99，啟動AddSave;
    /// 情境1：Sequence大於目前項目總數 => 加在最後，Sequence=項目總數;
    /// 情境2：項目總數小於Sequence，代表要插在中間 => 將Sequence鎖定項目之後全數Sequence+1
    /// </summary>
    private void AddSave_Click()
    {
        var input = InputCheck();
        var accounts = App.CachedAccounts.ToList();
        if (input != null)
        {
            var c = accounts.Find(Account => Account.Sequence == input.Sequence);
            if (c != null)
            {
                foreach (var Account in accounts)
                {
                    if (Account.Sequence >= input.Sequence && Account.Sequence < 98)
                    {
                        Account.Sequence += 1;
                    }
                }

                foreach (var item in accounts)
                {
                    App.ServiceRepo.UpdateAccount(item);
                }
            }
            else 
            {
                if (input.Sequence > accounts.Count()) 
                {
                    input.Sequence = accounts.Count();
                }
            }
            App.ServiceRepo.AddAccount(input);
            App.CachedAccounts = App.ServiceRepo.GetAccountOrderBySequence();
            AccountModelToPicker();
            Revise_Grid.IsVisible = false;
        }
        else
        {
            return;
        }
    }

    /// <summary>
    /// 情境1：往前移 => endIndex大於startIndex => 修改位置之後所有項目位置-1
    /// 情境2：往後移 => endIndex小於startIndex => 修改位置之後所有項目位置+1
    /// </summary>
    private void ReviseSave_Click()
    {
        var input = InputCheck();
        if (input != null) 
        {
            var newAccounts = App.CachedAccounts;
            int startIndex = (newAccounts.Find(account => account.AccountID == cacheID)).Sequence;

            if (input.Sequence > newAccounts.Count())
            {
                input.Sequence = newAccounts.Count()-1 ;
            }

            int endIndex = input.Sequence; 

            if (endIndex > startIndex)
            {
                foreach (var item in newAccounts)
                {
                    if (item.AccountID == input.AccountID)
                    {
                        item.Sequence = endIndex;
                    }
                    else if (item.Sequence <= endIndex && item.Sequence > startIndex)
                    {
                        item.Sequence -= 1;
                    }
                }
            }
            else if (endIndex < startIndex)
            {
                foreach (var item in newAccounts)
                {
                    if (item.AccountID == input.AccountID)
                    {
                        item.Sequence = endIndex;
                    }
                    else if (item.Sequence >= endIndex && item.Sequence < startIndex)
                    {
                        item.Sequence += 1;
                    }
                }
            }
            else
            {
                return;
            }

            newAccounts = newAccounts.OrderBy(item => item.Sequence).ToList();
            App.CachedAccounts = newAccounts;
            foreach (var item in newAccounts)
            {
                App.ServiceRepo.UpdateAccount(item);
            }
            AccountModelToPicker();
        }
    }


    /// <summary>
    /// 檢查輸入資料
    /// </summary>
    private AccountModel? InputCheck() 
    {
        string name = Name_Entry.Text;
        string sequence = Sequence_Entry.Text;
        int seq = 0;

        if (name.Length > 6) 
        {
            DisplayAlert("名稱已超過6個字", "為追求大字體，請將其拆成兩筆", "OK");
            return null;
        }

        try 
        {
            seq = Convert.ToInt32(sequence);
            if (seq < 0 || seq > 98) 
            {
                DisplayAlert("請輸入一位到兩位正整數", "", "OK");
                return null;
            }
        } 
        catch 
        {
            DisplayAlert("請輸入一位到兩位正整數", "", "OK");
            return null;
        }

        return  new AccountModel { AccountID = cacheID, Name = name, Sequence = seq };
    }

    /// <summary>
    /// 切換SaveButten
    /// </summary>
    private void Save_BTN_Clicked(object sender, EventArgs e)
    {
        if (cacheID == 99) 
        {
            AddSave_Click();
        }
        else
        {
            ReviseSave_Click();
        }
    }

    /// <summary>
    /// 刪除按鈕，Sequence=-1
    /// </summary>
    private async void Delete_BTN_Clicked(object sender, EventArgs e)
    {
        var ans = await DisplayAlert("刪除無法復原，確定要刪除嗎?", "刪除後歷史資料會繼續顯示，\n但無法在新增/修改中再次選取", "刪除", "取消");
        if (ans == true)
        {
            var accounts = App.CachedAccounts;
            var account = accounts.Find(account => account.AccountID == cacheID).Sequence;
            foreach (var a in accounts) 
            {
                if (a.AccountID == cacheID)
                {
                    a.Sequence = -1;
                }
                else if (a.Sequence > account) 
                {
                    a.Sequence -= 1;
                }
            }

            foreach (var a in accounts) 
            {
                App.ServiceRepo.UpdateAccount(a);
            }
            App.CachedAccounts = App.ServiceRepo.GetAccountOrderBySequence();
            AccountModelToPicker();
        }
        return;
    }

    /// <summary>
    /// 切換到類別頁面
    /// </summary>
    private void MenuToggle_BTN_Clicked(object sender, EventArgs e)
    {
        Shell.Current.CurrentItem.CurrentItem.Items.Add(new SetCategoryListPage());
        Shell.Current.CurrentItem.CurrentItem.Items.RemoveAt(0);
    }
}
