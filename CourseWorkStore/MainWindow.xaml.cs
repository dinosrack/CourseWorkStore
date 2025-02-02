using CourseWorkStore.MessageBoxes;
using CourseWorkStore.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CourseWorkStore
{
    public partial class MainWindow : Window
    {
        private string _username;

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(string username)
        {
            InitializeComponent();
            _username = username;
            CheckUserAccess();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDBInDataGrid();
        }

        void LoadDBInDataGrid()
        {
            using (CourseWorkStoreContext _db = new CourseWorkStoreContext())
            {
                Table.ItemsSource = _db.Products.ToList();

                Table2.ItemsSource = _db.Purchases.ToList();

                Table3.ItemsSource = _db.Suppliers.ToList();

                Table4.ItemsSource = _db.Users.ToList();
            }
        }
        private void CheckUserAccess()
        {
            if (_username != "Админ")
            {
                var usersTabItem = Tab.Items
                    .OfType<TabItem>()
                    .FirstOrDefault(t => t.Header.ToString() == "Пользователи");

                if (usersTabItem != null)
                {
                    usersTabItem.Visibility = Visibility.Collapsed;
                }

                var addButton = FindName("Add") as Button;
                var editButton = FindName("Edit") as Button;
                var removeButton = FindName("Remove") as Button;
                var borderAdmin = FindName("BorderAdmin") as Border;
                var borderMargin1 = FindName("BorderMargin1") as Border;
                var borderMargin2 = FindName("BorderMargin2") as Border;

                if (addButton != null)
                {
                    addButton.Visibility = Visibility.Collapsed;
                }

                if (editButton != null)
                {
                    editButton.Visibility = Visibility.Collapsed;
                }

                if (removeButton != null)
                {
                    removeButton.Visibility = Visibility.Collapsed;
                }

                if (borderAdmin != null)
                {
                    borderAdmin.Visibility = Visibility.Collapsed;
                }

                if (borderMargin1 != null)
                {
                    borderMargin1.Visibility = Visibility.Collapsed;
                }

                if (borderMargin2 != null)
                {
                    borderMargin2.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            AuthWindow authWindow = new AuthWindow();
            this.Close();
            authWindow.ShowDialog();
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            DataGrid activeDataGrid = null;
            string tableName = "";

            switch (Tab.SelectedIndex)
            {
                case 0:
                    activeDataGrid = Table;
                    tableName = "Товары";
                    break;
                case 1:
                    activeDataGrid = Table2;
                    tableName = "Заказы";
                    break;
                case 2:
                    activeDataGrid = Table3;
                    tableName = "Поставщики";
                    break;
                case 3:
                    activeDataGrid = Table4;
                    tableName = "Пользователи";
                    break;
                default:
                    break;
            }

            if (activeDataGrid != null)
            {
                var selectedItem = activeDataGrid.SelectedItem;
                if (selectedItem != null)
                {
                    DeleteRow deleteRowWindow = new DeleteRow(selectedItem);
                    deleteRowWindow.Owner = this;
                    deleteRowWindow.ShowDialog();

                    if (deleteRowWindow.DialogResult == true)
                    {
                        using (CourseWorkStoreContext _db = new CourseWorkStoreContext())
                        {
                            try
                            {
                                _db.Entry(selectedItem).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;

                                switch (tableName)
                                {
                                    case "Товары":
                                        var productId = ((Product)selectedItem).ProductId;
                                        var relatedPurchases = _db.Purchases.Where(p => p.ProductId == productId).ToList();
                                        _db.Purchases.RemoveRange(relatedPurchases);
                                        break;
                                    case "Заказы":
                                        var purchaseId = ((Purchase)selectedItem).PurchaseId;
                                        break;
                                    case "Поставщики":
                                        var supplierId = ((Supplier)selectedItem).SupplierId;

                                        var relatedProducts = _db.Products.Where(p => p.SupplierId == supplierId).ToList();

                                        foreach (var product in relatedProducts)
                                        {
                                            var relatedPurchases2 = _db.Purchases.Where(p => p.ProductId == product.ProductId).ToList();
                                            _db.Purchases.RemoveRange(relatedPurchases2);
                                        }

                                        _db.Products.RemoveRange(relatedProducts);

                                        _db.Suppliers.Remove((Supplier)selectedItem);

                                        _db.SaveChanges();
                                        break;
                                    default:
                                        break;
                                }

                                _db.SaveChanges();
                                LoadDBInDataGrid();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Ошибка при удалении: {ex.Message}\nВнутреннее исключение: {ex.InnerException?.Message}");
                            }
                        }
                    }
                }
                else
                {
                    RowNotSelected rowNotSelected = new RowNotSelected();
                    rowNotSelected.Owner = this;
                    rowNotSelected.ShowDialog();
                }
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var selectedTab = Tab.SelectedItem as TabItem;

            if (selectedTab != null)
            {
                string tableName = selectedTab.Header.ToString();

                AddOrEdit addOrEditWindow = new AddOrEdit(tableName);
                this.Close();
                addOrEditWindow.ShowDialog();

                LoadDBInDataGrid();
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            DataGrid activeDataGrid = null;
            string tableName = "";

            switch (Tab.SelectedIndex)
            {
                case 0:
                    activeDataGrid = Table;
                    tableName = "Товары";
                    break;
                case 1: 
                    activeDataGrid = Table2;
                    tableName = "Заказы";
                    break;
                case 2: 
                    activeDataGrid = Table3;
                    tableName = "Поставщики";
                    break;
                case 3: 
                    activeDataGrid = Table4;
                    tableName = "Пользователи";
                    break;
                default:
                    break;
            }

            if (activeDataGrid != null)
            {
                var selectedItem = activeDataGrid.SelectedItem;
                if (selectedItem != null)
                {
                    int id = 0;

                    switch (tableName)
                    {
                        case "Товары":
                            id = ((Product)selectedItem).ProductId;
                            break;
                        case "Заказы":
                            id = ((Purchase)selectedItem).PurchaseId;
                            break;
                        case "Поставщики":
                            id = ((Supplier)selectedItem).SupplierId;
                            break;
                        case "Пользователи":
                            id = ((User)selectedItem).UserId;
                            break;
                        default:
                            break;
                    }

                    var editWindow = new AddOrEdit(tableName, id);
                    this.Close();
                    editWindow.ShowDialog();

                    LoadDBInDataGrid();
                }
                else
                {
                    RowNotSelected rowNotSelected = new RowNotSelected();
                    rowNotSelected.Owner = this;
                    rowNotSelected.ShowDialog();
                }
            }
        }

        private void Find_Click(object sender, RoutedEventArgs e)
        {
            DataGrid activeDataGrid = null;

            switch (Tab.SelectedIndex)
            {
                case 0:
                    activeDataGrid = Table;
                    break;
                case 1:
                    activeDataGrid = Table2;
                    break;
                case 2:
                    activeDataGrid = Table3;
                    break;
                case 3:
                    activeDataGrid = Table4;
                    break;
                default:
                    break;
            }

            if (activeDataGrid != null)
            {
                var listItem = activeDataGrid.ItemsSource as IEnumerable<object>;

                if (listItem != null)
                {
                    var filtered = listItem.Cast<object>().Where(item =>
                    {
                        var idProperty = item.GetType().GetProperty("ProductId") ??
                                         item.GetType().GetProperty("PurchaseId") ??
                                         item.GetType().GetProperty("SupplierId") ??
                                         item.GetType().GetProperty("UserId");
                        if (idProperty != null)
                        {
                            var idValue = idProperty.GetValue(item)?.ToString();
                            return idValue == ID.Text;
                        }
                        return false;
                    });

                    if (filtered.Any())
                    {
                        var item = filtered.First();
                        activeDataGrid.SelectedItem = item;
                        activeDataGrid.ScrollIntoView(item);
                        activeDataGrid.Focus();
                    }
                    else
                    {
                        IdNotFound idNotFound = new IdNotFound();
                        idNotFound.Owner = this;
                        idNotFound.ShowDialog();
                    }
                }
            }
        }
    }
}