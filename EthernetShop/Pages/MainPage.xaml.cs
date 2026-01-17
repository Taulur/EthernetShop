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
        bool isAdmin = false;
        public ObservableCollection<Product> products { get; set; } = new();
        public ObservableCollection<Tag> tags { get; set; } = new();
        public ICollectionView productsView { get; set; }
        public string searchQuery { get; set; } = null!;
        public string filterPriceFrom { get; set; } = null!;
        public string filterPriceTo { get; set; } = null!;
        public Product current { get; set; } = new();
        EthernetShopContext context = DBService.Instance.Context;

        private bool[] _categoryModeArray = new bool[] { false, false, false, false, false };
        public bool[] CategoryModeArray
        {
            get { return _categoryModeArray; }
            set { _categoryModeArray = value; }
        }
        public int CategorySelectedMode
        {
            get { return Array.IndexOf(_categoryModeArray, true); }
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
            set { _brandyModeArray = value; }
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


        public MainPage(bool _isAdmin)
        {
            LoadList();
            productsView = CollectionViewSource.GetDefaultView(products);
            productsView.Filter = FilterProducts;

            isAdmin = _isAdmin;

            

            InitializeComponent();


            var adminButtons = new List<Button>
                  {
        brandButton,
        categoryButton,
        tagButton,
        productButton
    };
            foreach (var button in adminButtons)
            {
                button.Visibility = _isAdmin ? Visibility.Visible : Visibility.Collapsed;
            }

            BuildCategoryRadioButtons();
            BuildBrandRadioButtons();
        }
        public void LoadList() 
        {
            tags.Clear();
            foreach (var tag in context.Tags.ToList())
            {
                tags.Add(tag);
            }

            products.Clear();

            foreach (var product in context.Products
                .Include(p => p.Tags) 
                .ToList())
            {
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
            if (!filterPriceFrom.IsNullOrEmpty() && Convert.ToDecimal(filterPriceFrom) > product.Price)
                return false;
            if (!filterPriceTo.IsNullOrEmpty() && Convert.ToDecimal(filterPriceTo) < product.Price)
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
            if (selected != null)
            {
                switch (selected.Tag)
                {
                    case "Name":
                        productsView.SortDescriptions.Add(new SortDescription("Name",
                        ListSortDirection.Ascending));
                        break;
                    case "PriceAscending":
                        productsView.SortDescriptions.Add(new SortDescription("Price",
                        ListSortDirection.Ascending));
                        break;
                    case "PriceDescending":
                        productsView.SortDescriptions.Add(new SortDescription("Price",
                        ListSortDirection.Descending));
                        break;
                    case "StockAscending":
                        productsView.SortDescriptions.Add(new SortDescription("Stock",
                        ListSortDirection.Ascending));
                        break;
                    case "StockDescending":
                        productsView.SortDescriptions.Add(new SortDescription("Stock",
                        ListSortDirection.Descending));
                        break;
                }
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

        private void Button_Click(object sender, RoutedEventArgs e) 
        {
         
            searchQuery = null;
            filterPriceFrom = null;
            filterPriceTo = null;

        
            if (_categoryModeArray != null)
            {
                for (int i = 0; i < _categoryModeArray.Length; i++)
                {
                    _categoryModeArray[i] = false;
                }
            }

            if (_brandyModeArray != null)
            {
                for (int i = 0; i < _brandyModeArray.Length; i++)
                {
                    _brandyModeArray[i] = false;
                }
            }

           
            SortBox.SelectedIndex = -1;

          
            if (SearchBox != null)
                SearchBox.Text = string.Empty;

            if (PriceFromBox != null)
                PriceFromBox.Text = string.Empty;

            if (PriceToBox != null)
                PriceToBox.Text = string.Empty;

          
            productsView.Refresh();

        }

        private void brand(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new BrandPage());
        }

        private void category(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CategoryPage());
        }

        private void product(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditProductPage());
        }

        private void tag(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TagPage());
        }

        private void edit(object sender, RoutedEventArgs e)
        {
            if (isAdmin)
                NavigationService.Navigate(new EditProductPage(current));

        }

        private void BuildCategoryRadioButtons()
        {
            var categories = context.Categories
                                    .ToList();

            Array.Resize(ref _categoryModeArray, categories.Count);
            CategoryModeArray = _categoryModeArray;

            for (int i = 0; i < categories.Count; i++)
            {
                var rb = new RadioButton
                {
                    Content = categories[i].Name,
                    Tag = categories[i].Id
                };

                var binding = new Binding
                {
                    Source = CategoryModeArray,
                    Path = new PropertyPath($"[{i}]"),
                    Mode = BindingMode.TwoWay
                };
                rb.SetBinding(RadioButton.IsCheckedProperty, binding);

                rb.Checked += RadioButton_Checked;

                CategoryPanel.Children.Add(rb);
            }
        }

        private void BuildBrandRadioButtons()
        {
            var brands = context.Brands
                                .ToList();

            Array.Resize(ref _categoryModeArray, brands.Count);
            BrandModeArray = _categoryModeArray;

            for (int i = 0; i < brands.Count; i++)
            {
                var rb = new RadioButton
                {
                    Content = brands[i].Name,
                    Tag = brands[i].Id
                };

                var binding = new Binding
                {
                    Source = BrandModeArray,
                    Path = new PropertyPath($"[{i}]"),
                    Mode = BindingMode.TwoWay
                };
                rb.SetBinding(RadioButton.IsCheckedProperty, binding);

                rb.Checked += RadioButton_Checked;

                BrandPanel.Children.Add(rb);
            }
        }
    }
}
