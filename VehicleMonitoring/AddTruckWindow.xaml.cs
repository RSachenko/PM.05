using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace VehicleMonitoring
{
    /// <summary>
    /// Логика взаимодействия для AddTruckWindow.xaml
    /// </summary>
    public partial class AddTruckWindow : Window
    {
        private string ConnectionString = "Data Source=DESKTOP-8604I9P\\SQLEXPRESS;Initial Catalog=Vehicles;Integrated Security=True";

        public AddTruckWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) { }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e) { }

        private void TextBox_TextChanged_2(object sender, TextChangedEventArgs e) { }

        private void txtFuel_TextChanged(object sender, TextChangedEventArgs e) { }

        private void txtParam_TextChanged(object sender, TextChangedEventArgs e) { }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string name = txtName.Text;
            string model = txtModel.Text;
            int capacity = int.Parse(txtCapacity.Text);
            int fuelCapacity = int.Parse(txtFuel.Text);
            string status = txtStatus.Text;

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Add Model if not exists
                    string selectModelQuery = "SELECT model_id FROM Models WHERE description = @Description";
                    SqlCommand selectModelCmd = new SqlCommand(selectModelQuery, connection);
                    selectModelCmd.Parameters.AddWithValue("@Description", model);

                    int? modelId = selectModelCmd.ExecuteScalar() as int?;

                    if (!modelId.HasValue)
                    {
                        string insertModelQuery = "INSERT INTO Models (description) OUTPUT INSERTED.model_id VALUES (@Description)";
                        SqlCommand insertModelCmd = new SqlCommand(insertModelQuery, connection);
                        insertModelCmd.Parameters.AddWithValue("@Description", model);
                        modelId = (int)insertModelCmd.ExecuteScalar();
                    }

                    // Add Capacity if not exists
                    string selectCapacityQuery = "SELECT capacity_id FROM Capacity WHERE description = @Description";
                    SqlCommand selectCapacityCmd = new SqlCommand(selectCapacityQuery, connection);
                    selectCapacityCmd.Parameters.AddWithValue("@Description", capacity.ToString());

                    int? capacityId = selectCapacityCmd.ExecuteScalar() as int?;

                    if (!capacityId.HasValue)
                    {
                        string insertCapacityQuery = "INSERT INTO Capacity (description) OUTPUT INSERTED.capacity_id VALUES (@Description)";
                        SqlCommand insertCapacityCmd = new SqlCommand(insertCapacityQuery, connection);
                        insertCapacityCmd.Parameters.AddWithValue("@Description", capacity.ToString());
                        capacityId = (int)insertCapacityCmd.ExecuteScalar();
                    }

                    // Add FuelCapacity if not exists
                    string selectFuelCapacityQuery = "SELECT fuel_id FROM FuelCapacity WHERE description = @Description";
                    SqlCommand selectFuelCapacityCmd = new SqlCommand(selectFuelCapacityQuery, connection);
                    selectFuelCapacityCmd.Parameters.AddWithValue("@Description", fuelCapacity.ToString());

                    int? fuelCapacityId = selectFuelCapacityCmd.ExecuteScalar() as int?;

                    if (!fuelCapacityId.HasValue)
                    {
                        string insertFuelCapacityQuery = "INSERT INTO FuelCapacity (description) OUTPUT INSERTED.fuel_id VALUES (@Description)";
                        SqlCommand insertFuelCapacityCmd = new SqlCommand(insertFuelCapacityQuery, connection);
                        insertFuelCapacityCmd.Parameters.AddWithValue("@Description", fuelCapacity.ToString());
                        fuelCapacityId = (int)insertFuelCapacityCmd.ExecuteScalar();
                    }

                    // Add Status if not exists
                    string selectStatusQuery = "SELECT status_id FROM Statuses WHERE description = @Description";
                    SqlCommand selectStatusCmd = new SqlCommand(selectStatusQuery, connection);
                    selectStatusCmd.Parameters.AddWithValue("@Description", status);

                    int? statusId = selectStatusCmd.ExecuteScalar() as int?;

                    if (!statusId.HasValue)
                    {
                        string insertStatusQuery = "INSERT INTO Statuses (description) OUTPUT INSERTED.status_id VALUES (@Description)";
                        SqlCommand insertStatusCmd = new SqlCommand(insertStatusQuery, connection);
                        insertStatusCmd.Parameters.AddWithValue("@Description", status);
                        statusId = (int)insertStatusCmd.ExecuteScalar();
                    }

                    // Insert new DumpTruck
                    string insertTruckQuery = "INSERT INTO DumpTrucks (name, model_id, capacity_id, fuel_id, status_id) VALUES (@Name, @ModelId, @CapacityId, @FuelId, @StatusId)";
                    SqlCommand insertTruckCmd = new SqlCommand(insertTruckQuery, connection);
                    insertTruckCmd.Parameters.AddWithValue("@Name", name);
                    insertTruckCmd.Parameters.AddWithValue("@ModelId", modelId.Value);
                    insertTruckCmd.Parameters.AddWithValue("@CapacityId", capacityId.Value);
                    insertTruckCmd.Parameters.AddWithValue("@FuelId", fuelCapacityId.Value);
                    insertTruckCmd.Parameters.AddWithValue("@StatusId", statusId.Value);

                    insertTruckCmd.ExecuteNonQuery();

                    MessageBox.Show("Самосвал успешно добавлен.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            VehiclesWindow vehiclesWindow = new VehiclesWindow();
            vehiclesWindow.Show();
            Close();
        }
    }
}
