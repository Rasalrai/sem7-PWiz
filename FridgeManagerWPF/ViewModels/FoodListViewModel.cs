using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Data;
using System.Windows.Input;

namespace FridgeManagerWPF.ViewModels
{
    public class FoodListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<FoodViewModel> food;
        public ObservableCollection<FoodViewModel> Food
        {
            get => food;
            set
            {
                food = value;
                RaisePropertyChanged(nameof(Food));
            }
        }

        private ObservableCollection<Interfaces.IProducer> producers;

        public ObservableCollection<Interfaces.IProducer> Producers
        {
            get => producers;
            set
            {
                producers = value;
                RaisePropertyChanged(nameof(Producers));
            }
        }

        private BLC.BLC blc;

        private ListCollectionView view;

        public FoodListViewModel()
        {
            //BLC.BLC.LibName = "FoodDB1.dll";
            //BLC.BLC.LibName = "FoodDBEF.dll";
            BLC.BLC.LibName = ConfigurationManager.AppSettings["dbName"];
            blc = BLC.BLC.GetBLC();

            food = new ObservableCollection<FoodViewModel>();
            view = (ListCollectionView)CollectionViewSource.GetDefaultView(food);

            foreach (var f in blc.GetFood())
            {
                Food.Add(new FoodViewModel(f));
            }
            RaisePropertyChanged(nameof(Food));

            Producers = new ObservableCollection<Interfaces.IProducer>(blc.GetProducers());

            Action<object> act = AddNewItem;

            EditedItem = null;
            SelectedItem = null;

            addNewItemCmd = new RelayCommand(AddNewItem, param => CanAddNewItem());
            saveItemCmd = new RelayCommand(param => SaveChanges(), param => CanAddToList());
            removeItemCmd = new RelayCommand(param => RemoveItem(), param => CanRemove());
            filterDataCmd = new RelayCommand(param => FilterData());
        }
        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void _saveDB()
        {
            blc.SaveItem(null);
        }

        #region AddNewItem Command implementation

        private FoodViewModel editedItem = null;

        public FoodViewModel EditedItem
        {
            get => editedItem;
            set
            {
                editedItem = value;
                RaisePropertyChanged(nameof(EditedItem));
            }
        }
        private void AddNewItem(object obj)
        {
            //Food.Add(new FoodViewModel(blc.CreateNewItem()));
            FoodViewModel fvm = new FoodViewModel(blc.CreateNewItem());
            EditedItem = fvm;
            fvm.IsChanged = true;
            SelectedItem = null;
        }

        // for creating new items
        private bool CanAddNewItem()
        {
            return EditedItem == null || !EditedItem.IsChanged;
        }

        private void SaveChanges()
        {
            // add newly created item
            // TODO compare selected and edited, like in remove cmd
            if (SelectedItem == null)
            {
                AddItemToList();
            }
            // just update db with up-to-date data of existing items
            else if (!EditedItem.HasErrors)
            {
                // TODO save changes in DAO
                _saveDB();
            }
        }

        private void AddItemToList()
        {
            if (!EditedItem.HasErrors)
            {
                food.Add(editedItem);
                EditedItem.SaveItem();
                EditedItem = null;
            }
            // TODO else: display error
        }

        private bool CanAddToList()
        {
            if (EditedItem == null)
            {
                return false;
            }
            return !EditedItem.HasErrors;
        }

        private void RemoveItem()
        {
            // item that's not yet saved
            if (SelectedItem != EditedItem)
            {
                EditedItem = null;
            }
            else
            {
                EditedItem.Remove();
                Food.Remove(EditedItem);
            }
        }

        private bool CanRemove()
        {
            return !(SelectedItem == null && EditedItem == null);
        }

        private FoodViewModel selectedItem = null;

        public FoodViewModel SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value;
                if (CanAddNewItem())
                {
                    EditedItem = SelectedItem;
                }
                
                RaisePropertyChanged(nameof(SelectedItem));
            }
        }

        private string filterValue;

        public string FilterValue
        {
            get => filterValue;
            set
            {
                filterValue = value;
            }
        }

        private  void FilterData()
        {
            if (string.IsNullOrWhiteSpace(filterValue))
            {
                view.Filter = null;
            }
            else
            {
                view.Filter = (f) => ((FoodViewModel)f).Name.Contains(filterValue);
                view.SortDescriptions.Add(new SortDescription(nameof(FoodViewModel.Name), ListSortDirection.Ascending));
            }
        }

        private RelayCommand filterDataCmd;

        public ICommand FilterDataCmd
        {
            get => filterDataCmd;
        }

        private RelayCommand addNewItemCmd;

        public ICommand AddNewItemCmd
        {
            get => addNewItemCmd;
        }

        private RelayCommand saveItemCmd;

        public ICommand SaveItemCmd
        {
            get => saveItemCmd;
        }

        private RelayCommand removeItemCmd;

        public ICommand RemoveItemCmd
        {
            get => removeItemCmd;
        }

        #endregion
    }
}
