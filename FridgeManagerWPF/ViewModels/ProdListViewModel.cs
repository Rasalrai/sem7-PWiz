using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Data;
using System.Windows.Input;

namespace FridgeManagerWPF.ViewModels
{
    public class ProdListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<ProdViewModel> producers;

        public ObservableCollection<ProdViewModel> Producers
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

        public ProdListViewModel()
        {
            BLC.BLC.LibName = ConfigurationManager.AppSettings["dbName"];
            blc = BLC.BLC.GetBLC();

            producers = new ObservableCollection<ProdViewModel>();
            view = (ListCollectionView)CollectionViewSource.GetDefaultView(producers);

            foreach (var p in blc.GetProducers())
            {
                Producers.Add(new ProdViewModel(p));
                if (p.ID > maxID)
                {
                    maxID = p.ID;
                }
            }
            RaisePropertyChanged(nameof(Producers));

            EditedItem = null;
            SelectedItem = null;

            addNewProdCmd = new RelayCommand(AddNewItem, param => CanAddNewItem());
            saveProdCmd = new RelayCommand(param => SaveChanges(), param => CanAddToList());
            removeProdCmd = new RelayCommand(param => RemoveItem(), param => CanRemove());
            filterProdDataCmd = new RelayCommand(param => FilterProdData());
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

        private ProdViewModel editedItem = null;

        public ProdViewModel EditedItem
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
            ProdViewModel pvm = new ProdViewModel(blc.CreateNewProducer());
            EditedItem = pvm;
            pvm.IsChanged = true;
            SelectedItem = null;

            maxID++;
            pvm.ProdID = maxID;
        }

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
                producers.Add(editedItem);
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
                if (EditedItem.ProdID == maxID)
                {
                    maxID--;
                }
                EditedItem = null;
                SelectedItem = null;
            }
            else
            {
                EditedItem.Remove();
                Producers.Remove(EditedItem);
                EditedItem = null;
                SelectedItem = null;
            }
        }

        private bool CanRemove()
        {
            return !(SelectedItem == null && EditedItem == null);
        }

        private ProdViewModel selectedItem = null;

        public ProdViewModel SelectedItem
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

        private string filterProdValue;

        public string FilterProdValue
        {
            get => filterProdValue;
            set
            {
                filterProdValue = value;
            }
        }

        private void FilterProdData()
        {
            if (string.IsNullOrWhiteSpace(filterProdValue))
            {
                view.Filter = null;
            }
            else
            {
                view.Filter = (p) => ((ProdViewModel)p).Name.ToLower().Contains(filterProdValue.ToLower());
                view.SortDescriptions.Add(new SortDescription(nameof(ProdViewModel.ProdID), ListSortDirection.Ascending));
            }
        }

        private RelayCommand filterProdDataCmd;

        public ICommand FilterProdDataCmd
        {
            get => filterProdDataCmd;
        }

        private RelayCommand addNewProdCmd;

        public ICommand AddNewProdCmd
        {
            get => addNewProdCmd;
        }

        private RelayCommand saveProdCmd;

        public ICommand SaveProdCmd
        {
            get => saveProdCmd;
        }

        private RelayCommand removeProdCmd;

        public ICommand RemoveProdCmd
        {
            get => removeProdCmd;
        }

        #endregion
    }
}
