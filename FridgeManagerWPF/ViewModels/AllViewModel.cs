using System.ComponentModel;

namespace FridgeManagerWPF.ViewModels
{
    public class AllViewModel : INotifyPropertyChanged
    {
        public FoodListViewModel F { get; set; }
        public ProdListViewModel P { get; set; }

        public AllViewModel()
        {
            F = new FoodListViewModel();
            P = new ProdListViewModel();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
