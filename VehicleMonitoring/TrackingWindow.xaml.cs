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
using System.Windows.Threading;

namespace VehicleMonitoring
{
    /// <summary>
    /// Логика взаимодействия для TrackingWindow.xaml
    /// </summary>
    public partial class TrackingWindow : Window
    {
        private DispatcherTimer timer;
        private Random random;

        public TrackingWindow()
        {
            InitializeComponent();
            random = new Random();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += Timer_Tick;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            MoveVehicle(Truck1);
            MoveVehicle(Truck2);
            MoveVehicle(Excavator1);
            MoveVehicle(Excavator2);
        }

        private void MoveVehicle(Ellipse vehicle)
        {
            double maxLeft = MapImage.ActualWidth - vehicle.Width;
            double maxTop = MapImage.ActualHeight - vehicle.Height;

            double newLeft = random.NextDouble() * maxLeft;
            double newTop = random.NextDouble() * maxTop;

            vehicle.SetValue(Canvas.LeftProperty, newLeft);
            vehicle.SetValue(Canvas.TopProperty, newTop);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            VehiclesWindow vehiclesWindow = new VehiclesWindow();
            vehiclesWindow.Show();
            Close();
        }
    }
}
