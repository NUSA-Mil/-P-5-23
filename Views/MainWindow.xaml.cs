using ShopApp.Views;
using System.Windows;
using System.Windows.Controls;

namespace ShopApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Инициализация RadioButton и CurrentView
            rbCategories = this.FindName("rbCategories") as RadioButton;
            rbProducts = this.FindName("rbProducts") as RadioButton;
            CurrentView = this.FindName("CurrentView") as ContentControl;

            // Установка начального значения
            if (rbCategories != null && rbCategories.IsChecked == true)
            {
                RadioButton_Checked(rbCategories, null);
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (CurrentView == null) return;

            if (rbCategories != null && rbCategories.IsChecked == true)
            {
                CurrentView.Content = new CategoriesView();
            }
            else if (rbProducts != null && rbProducts.IsChecked == true)
            {
                CurrentView.Content = new ProductsView();
            }
        }
    }
}