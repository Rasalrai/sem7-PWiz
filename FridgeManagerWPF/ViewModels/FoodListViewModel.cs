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
            BLC.BLC.LibName = ConfigurationManager.AppSettings["dbName"];
            blc = BLC.BLC.GetBLC();

            food = new ObservableCollection<FoodViewModel>();
            view = (ListCollectionView)CollectionViewSource.GetDefaultView(food);

            foreach (var f in blc.GetFood())
            {
                Food.Add(new FoodViewModel(f));
                if (f.ID > maxID)
                {
                    maxID = f.ID;
                }
            }
            RaisePropertyChanged(nameof(Food));

            Producers = new ObservableCollection<Interfaces.IProducer>(blc.GetProducers());

            //Action<object> act = AddNewItem;

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

        #region Command implementation

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

        private int maxID = 0;

        private void AddNewItem(object obj)
        {
            //Food.Add(new FoodViewModel(blc.CreateNewItem()));
            FoodViewModel fvm = new FoodViewModel(blc.CreateNewItem());
            EditedItem = fvm;
            fvm.IsChanged = true;
            SelectedItem = null;

            maxID++;
            fvm.FoodID = maxID;
        }

        // for creating new items
        private bool CanAddNewItem()
        {
            return EditedItem == null || !EditedItem.IsChanged;
        }

        private void SaveChanges()
        {
            if (SelectedItem == null)
            {
                AddItemToList();
            }
            else if (!EditedItem.HasErrors)
            {
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
                if (EditedItem.FoodID == maxID)
                {
                    maxID--;
                }
                EditedItem = null;
                SelectedItem = null;
            }
            else
            {
                EditedItem.Remove();
                Food.Remove(EditedItem);
                EditedItem = null;
                SelectedItem = null;
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

        private void FilterData()
        {
            if (string.IsNullOrWhiteSpace(filterValue))
            {
                view.Filter = null;
            }
            else
            {
                view.Filter = (f) => ((FoodViewModel)f).Name.ToLower().Contains(filterValue.ToLower());
                view.SortDescriptions.Add(new SortDescription(nameof(FoodViewModel.FoodID), ListSortDirection.Ascending));
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
