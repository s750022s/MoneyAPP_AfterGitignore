using System.Collections.ObjectModel;
using System.ComponentModel;
using ZMoney.Models;
using ZMoney.Services;


namespace ZMoney.ViewModels
{
    public class ListModel: INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        int _id, _sequence, _currentTotal;
        string _name = "";

        public int Id { get => _id;set { _id = value; OnPropertyChanged("Id"); } }
        public string Name { get => _name; set { _name = value; OnPropertyChanged("Name"); } }
        public int Sequence { get => _sequence; set { _sequence = value; OnPropertyChanged("Sequence"); } }
        public int CurrentTotal { get => _currentTotal; set { _currentTotal = value; OnPropertyChanged("CurrentTotal"); } }

        public ListModel(int id, string name, int sequence) 
        {
            Id = id;
            Name = name;
            Sequence = sequence;
        }

        public ListModel(CategoryModel categoryModel) 
        { 
            Id = categoryModel.Id;
            Name = categoryModel.Name;
            Sequence = categoryModel.Sequence;
        }

        public ListModel(AccountModel accountModel)
        {
            Id = accountModel.Id;
            Name = accountModel.Name;
            Sequence = accountModel.Sequence;
            CurrentTotal = accountModel.CurrentTotal;
        }

    }

    public class ListSetViewModel
    {
        private bool IsCategory = true;
        private ObservableCollection<ListModel> dataList = [];

        public ListSetViewModel(DbManager dbManager) 
        {
            SharedMethod.CheckAppCached(dbManager);
        }


        public ObservableCollection<ListModel> SetPageData() 
        {
            dataList.Clear();
            if (IsCategory)
            {
                foreach (var item in App.CachedCategorys)
                {
                    dataList.Add(new ListModel(item));
                }
            }
            else 
            {
                foreach (var item in App.CachedAccounts)
                {
                    dataList.Add(new ListModel(item));
                }
            }

            dataList.Add(new ListModel(999,"+",999));
            return dataList;
        }

        public ListModel GetContent(int id) 
        {
            return dataList.First(x => x.Id == id);
        }

        public string CategoryChangeAccount_Clicked() 
        {
            IsCategory = !IsCategory;
            return IsCategory?"類別":"帳戶";
        }

        private void InputCheck(string name, string sequence, out int sequenceInt) 
        {
            if (name.Length > 6) 
            {
                throw new ArgumentException("名稱請不要超過6個字。");
            }

           sequence = sequence == "" ? "100" : sequence;
            if (!int.TryParse(sequence.Replace(",", ""), out sequenceInt))
            {
                throw new ArgumentException("請輸入一位或兩位正整數。");
            }
            else if (sequenceInt < 0 || sequenceInt > 99) 
            { 
            throw new ArgumentException("請輸入一位或兩位正整數。");
            }
        }

        private void AddFormula(string name , int sequence, object target) 
        {
            if (IsCategory)
            {
                var target = App.CachedCategorys;
            }
            else 
            {
                var target = App.CachedAccounts;
            }


            var getSequenceItem = dataList.First(x => x.Sequence == sequence);
            if (getSequenceItem != null)
            {
                foreach (var item in dataList)
                {
                    if (item.Sequence >= sequence && item.Sequence < 99)
                    {
                        item.Sequence += 1;
                    }
                }

                foreach (var item in categorys)
                {
                    App.ServiceRepo.UpdateCategory(item);
                }
            }
            else 
            {
                sequence = Math.Min(sequence, dataList.Count);
            }
        }

    }
}
