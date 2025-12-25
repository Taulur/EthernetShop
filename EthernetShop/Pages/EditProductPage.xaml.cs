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
    /// Логика взаимодействия для EditProductPage.xaml
    /// </summary>
    /// 



    public partial class EditProductPage : Page
    {

        bool isEdit = false;
        public Product product { get; set; } = new();
        public BrandService brandService { get; set; } = new();
        public CategoryService categoryService { get; set; } = new();
        public ProductService productService { get; set; } = new();
        public int categoryId { get; set; } = new();
        public int brandId { get; set; } = new();
        public EditProductPage(Product? _editProduct = null)
        {
            if (_editProduct != null)
            {
                product = _editProduct;
                isEdit = true;
            }
            else
            {
             
            }
          
            InitializeComponent();
        }

        private void save(object sender, RoutedEventArgs e)
        {
            foreach (var brand in brandService.Brands)
            {
                if (brand.Id == brandId)
                {
                    product.Brand = brand;
                    break;
                }
            }
            foreach (var category in categoryService.Categories)
            {
                if (category.Id == categoryId)
                {
                    product.Category = category;
                    break;
                }
            }
            if (isEdit)
            {
                productService.Commit();

            }
            else
            {

                product.CreatedAt = DateTime.Now.ToString();


                productService.Add(product);
            }
            NavigationService.GoBack();

        }

        private void back(object sender, RoutedEventArgs e)
        {
           
            NavigationService.GoBack();

        }

    }
}
