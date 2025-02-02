using CourseWorkStore.MessageBoxes;
using CourseWorkStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
using System.Windows.Shapes;

namespace CourseWorkStore
{
    public partial class AddOrEdit : Window
    {
        private string _tableName;
        private List<TextBox> _textBoxes = new List<TextBox>();
        private bool _isEditMode = false;
        private int _editId = 0;

        public AddOrEdit(string tableName)
        {
            InitializeComponent();
            _tableName = tableName;
            TableName.Text = tableName;
            CreateFieldsBasedOnTable();
        }

        public AddOrEdit(string tableName, int id)
        {
            InitializeComponent();
            _tableName = tableName;
            _isEditMode = true;
            _editId = id;
            TableName.Text = tableName;
            Header.Text = "Изменение записи";
            AddOrEditBtn.Content = "Изменить";
            AddOrEditBtn.Style = (Style)FindResource("ButtonEdit");
            CreateFieldsBasedOnTable();
            LoadDataForEdit();
        }

        private void LoadDataForEdit()
        {
            using (CourseWorkStoreContext _db = new CourseWorkStoreContext())
            {
                switch (_tableName)
                {
                    case "Товары":
                        var product = _db.Products.FirstOrDefault(p => p.ProductId == _editId);
                        if (product != null)
                        {
                            _textBoxes[0].Text = product.ProductName;
                            _textBoxes[1].Text = product.ProductDescription;
                            _textBoxes[2].Text = product.ProductPrice.ToString();
                            _textBoxes[3].Text = product.ProductQuantity.ToString();
                            _textBoxes[4].Text = product.SupplierId.ToString();
                        }
                        break;
                    case "Заказы":
                        var purchase = _db.Purchases.FirstOrDefault(p => p.PurchaseId == _editId);
                        if (purchase != null)
                        {
                            _textBoxes[0].Text = purchase.ProductId.ToString();
                            _textBoxes[1].Text = purchase.PurchaseQuantity.ToString();
                            _textBoxes[2].Text = purchase.PurchaseAmount.ToString();
                            _textBoxes[3].Text = purchase.PurchaseDate.ToString();
                        }
                        break;
                    case "Поставщики":
                        var supplier = _db.Suppliers.FirstOrDefault(s => s.SupplierId == _editId);
                        if (supplier != null)
                        {
                            _textBoxes[0].Text = supplier.SupplierName;
                            _textBoxes[1].Text = supplier.SupplierPhone;
                            _textBoxes[2].Text = supplier.SupplierEmail;
                            _textBoxes[3].Text = supplier.SupplierAddress;
                        }
                        break;
                    case "Пользователи":
                        var user = _db.Users.FirstOrDefault(u => u.UserId == _editId);
                        if (user != null)
                        {
                            _textBoxes[0].Text = user.Username;
                            _textBoxes[1].Text = user.Pass;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void CreateFieldsBasedOnTable()
        {
            switch (_tableName)
            {
                case "Товары":
                    CreateFields(new List<string> { "Название", "Описание", "Цена", "Количество", "ID поставщика" });
                    break;
                case "Заказы":
                    CreateFields(new List<string> { "ID товара", "Количество", "Цена", "Дата" });
                    break;
                case "Поставщики":
                    CreateFields(new List<string> { "Имя", "Телефон", "Почта", "Адрес" });
                    break;
                case "Пользователи":
                    CreateFields(new List<string> { "Логин", "Пароль" });
                    break;
                default:
                    break;
            }
        }

        private void CreateFields(List<string> fieldNames)
        {
            foreach (var fieldName in fieldNames)
            {
                var textBlock = new TextBlock
                {
                    Text = fieldName, 
                    FontSize = 16,
                    Foreground = Brushes.White
                };

                MainArea.Children.Add(textBlock);

                var borderAfterTextBlock = new Border
                {
                    Margin = new Thickness(2)
                };
                MainArea.Children.Add(borderAfterTextBlock);

                var textBox = new TextBox();
                MainArea.Children.Add(textBox);

                var borderAfterTextBox = new Border
                {
                    Margin = new Thickness(10)
                };
                MainArea.Children.Add(borderAfterTextBox);

                _textBoxes.Add(textBox);
            }
        }

        private void AddOrEditBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_textBoxes.Any(tb => string.IsNullOrWhiteSpace(tb.Text)))
            {
                FieldsIsEmpty fieldsIsEmpty = new FieldsIsEmpty();
                fieldsIsEmpty.Owner = this;
                fieldsIsEmpty.ShowDialog();
                return;
            }

            using (CourseWorkStoreContext _db = new CourseWorkStoreContext())
            {
                if (_isEditMode)
                {
                    switch (_tableName)
                    {
                        case "Товары":
                            EditProduct(_db);
                            break;
                        case "Заказы":
                            EditPurchase(_db);
                            break;
                        case "Поставщики":
                            EditSupplier(_db);
                            break;
                        case "Пользователи":
                            EditUser(_db);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (_tableName)
                    {
                        case "Товары":
                            AddProduct(_db);
                            break;
                        case "Заказы":
                            AddPurchase(_db);
                            break;
                        case "Поставщики":
                            AddSupplier(_db);
                            break;
                        case "Пользователи":
                            AddUser(_db);
                            break;
                        default:
                            break;
                    }
                }

                _db.SaveChanges();
            }

            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.ShowDialog();
        }

        private void AddProduct(CourseWorkStoreContext _db)
        {
            var product = new Product
            {
                ProductName = _textBoxes[0].Text,
                ProductDescription = _textBoxes[1].Text,
                ProductPrice = decimal.Parse(_textBoxes[2].Text),
                ProductQuantity = int.Parse(_textBoxes[3].Text),
                SupplierId = int.Parse(_textBoxes[4].Text)
            };

            _db.Products.Add(product);
        }

        private void AddPurchase(CourseWorkStoreContext _db)
        {
            var purchase = new Purchase
            {
                ProductId = int.Parse(_textBoxes[0].Text),
                PurchaseQuantity = int.Parse(_textBoxes[1].Text),
                PurchaseAmount = decimal.Parse(_textBoxes[2].Text),
                PurchaseDate = DateOnly.Parse(_textBoxes[3].Text)
            };

            _db.Purchases.Add(purchase);
        }

        private void AddSupplier(CourseWorkStoreContext _db)
        {
            var supplier = new Supplier
            {
                SupplierName = _textBoxes[0].Text,
                SupplierPhone = _textBoxes[1].Text,
                SupplierEmail = _textBoxes[2].Text,
                SupplierAddress = _textBoxes[3].Text
            };

            _db.Suppliers.Add(supplier);
        }

        private void AddUser(CourseWorkStoreContext _db)
        {
            var user = new User
            {
                Username = _textBoxes[0].Text,
                Pass = _textBoxes[1].Text
            };

            _db.Users.Add(user);
        }

        private void EditProduct(CourseWorkStoreContext _db)
        {
            var product = _db.Products.FirstOrDefault(p => p.ProductId == _editId);
            if (product != null)
            {
                product.ProductName = _textBoxes[0].Text;
                product.ProductDescription = _textBoxes[1].Text;
                product.ProductPrice = decimal.Parse(_textBoxes[2].Text);
                product.ProductQuantity = int.Parse(_textBoxes[3].Text);
                product.SupplierId = int.Parse(_textBoxes[4].Text);
            }
        }

        private void EditPurchase(CourseWorkStoreContext _db)
        {
            var purchase = _db.Purchases.FirstOrDefault(p => p.PurchaseId == _editId);
            if (purchase != null)
            {
                purchase.ProductId = int.Parse(_textBoxes[0].Text);
                purchase.PurchaseQuantity = int.Parse(_textBoxes[1].Text);
                purchase.PurchaseAmount = decimal.Parse(_textBoxes[2].Text);
                purchase.PurchaseDate = DateOnly.Parse(_textBoxes[3].Text);
            }
        }

        private void EditSupplier(CourseWorkStoreContext _db)
        {
            var supplier = _db.Suppliers.FirstOrDefault(s => s.SupplierId == _editId);
            if (supplier != null)
            {
                supplier.SupplierName = _textBoxes[0].Text;
                supplier.SupplierPhone = _textBoxes[1].Text;
                supplier.SupplierEmail = _textBoxes[2].Text;
                supplier.SupplierAddress = _textBoxes[3].Text;
            }
        }

        private void EditUser(CourseWorkStoreContext _db)
        {
            var user = _db.Users.FirstOrDefault(u => u.UserId == _editId);
            if (user != null)
            {
                user.Username = _textBoxes[0].Text;
                user.Pass = _textBoxes[1].Text;
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.ShowDialog();
        }
    }
}
