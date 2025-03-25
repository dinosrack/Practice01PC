using ProdPructice01.Models;
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

namespace ProdPructice01
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDBInDataGrid();
        }

        void LoadDBInDataGrid()
        {
            using (ProdPracticeContext _db = new ProdPracticeContext())
            {
                Table.ItemsSource = _db.Manufacturers.ToList();

                Table2.ItemsSource = _db.Computers.ToList();

                Table3.ItemsSource = _db.Sellers.ToList();

                Table4.ItemsSource = _db.MarketOffers.ToList();
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            DataGrid activeDataGrid = null;
            string tableName = "";

            switch (Tab.SelectedIndex)
            {
                case 0:
                    activeDataGrid = Table;
                    tableName = "Производители";
                    break;
                case 1:
                    activeDataGrid = Table2;
                    tableName = "Компьютеры";
                    break;
                case 2:
                    activeDataGrid = Table3;
                    tableName = "Продавцы";
                    break;
                case 3:
                    activeDataGrid = Table4;
                    tableName = "Рынок";
                    break;
                default:
                    break;
            }

            if (activeDataGrid != null)
            {
                var selectedItem = activeDataGrid.SelectedItem;
                if (selectedItem != null)
                {
                    MessageBoxResult result = MessageBox.Show(
                        "Вы уверены, что хотите удалить выбранную запись?",
                        "Подтверждение удаления",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        using (ProdPracticeContext _db = new ProdPracticeContext())
                        {
                            try
                            {
                                _db.Entry(selectedItem).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;

                                switch (tableName)
                                {
                                    case "Computers": 
                                        var computerId = ((Computer)selectedItem).Id;
                                        var relatedMarketOffers = _db.MarketOffers.Where(mo => mo.ComputerId == computerId).ToList();
                                        _db.MarketOffers.RemoveRange(relatedMarketOffers);
                                        break;

                                    case "MarketOffers": 
                                        var marketOfferId = ((MarketOffer)selectedItem).Id;
                                        break;

                                    case "Manufacturers":
                                        var manufacturerId = ((Manufacturer)selectedItem).Id;

                                        var relatedComputers = _db.Computers.Where(c => c.ManufacturerId == manufacturerId).ToList();

                                        foreach (var computer in relatedComputers)
                                        {
                                            var relatedMarketOffers2 = _db.MarketOffers.Where(mo => mo.ComputerId == computer.Id).ToList();
                                            _db.MarketOffers.RemoveRange(relatedMarketOffers2);
                                        }

                                        _db.Computers.RemoveRange(relatedComputers);
                                        _db.Manufacturers.Remove((Manufacturer)selectedItem);
                                        break;

                                    case "Sellers": 
                                        var sellerId = ((Seller)selectedItem).Id;

                                        var relatedMarketOffers3 = _db.MarketOffers.Where(mo => mo.SellerId == sellerId).ToList();
                                        _db.MarketOffers.RemoveRange(relatedMarketOffers3);

                                        _db.Sellers.Remove((Seller)selectedItem);
                                        break;

                                    default:
                                        break;
                                }
                                _db.SaveChanges();
                                LoadDBInDataGrid();

                                MessageBox.Show("Запись успешно удалена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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
                    MessageBox.Show("Необходимо выбрать строку для выполнения дальнейшего действия!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var selectedTab = Tab.SelectedItem as TabItem;

            if (selectedTab != null)
            {
                string tableName = selectedTab.Header.ToString();

                AddEditWindow addEditWindow = new AddEditWindow(tableName);
                this.Close();
                addEditWindow.ShowDialog();

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
                    tableName = "Производители";
                    break;
                case 1:
                    activeDataGrid = Table2;
                    tableName = "Компьютеры";
                    break;
                case 2:
                    activeDataGrid = Table3;
                    tableName = "Продавцы";
                    break;
                case 3:
                    activeDataGrid = Table4;
                    tableName = "Рынок";
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
                        case "Производители":
                            id = ((Manufacturer)selectedItem).Id;
                            break;
                        case "Компьютеры":
                            id = ((Computer)selectedItem).Id;
                            break;
                        case "Продавцы":
                            id = ((Seller)selectedItem).Id;
                            break;
                        case "Рынок":
                            id = ((MarketOffer)selectedItem).Id;
                            break;
                        default:
                            break;
                    }

                    var editWindow = new AddEditWindow(tableName, id);
                    this.Close();
                    editWindow.ShowDialog();

                    LoadDBInDataGrid();
                }
                else
                {
                    MessageBox.Show("Необходимо выбрать строку для выполнения дальнейшего действия!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
                        var idProperty = item.GetType().GetProperty("Id") ??
                                         item.GetType().GetProperty("Id") ??
                                         item.GetType().GetProperty("Id") ??
                                         item.GetType().GetProperty("Id");
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
                        MessageBox.Show("ID, который вы искали не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void Requests_Click(object sender, RoutedEventArgs e)
        {
            RequestForSelectionWindow requestWindow = new RequestForSelectionWindow();
            this.Close();
            requestWindow.ShowDialog();
        }
    }
}