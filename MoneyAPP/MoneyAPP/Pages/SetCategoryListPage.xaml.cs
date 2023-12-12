using MoneyAPP.Controls;
using MoneyAPP.Models;

namespace MoneyAPP.Pages;

//如有修改bug，請一併修改SetAccountListPage！

public partial class SetCategoryListPage : ContentPage
{
    int cacheID = 0;
    public SetCategoryListPage()
	{
		InitializeComponent();
        CategoryModelToPicker();
    }

    /// <summary>
    /// 類別項目生成
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
        var category = App.CachedCategorys.Find(Category => Category.CategoryID == cacheID);
        Name_Entry.Text = category.Name;
        Sequence_Entry.Text = category.Sequence.ToString();
    }

    /// <summary>
    /// 當id=99，啟動AddSave;
    /// 情境1：Sequence大於目前項目總數 => 加在最後，Sequence=項目總數;
    /// 情境2：項目總數小於Sequence，代表要插在中間 => 將Sequence鎖定項目之後全數Sequence+1
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
    /// 情境1：往前移 => endIndex 大於 startIndex => 修改位置之後所有項目位置-1
    /// 情境2：往後移 => endIndex 小於 startIndex => 修改位置之後所有項目位置+1
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
    /// 檢查輸入資料
    /// </summary>
    private CategoryModel? InputCheck() 
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

        return  new CategoryModel { CategoryID = cacheID, Name = name, Sequence = seq };
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
    /// 切換到帳戶頁面
    /// </summary>
    private void MenuToggle_BTN_Clicked(object sender, EventArgs e)
    {
        Shell.Current.CurrentItem.CurrentItem.Items.Add(new SetAccountListPage());
        Shell.Current.CurrentItem.CurrentItem.Items.RemoveAt(0);
    }
}
