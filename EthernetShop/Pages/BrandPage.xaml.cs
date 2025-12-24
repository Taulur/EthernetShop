using EthernetShop.Models;
using EthernetShop.Services;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace EthernetShop.Pages
{
    /// <summary>
    /// Логика взаимодействия для BrandPage.xaml
    /// </summary>
    public partial class BrandPage : Page
    {
        public BrandService service { get; set; } = new();
        public Brand? current { get; set; } = null;
        public BrandPage()
        {
            InitializeComponent();
        }
        private void back(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        private void add(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditBrandPage());
        }
        private void edit(object sender, RoutedEventArgs e)
        {
            if (current != null)
            {
                NavigationService.Navigate(new EditBrandPage(current));
            }
            else
            {
                MessageBox.Show("Выберите бренд");
            }
        }
        private void remove(object sender, RoutedEventArgs e)
        {
            if (current != null)
            {
                if (MessageBox.Show("Вы действительно хотите удалить бренд?",
                "Удалить бренд?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    service.Remove(current);
                }
            }
            else
            {
                MessageBox.Show("Выберите бренд для удаления", "Выберите бренд",
                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

     
    }
}
