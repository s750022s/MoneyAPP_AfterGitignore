using MoneyAPP.Controls;
using MoneyAPP.Models;

namespace MoneyAPP.Pages;

//�p���ק�bug�A�Ф@�֭ק�SetAccountListPage�I

/// <summary>
/// �b����]�w��
/// </summary>
public partial class SetAccountListPage : ContentPage
{
    /// <summary>
    /// �Ȧs�b��ID
    /// </summary>
    int cacheID = 0;


    public SetAccountListPage()
	{
		InitializeComponent();
        AccountModelToPicker();
    }

    /// <summary>
    /// �b�ᶵ�إͦ�
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
    /// �I��Border�A�|���X��Border�����
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
    /// ��id=99�A�Ұ�AddSave;
    /// ����1�GSequence�j��ثe�����`�� => �[�b�̫�ASequence=�����`��;
    /// ����2�G�����`�Ƥp��Sequence�A�N��n���b���� => �NSequence��w���ؤ������Sequence+1
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
    /// ����1�G���e�� => endIndex�j��startIndex => �ק��m����Ҧ����ئ�m-1
    /// ����2�G���Ჾ => endIndex�p��startIndex => �ק��m����Ҧ����ئ�m+1
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
    /// �ˬd��J���
    /// </summary>
    private AccountModel? InputCheck() 
    {
        string name = Name_Entry.Text;
        string sequence = Sequence_Entry.Text;
        int seq = 0;

        if (name.Length > 6) 
        {
            DisplayAlert("�W�٤w�W�L6�Ӧr", "���l�D�j�r��A�бN���ⵧ", "OK");
            return null;
        }

        try 
        {
            seq = Convert.ToInt32(sequence);
            if (seq < 0 || seq > 98) 
            {
                DisplayAlert("�п�J�@����쥿���", "", "OK");
                return null;
            }
        } 
        catch 
        {
            DisplayAlert("�п�J�@����쥿���", "", "OK");
            return null;
        }

        return  new AccountModel { AccountID = cacheID, Name = name, Sequence = seq };
    }

    /// <summary>
    /// ����SaveButten
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
    /// �R�����s�ASequence=-1
    /// </summary>
    private async void Delete_BTN_Clicked(object sender, EventArgs e)
    {
        var ans = await DisplayAlert("�R���L�k�_��A�T�w�n�R����?", "�R������v��Ʒ|�~����ܡA\n���L�k�b�s�W/�ק襤�A�����", "�R��", "����");
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
    /// ���������O����
    /// </summary>
    private void MenuToggle_BTN_Clicked(object sender, EventArgs e)
    {
        Shell.Current.CurrentItem.CurrentItem.Items.Add(new SetCategoryListPage());
        Shell.Current.CurrentItem.CurrentItem.Items.RemoveAt(0);
    }
}
