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
    /// Логика взаимодействия для EditCategoryPage.xaml
    /// </summary>
    public partial class EditCategoryPage : Page
    {
        Category _category = new();
        CategoryService service = new();
        bool IsEdit = false;
        public EditCategoryPage(Category? category = null)
        {
            InitializeComponent();
            if (category != null)
            {
                service.LoadRelation(category, "Products");
                _category = category;
                IsEdit = true;
            }
            DataContext = _category;
        }
        private void save(object sender, RoutedEventArgs e)
        {
            if (IsEdit)
                service.Commit();
            else
                service.Add(_category);
            back(sender, e);
        }
        private void back(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
