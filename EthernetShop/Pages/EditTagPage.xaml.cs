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
    /// Логика взаимодействия для EditTagPage.xaml
    /// </summary>
    public partial class EditTagPage : Page
    {
        Tag _tag = new();
        TagService service = new();
        bool IsEdit = false;
        public EditTagPage(Tag? tag = null)
        {
            InitializeComponent();
            if (tag != null)
            {
              
                _tag = tag;
                IsEdit = true;
            }
            DataContext = _tag;
        }
        private void save(object sender, RoutedEventArgs e)
        {
            if (IsEdit)
                service.Commit();
            else
                service.Add(_tag);
            back(sender, e);
        }
        private void back(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
