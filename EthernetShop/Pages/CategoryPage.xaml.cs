using EthernetShop.Models;
using EthernetShop.Services;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для CategoryPage.xaml
    /// </summary>
    public partial class CategoryPage : Page
    {
        public CategoryService service { get; set; } = new();
        public Category? current { get; set; } = null;
        public CategoryPage()
        {
            InitializeComponent();
        }
        private void back(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        private void add(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditCategoryPage());
        }
        private void edit(object sender, RoutedEventArgs e)
        {
            if (current != null)
            {
                NavigationService.Navigate(new EditCategoryPage(current));
            }
            else
            {
                MessageBox.Show("Выберите категорию");
            }
        }
        private void remove(object sender, RoutedEventArgs e)
        {
            if (current != null)
            {
                if (MessageBox.Show("Вы действительно хотите удалить категорию?",
                "Удалить категорию?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    service.Remove(current);
                }
            }
            else
            {
                MessageBox.Show("Выберите категорию для удаления", "Выберите категорию",
                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
