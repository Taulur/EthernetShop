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
    /// Логика взаимодействия для EditBrandPage.xaml
    /// </summary>
    public partial class EditBrandPage : Page
    {
        Brand _brand = new();
        BrandService service = new();
        bool IsEdit = false;
        public EditBrandPage(Brand? brand = null)
        {
            InitializeComponent();
            if (brand != null)
            {
                service.LoadRelation(brand, "Products");
                _brand = brand;
                IsEdit = true;
            }
            DataContext = _brand;
        }
        private void save(object sender, RoutedEventArgs e)
        {
            if (IsEdit)
                service.Commit();
            else
                service.Add(_brand);
            back(sender, e);
        }
        private void back(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
