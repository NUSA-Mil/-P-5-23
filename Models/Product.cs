using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShopApp.Models
{
    public class Product : INotifyPropertyChanged
    {
        private int _productId;
        private string _name;
        private decimal _price;
        private int _quantity;
        private int _categoryId;
        private Category _category;

        public int ProductId
        {
            get => _productId;
            set
            {
                _productId = value;
                OnPropertyChanged(nameof(ProductId));
            }
        }

        [Required(ErrorMessage = "Название товара обязательно")]
        [StringLength(100, ErrorMessage = "Название не должно превышать 100 символов")]
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        [Range(0.01, double.MaxValue, ErrorMessage = "Цена должна быть больше 0")]
        public decimal Price
        {
            get => _price;
            set
            {
                if (value <= 0)
                    throw new ValidationException("Цена должна быть больше 0");
                _price = value;
                OnPropertyChanged(nameof(Price));
            }
        }

        [Range(1, int.MaxValue, ErrorMessage = "Количество должно быть больше 0")]
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (value <= 0)
                    throw new ValidationException("Количество должно быть больше 0");
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }

        [Required(ErrorMessage = "Необходимо выбрать категорию")]
        public int CategoryId
        {
            get => _categoryId;
            set
            {
                _categoryId = value;
                OnPropertyChanged(nameof(CategoryId));
            }
        }

        public Category Category
        {
            get => _category;
            set
            {
                _category = value;
                OnPropertyChanged(nameof(Category));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}