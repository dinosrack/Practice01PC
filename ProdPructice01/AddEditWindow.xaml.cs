using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProdPructice01.Models;
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

namespace ProdPructice01
{
    public partial class AddEditWindow : Window
    {
        private string _tableName;
        private List<TextBox> _textBoxes = new List<TextBox>();
        private bool _isEditMode = false;
        private int _editId = 0;

        public AddEditWindow(string tableName)
        {
            InitializeComponent();
            _tableName = tableName;
            TableName.Text = tableName;
            CreateFieldsBasedOnTable();
        }

        public AddEditWindow(string tableName, int id)
        {
            InitializeComponent();
            _tableName = tableName;
            _isEditMode = true;
            _editId = id;
            TableName.Text = tableName;
            Header.Text = "Изменение записи";
            Title = "Изменение записи";
            AddOrEditBtn.Content = "Изменить";
            CreateFieldsBasedOnTable();
            LoadDataForEdit();
        }

        private void LoadDataForEdit()
        {
            using (ProdPracticeContext _db = new ProdPracticeContext())
            {
                switch (_tableName)
                {
                    case "Производители":
                        var manufacturer = _db.Manufacturers.FirstOrDefault(p => p.Id == _editId);
                        if (manufacturer != null)
                        {
                            _textBoxes[0].Text = manufacturer.Name;
                            _textBoxes[1].Text = manufacturer.Location;
                        }
                        break;
                    case "Компьютеры":
                        var computer = _db.Computers.FirstOrDefault(p => p.Id == _editId);
                        if (computer != null)
                        {
                            _textBoxes[0].Text = computer.ManufacturerId.ToString();
                            _textBoxes[1].Text = computer.ModelName;
                            _textBoxes[2].Text = computer.Cputype;
                            _textBoxes[3].Text = computer.Cpufrequency.ToString();
                            _textBoxes[4].Text = computer.Ram.ToString();
                            _textBoxes[5].Text = computer.Hdd.ToString();
                            _textBoxes[6].Text = computer.ReleaseDate.ToString();
                        }
                        break;
                    case "Продавцы":
                        var seller = _db.Sellers.FirstOrDefault(s => s.Id == _editId);
                        if (seller != null)
                        {
                            _textBoxes[0].Text = seller.Name;
                            _textBoxes[1].Text = seller.Address;
                            _textBoxes[2].Text = seller.Phone;
                        }
                        break;
                    case "Рынок":
                        var marketOffer = _db.MarketOffers.FirstOrDefault(u => u.Id == _editId);
                        if (marketOffer != null)
                        {
                            _textBoxes[0].Text = marketOffer.ComputerId.ToString();
                            _textBoxes[1].Text = marketOffer.SellerId.ToString();
                            _textBoxes[2].Text = marketOffer.BatchSize.ToString();
                            _textBoxes[3].Text = marketOffer.BatchPrice.ToString();
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
                case "Производители":
                    CreateFields(new List<string> { "Название", "Место" });
                    break;
                case "Компьютеры":
                    CreateFields(new List<string> { "ID производителя", "Название", "Тип процессора", "Частота процессора", "Оперативная память", "Жесткий диск", "Дата выпуска" });
                    break;
                case "Продавцы":
                    CreateFields(new List<string> { "Название", "Адрес", "Телефон" });
                    break;
                case "Рынок":
                    CreateFields(new List<string> { "ID компьютера", "ID продавца", "Размер партии", "Цена партии" });
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
                    FontSize = 16
                };
                MainArea.Children.Add(textBlock);

                var borderAfterTextBlock = new Border
                {
                    Margin = new Thickness(2)
                };
                MainArea.Children.Add(borderAfterTextBlock);

                var textBox = new TextBox
                {
                    Height = 35,
                    FontSize = 16,
                    VerticalContentAlignment = VerticalAlignment.Center
                };
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
                MessageBox.Show("Все поля необходимо заполнить для добавления новой строки!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (ProdPracticeContext _db = new ProdPracticeContext())
            {
                try
                {
                    if (_isEditMode)
                    {
                        switch (_tableName)
                        {
                            case "Производители":
                                EditManufacturer(_db);
                                break;
                            case "Компьютеры":
                                EditComputer(_db);
                                break;
                            case "Продавцы":
                                EditSeller(_db);
                                break;
                            case "Рынок":
                                EditMarketOffer(_db);
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        switch (_tableName)
                        {
                            case "Производители":
                                AddManufacturer(_db);
                                break;
                            case "Компьютеры":
                                AddComputer(_db);
                                break;
                            case "Продавцы":
                                AddSeller(_db);
                                break;
                            case "Рынок":
                                AddMarketOffer(_db);
                                break;
                            default:
                                break;
                        }
                    }

                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.ShowDialog();
        }

        private void AddManufacturer(ProdPracticeContext _db)
        {
            var manufacturer = new Manufacturer
            {
                Name = _textBoxes[0].Text,
                Location = _textBoxes[1].Text
            };

            _db.Manufacturers.Add(manufacturer);
        }

        private void AddComputer(ProdPracticeContext _db)
        {
            var computer = new Computer
            {
                ManufacturerId = int.Parse(_textBoxes[0].Text),
                ModelName = _textBoxes[1].Text,
                Cputype = _textBoxes[2].Text,
                Cpufrequency = decimal.Parse(_textBoxes[3].Text),
                Ram = int.Parse(_textBoxes[4].Text),
                Hdd = int.Parse(_textBoxes[5].Text),
                ReleaseDate = DateOnly.Parse(_textBoxes[6].Text)
            };

            _db.Computers.Add(computer);
        }

        private void AddSeller(ProdPracticeContext _db)
        {
            var seller = new Seller
            {
                Name = _textBoxes[0].Text,
                Address = _textBoxes[1].Text,
                Phone = _textBoxes[2].Text
            };

            _db.Sellers.Add(seller);
        }

        private void AddMarketOffer(ProdPracticeContext _db)
        {
            var marketOffer = new MarketOffer
            {
                ComputerId = int.Parse(_textBoxes[0].Text),
                SellerId = int.Parse(_textBoxes[1].Text),
                BatchSize = int.Parse(_textBoxes[2].Text),
                BatchPrice = decimal.Parse(_textBoxes[3].Text)
            };

            _db.MarketOffers.Add(marketOffer);
        }

        private void EditManufacturer(ProdPracticeContext _db)
        {
            var manufacturer = _db.Manufacturers.FirstOrDefault(p => p.Id == _editId);
            if (manufacturer != null)
            {
                manufacturer.Name = _textBoxes[0].Text;
                manufacturer.Location = _textBoxes[1].Text;
            }
        }

        private void EditComputer(ProdPracticeContext _db)
        {
            var computer = _db.Computers.FirstOrDefault(p => p.Id == _editId);
            if (computer != null)
            {
                computer.ManufacturerId = int.Parse(_textBoxes[0].Text);
                computer.ModelName = _textBoxes[1].Text;
                computer.Cputype = _textBoxes[2].Text;
                computer.Cpufrequency = decimal.Parse(_textBoxes[3].Text);
                computer.Ram = int.Parse(_textBoxes[4].Text);
                computer.Hdd = int.Parse(_textBoxes[5].Text);
                computer.ReleaseDate = DateOnly.Parse(_textBoxes[6].Text);
            }
        }

        private void EditSeller(ProdPracticeContext _db)
        {
            var seller = _db.Sellers.FirstOrDefault(s => s.Id == _editId);
            if (seller != null)
            {
                seller.Name = _textBoxes[0].Text;
                seller.Address = _textBoxes[1].Text;
                seller.Phone = _textBoxes[2].Text;
            }
        }

        private void EditMarketOffer(ProdPracticeContext _db)
        {
            var marketOffer = _db.MarketOffers.FirstOrDefault(u => u.Id == _editId);
            if (marketOffer != null)
            {
                marketOffer.ComputerId = int.Parse(_textBoxes[0].Text);
                marketOffer.SellerId = int.Parse(_textBoxes[1].Text);
                marketOffer.BatchSize = int.Parse(_textBoxes[2].Text);
                marketOffer.BatchPrice = decimal.Parse(_textBoxes[3].Text);
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
