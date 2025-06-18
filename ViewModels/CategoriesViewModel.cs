using ShopApp.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ShopApp.ViewModels
{
    public class CategoriesViewModel : ViewModelBase
    {
        private readonly ShopDbContext _context;
        private ObservableCollection<Category> _categories;
        private Category _selectedCategory;
        private Category _currentCategory;
        private bool _isEditing;

        public ObservableCollection<Category> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }

        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
                OnPropertyChanged(nameof(IsCategorySelected));
            }
        }

        public Category CurrentCategory
        {
            get => _currentCategory;
            set
            {
                _currentCategory = value;
                OnPropertyChanged(nameof(CurrentCategory));
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

        public bool IsCategorySelected => SelectedCategory != null;

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public CategoriesViewModel()
        {
            _context = new ShopDbContext();
            LoadCategories();

            AddCommand = new RelayCommand(_ => {
                CurrentCategory = new Category();
                IsEditing = true;
            });

            EditCommand = new RelayCommand(_ => {
                if (SelectedCategory != null)
                {
                    CurrentCategory = new Category
                    {
                        CategoryId = SelectedCategory.CategoryId,
                        Name = SelectedCategory.Name,
                        Description = SelectedCategory.Description
                    };
                    IsEditing = true;
                }
            }, _ => IsCategorySelected);

            DeleteCommand = new RelayCommand(_ => {
                if (SelectedCategory != null)
                {
                    try
                    {
                        var category = _context.Categories.Find(SelectedCategory.CategoryId);
                        if (category != null)
                        {
                            _context.Categories.Remove(category);
                            _context.SaveChanges();
                            LoadCategories();
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Нельзя удалить категорию, так как она используется в товарах",
                                      "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }, _ => IsCategorySelected);

            SaveCommand = new RelayCommand(_ => {
                try
                {
                    if (string.IsNullOrWhiteSpace(CurrentCategory.Name))
                    {
                        MessageBox.Show("Название категории обязательно", "Ошибка",
                                      MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (CurrentCategory.CategoryId == 0)
                    {
                        _context.Categories.Add(CurrentCategory);
                    }
                    else
                    {
                        var existing = _context.Categories.Find(CurrentCategory.CategoryId);
                        if (existing != null)
                        {
                            existing.Name = CurrentCategory.Name;
                            existing.Description = CurrentCategory.Description;
                        }
                    }
                    _context.SaveChanges();
                    IsEditing = false;
                    LoadCategories();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });

            CancelCommand = new RelayCommand(_ => {
                IsEditing = false;
            });
        }

        private void LoadCategories()
        {
            _context.Categories.Load();
            Categories = _context.Categories.Local;
        }
    }
}