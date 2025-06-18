using ShopApp.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Windows.Input;

namespace ShopApp.ViewModels
{
    public class ProductsViewModel : ViewModelBase
    {
        private readonly ShopDbContext _context;
        private ObservableCollection<Product> _products;
        private ObservableCollection<Category> _categories;
        private Product _selectedProduct;
        private Product _currentProduct;
        private bool _isEditing;

        public ObservableCollection<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        public ObservableCollection<Category> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }

        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
                OnPropertyChanged(nameof(IsProductSelected));
            }
        }

        public Product CurrentProduct
        {
            get => _currentProduct;
            set
            {
                _currentProduct = value;
                OnPropertyChanged(nameof(CurrentProduct));
            }
        }

        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                _isEditing = value;
                OnPropertyChanged(nameof(IsEditing));
            }
        }

        public bool IsProductSelected => SelectedProduct != null;

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public ProductsViewModel()
        {
            _context = new ShopDbContext();
            LoadData();

            AddCommand = new RelayCommand(_ => AddProduct());
            EditCommand = new RelayCommand(_ => EditProduct(), _ => IsProductSelected);
            DeleteCommand = new RelayCommand(_ => DeleteProduct(), _ => IsProductSelected);
            RefreshCommand = new RelayCommand(_ => LoadData());
            SaveCommand = new RelayCommand(_ => SaveProduct());
            CancelCommand = new RelayCommand(_ => CancelEdit());
        }

        private void LoadData()
        {
            _context.Products.Include(p => p.Category).Load();
            _context.Categories.Load();

            Products = _context.Products.Local;
            Categories = _context.Categories.Local;
            IsEditing = false;
        }

        private void AddProduct()
        {
            CurrentProduct = new Product { CategoryId = Categories.FirstOrDefault()?.CategoryId ?? 0 };
            IsEditing = true;
        }

        private void EditProduct()
        {
            CurrentProduct = new Product
            {
                ProductId = SelectedProduct.ProductId,
                Name = SelectedProduct.Name,
                Price = SelectedProduct.Price,
                Quantity = SelectedProduct.Quantity,
                CategoryId = SelectedProduct.CategoryId
            };
            IsEditing = true;
        }

        private void SaveProduct()
        {
            if (SelectedProduct == null || CurrentProduct.ProductId == 0)
            {
                _context.Products.Add(CurrentProduct);
            }
            else
            {
                var product = _context.Products.Find(CurrentProduct.ProductId);
                if (product != null)
                {
                    product.Name = CurrentProduct.Name;
                    product.Price = CurrentProduct.Price;
                    product.Quantity = CurrentProduct.Quantity;
                    product.CategoryId = CurrentProduct.CategoryId;
                }
            }
            _context.SaveChanges();
            LoadData();
        }

        private void CancelEdit()
        {
            IsEditing = false;
        }

        private void DeleteProduct()
        {
            if (SelectedProduct == null) return;

            var product = _context.Products.Find(SelectedProduct.ProductId);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
                LoadData();
            }
        }
    }
}