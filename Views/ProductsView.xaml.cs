using ShopApp.ViewModels;
using System.Windows.Controls;

namespace ShopApp.Views
{
    public partial class ProductsView : UserControl
    {
        public ProductsView()
        {
            InitializeComponent();
            DataContext = new ProductsViewModel();
        }
    }
}