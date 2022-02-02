using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FridgeManagerWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //public ViewModels.FoodViewModel FoodItem { get; set; }

        //public ViewModels.FoodListViewModel FoodLVM { get; set; }
        public MainWindow()
        {
            //BLC.BLC blc = new BLC.BLC("FoodDB2.dll");
            //FoodItem = new ViewModels.FoodViewModel( blc.GetFood().First());
            //this.DataContext = FoodItem;
            //FoodLVM = new ViewModels.FoodListViewModel();
            InitializeComponent();
            // Food = new ObservableCollection<Interfaces.IFood>( blc.GetFood());
            //Lista.ItemsSource = Food;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //FoodItem.Name = "Nowe żarełko";
            //FoodLVM.Food.Add(FoodLVM.Food[0]);
        }
    }
}

/*
 poza edited item, zrób też selected item - albo jakoś inaczej żeby można było
    zacząć tworzyć nowy rekord, potem zedytować istniejący, i dokończyć
    tworzenie nowego
 */