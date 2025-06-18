using ShopApp.ViewModels;
using System.Windows.Controls;

namespace ShopApp.Views
{
    public partial class CategoriesView : UserControl
    {
        public CategoriesView()
        {
            InitializeComponent();
            DataContext = new CategoriesViewModel();
        }
    }
}