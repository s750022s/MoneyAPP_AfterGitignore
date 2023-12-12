using MoneyAPP.Controls;
using MoneyAPP.Models;

namespace MoneyAPP.Pages;

//�p���ק�bug�A�Ф@�֭ק�SetAccountListPage�I

public partial class SetCategoryListPage : ContentPage
{
    int cacheID = 0;
    public SetCategoryListPage()
	{
		InitializeComponent();
        CategoryModelToPicker();
    }

    /// <summary>
    /// ���O���إͦ�
    /// </summary>
    private void CategoryModelToPicker()
    {
        if (App.CachedCategorys == null)
        {
            App.CachedCategorys = App.ServiceRepo.GetCategoryOrderBySequence();
            App.CachedAccounts = App.ServiceRepo.GetAccountOrderBySequence();
        }

        var categorys = App.CachedCategorys.ToList();
        categorys.Add(new CategoryModel { CategoryID = 99, Name = "+", Sequence = 99 });

        DatasCollectionView.ItemsSource = categorys;
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
        var category = App.CachedCategorys.Find(Category => Category.CategoryID == cacheID);
        Name_Entry.Text = category.Name;
        Sequence_Entry.Text = category.Sequence.ToString();
    }

    /// <summary>
    /// ��id=99�A�Ұ�AddSave;
    /// ����1�GSequence�j��ثe�����`�� => �[�b�̫�ASequence=�����`��;
    /// ����2�G�����`�Ƥp��Sequence�A�N��n���b���� => �NSequence��w���ؤ������Sequence+1
    /// </summary>
    private void AddSave_Click()
    {
        var input = InputCheck();
        var categorys = App.CachedCategorys.ToList();
        if (input != null)
        {
            var c = categorys.Find(Category => Category.Sequence == input.Sequence);
            if (c != null)
            {
                foreach (var category in categorys)
                {
                    if (category.Sequence >= input.Sequence && category.Sequence < 98)
                    {
                        category.Sequence += 1;
                    }
                }

                foreach (var item in categorys)
                {
                    App.ServiceRepo.UpdateCategory(item);
                }
            }
            else 
            {
                if (input.Sequence > categorys.Count()) 
                {
                    input.Sequence = categorys.Count();
                }
            }
            App.ServiceRepo.AddCategory(input);
            App.CachedCategorys = App.ServiceRepo.GetCategoryOrderBySequence();
            CategoryModelToPicker();
            Revise_Grid.IsVisible = false;
        }
        else
        {
            return;
        }
    }

    /// <summary>
    /// ����1�G���e�� => endIndex �j�� startIndex => �ק��m����Ҧ����ئ�m-1
    /// ����2�G���Ჾ => endIndex �p�� startIndex => �ק��m����Ҧ����ئ�m+1
    /// </summary>
    private void ReviseSave_Click()
    {
        var input = InputCheck();
        if (input != null) 
        {
            var newCategorys = App.CachedCategorys;
            int startIndex = (newCategorys.Find(Category => Category.CategoryID == cacheID)).Sequence;

            if (input.Sequence > newCategorys.Count())
            {
                input.Sequence = newCategorys.Count()-1 ;
            }

            int endIndex = input.Sequence; 

            if (endIndex > startIndex)
            {
                foreach (var item in newCategorys)
                {
                    if (item.CategoryID == input.CategoryID)
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
                foreach (var item in newCategorys)
                {
                    if (item.CategoryID == input.CategoryID)
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

            newCategorys = newCategorys.OrderBy(item => item.Sequence).ToList();
            App.CachedCategorys = newCategorys;
            foreach (var item in newCategorys)
            {
                App.ServiceRepo.UpdateCategory(item);
            }
            CategoryModelToPicker();
        }
    }

    /// <summary>
    /// �ˬd��J���
    /// </summary>
    private CategoryModel? InputCheck() 
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

        return  new CategoryModel { CategoryID = cacheID, Name = name, Sequence = seq };
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
            var categorys = App.CachedCategorys;
            var category = categorys.Find(Category => Category.CategoryID == cacheID).Sequence;
            foreach (var c in categorys) 
            {
                if (c.CategoryID == cacheID)
                {
                    c.Sequence = -1;
                }
                else if (c.Sequence > category) 
                {
                    c.Sequence -= 1;
                }
            }

            foreach (var c in categorys) 
            {
                App.ServiceRepo.UpdateCategory(c);
            }
            App.CachedCategorys = App.ServiceRepo.GetCategoryOrderBySequence();
            CategoryModelToPicker();
        }
        return;
    }

    /// <summary>
    /// ������b�᭶��
    /// </summary>
    private void MenuToggle_BTN_Clicked(object sender, EventArgs e)
    {
        Shell.Current.CurrentItem.CurrentItem.Items.Add(new SetAccountListPage());
        Shell.Current.CurrentItem.CurrentItem.Items.RemoveAt(0);
    }
}
