using EthernetShop.Models;
using EthernetShop.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    { 
        public ObservableCollection<Product> products { get; set; } = new();
        public ObservableCollection<Tag> tags { get; set; } = new();
        public ICollectionView productsView { get; set; }
        public string searchQuery { get; set; } = null!;
        public string filterPriceFrom { get; set; } = null!;
        public string filterPriceTo { get; set; } = null!;
        EthernetShopContext context = DBService.Instance.Context;

        private bool[] _categoryModeArray = new bool[] { false, false, false, false, false };
        public bool[] CategoryModeArray
        {
            get { return _categoryModeArray; }
        }
        public int CategorySelectedMode
        {
            get { return Array.IndexOf(_categoryModeArray, true);}
            set
            {
                for (int i = 0; i < 0; i++)
                {
                    _categoryModeArray[i] = false;
                }
            }
                    
        }

        private bool[] _brandyModeArray = new bool[] { false, false, false, false, false };
        public bool[] BrandModeArray
        {
            get { return _brandyModeArray; }
        }
        public int BrandSelectedMode
        {
            get { return Array.IndexOf(_brandyModeArray, true); }
            set
            {
                for (int i = 0; i < 0; i++)
                {
                    _brandyModeArray[i] = false;
                }
            }
        }


        public MainPage()
        {
            LoadList();
            productsView = CollectionViewSource.GetDefaultView(products);
            productsView.Filter = FilterProducts;

            InitializeComponent();
        }
        //object sender, EventArgs e
        public void LoadList() // СДЕЛАТЬ ТЭГИ
        {
            tags.Clear();
            foreach (var tag in context.Tags.ToList())
            {
                tags.Add(tag);
            }

            products.Clear();

            // Используем Include для загрузки связанных тегов
            foreach (var product in context.Products
                .Include(p => p.Tags)  // Это важно!
                .ToList())
            {
                // Для проверки - можно оставить или убрать
                foreach (var tag in product.Tags)
                {
                    Console.WriteLine($"Product: {product.Name}, Tag: {tag.Name}");
                }

                products.Add(product);
            }
        }

        public bool FilterProducts(object obj)
        {
            if (obj is not Product)
                return false;
            var product = (Product)obj;
            if (searchQuery != null && !product.Name.Contains(searchQuery, StringComparison.CurrentCultureIgnoreCase))
                return false;
            if (!filterPriceFrom.IsNullOrEmpty() && Convert.ToDouble(filterPriceFrom) > product.Price)
                return false;
            if (!filterPriceTo.IsNullOrEmpty() && Convert.ToDouble(filterPriceTo) < product.Price)
                return false;
            if (CategorySelectedMode != -1)
            {
                if (product.CategoryId != CategorySelectedMode + 1)
                    return false;
            }

            if (BrandSelectedMode != -1)
            {
                if (product.BrandId != BrandSelectedMode + 1)
                    return false;
            }
            return true;
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            productsView.Refresh();
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            productsView.SortDescriptions.Clear();
            var cb = (ComboBox)sender;
            var selected = (ComboBoxItem)cb.SelectedItem;
            switch (selected.Tag)
            {
                case "Name":
                    productsView.SortDescriptions.Add(new SortDescription("Name",
                    ListSortDirection.Ascending));
                    break;
                case "Price":
                    productsView.SortDescriptions.Add(new SortDescription("Price",
                    ListSortDirection.Ascending));
                    break;
                case "Count":
                    productsView.SortDescriptions.Add(new SortDescription("Stock",
                    ListSortDirection.Ascending));
                    break;
            }
            productsView.Refresh();
        }

        private void RadioButton_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            productsView.Refresh();
          
           

        }

        private void Button_Click(object sender, RoutedEventArgs e) // СБРОС
        {
            //searchQuery = null;
            //filterPriceFrom = null;
            //filterPriceTo = null;
            //CategorySelectedMode = -1;
            //BrandSelectedMode = -1;
            //productsView.Refresh();

        }

        private void brand(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new BrandPage());
        }

        private void category(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CategoryPage());
        }

        private void tag(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TagPage());
        }
    }
}
