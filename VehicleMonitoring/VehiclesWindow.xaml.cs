using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace VehicleMonitoring
{
    /// <summary>
    /// Логика взаимодействия для VehiclesWindow.xaml
    /// </summary>
    public partial class VehiclesWindow : Window
    {
        public VehiclesWindow()
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            LoadTrucks();
        }
        private void LoadTrucks()
        {
            List<Truck> trucks = DatabaseHelper.GetAllTrucks();

            if (trucks != null && trucks.Count > 0)
            {
                TrucksComboBox.ItemsSource = trucks;
                TrucksComboBox.DisplayMemberPath = "Name";
                TrucksComboBox.SelectedValuePath = "TruckID";

                // Отладочная информация
                Console.WriteLine("Trucks loaded into ComboBox.");
                foreach (var truck in trucks)
                {
                    Console.WriteLine($"Truck: {truck.Name}");
                }
            }
            else
            {
                Console.WriteLine("No trucks loaded.");
            }
        }

        private void TrucksComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Truck selectedTruck = (Truck)TrucksComboBox.SelectedItem;
            if (selectedTruck != null)
            {
                TruckDetails.Text = $"Название: {selectedTruck.Name}\n" +
                                    $"Номер: {selectedTruck.TruckID}\n" +
                                    $"Модель: {selectedTruck.Model}\n" +
                                    $"Грузоподъемность: {selectedTruck.Capacity}\n" +
                                    $"Емкость бака: {selectedTruck.FuelCapacity}\n" +
                                    $"Статус: {selectedTruck.Status}";

                // Отладочная информация
                Console.WriteLine($"Selected Truck: {selectedTruck.Name}");
            }
        }

        private void AddTruckButton_Click(object sender, RoutedEventArgs e)
        {
            AddTruckWindow addTruckWindow = new AddTruckWindow();
            addTruckWindow.Show();
            Close();
        }

        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            AddUserWindow addUserWindow = new AddUserWindow();
            addUserWindow.Show();
            Close();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void Map_Click(object sender, RoutedEventArgs e)
        {
            TrackingWindow trackingWindow = new TrackingWindow();
            trackingWindow.Show();
            Close();
        }
    }
}
