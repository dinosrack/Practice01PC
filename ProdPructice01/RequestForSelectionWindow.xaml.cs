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
    public partial class RequestForSelectionWindow : Window
    {
        public RequestForSelectionWindow()
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
                Table.ItemsSource = _db.NewestAmdRyzen7Models.ToList();

                Table2.ItemsSource = _db.HighestFrequencyInCalifornia.ToList();

                Table3.ItemsSource = _db.TopManufacturerByRevenues.ToList();

                Table4.ItemsSource = _db.CitiesWithAmdRyzen7s.ToList();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.ShowDialog();
        }

        private void Req1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Определить фирму, которая представляет самую новую модель на базе процессора «AMD Ryzen 7».", "Задание", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Req2_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Выбрать модель с наибольшей тактовой частотой, которая выпускается в «Калифорния, США».", "Задание", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Req3_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Определить фирму, которая представляет на рынке товары на наибольшую сумму.", "Задание", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Req4_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Выбрать города, в которых выпускаются ПЭВМ на базе процессора «AMD Ryzen 7».", "Задание", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
