using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VehicleMonitoring
{
    public static class DatabaseHelper
    {
        private const string ConnectionString = "Data Source=DESKTOP-8604I9P\\SQLEXPRESS;Initial Catalog=Vehicles;Integrated Security=True";

        public static bool ValidateUser(string login, string password)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Users WHERE login = @login AND password = @password";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@login", login);
                    command.Parameters.AddWithValue("@password", password);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        return reader.HasRows;
                    }
                }
            }
        }

        public static List<Truck> GetAllTrucks()
        {
            List<Truck> trucks = new List<Truck>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Database connection opened successfully.");

                    string query = "SELECT t.id, t.name, m.description AS model, c.description AS capacity, f.description AS fuel, s.description AS status " +
                                   "FROM DumpTrucks t " +
                                   "INNER JOIN Models m ON t.model_id = m.model_id " +
                                   "INNER JOIN Capacity c ON t.capacity_id = c.capacity_id " +
                                   "INNER JOIN FuelCapacity f ON t.fuel_id = f.fuel_id " +
                                   "INNER JOIN Statuses s ON t.status_id = s.status_id";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var truck = new Truck
                                {
                                    TruckID = (int)reader["id"],
                                    Name = reader["name"].ToString(),
                                    Model = reader["model"].ToString(),
                                    Capacity = reader["capacity"].ToString(),
                                    FuelCapacity = reader["fuel"].ToString(),
                                    Status = reader["status"].ToString()
                                };

                                trucks.Add(truck);
                                Console.WriteLine($"Loaded Truck: {truck.Name}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Database connection failed: {ex.Message}");
                }
            }

            return trucks;
        }

        public static Truck GetTruckById(int truckId)
        {
            Truck truck = null;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT t.id, t.name, m.description AS model, c.description AS capacity, f.description AS fuel, s.description AS status " +
                               "FROM DumpTrucks t " +
                               "INNER JOIN Models m ON t.model_id = m.model_id " +
                               "INNER JOIN Capacity c ON t.capacity_id = c.capacity_id " +
                               "INNER JOIN FuelCapacity f ON t.fuel_id = f.fuel_id " +
                               "INNER JOIN Statuses s ON t.status_id = s.status_id " +
                               "WHERE t.id = @id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", truckId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            truck = new Truck
                            {
                                TruckID = (int)reader["id"],
                                Name = reader["name"].ToString(),
                                Model = reader["model"].ToString(),
                                Capacity = reader["capacity"].ToString(),
                                FuelCapacity = reader["fuel"].ToString(),
                                Status = reader["status"].ToString()
                            };

                        }
                    }
                }
            }

            return truck;
        }
        public static bool AddTruck(Truck truck)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    // Проверяем, существует ли уже самосвал с такими параметрами
                    string checkQuery = "SELECT COUNT(*) FROM DumpTrucks WHERE name = @name AND model_id = (SELECT model_id FROM Models WHERE description = @model)";

                    using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@name", truck.Name);
                        checkCommand.Parameters.AddWithValue("@model", truck.Model);

                        int existingCount = (int)checkCommand.ExecuteScalar();

                        if (existingCount > 0)
                        {
                            MessageBox.Show("Самосвал с таким именем и моделью уже существует в базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return false;
                        }
                    }

                    // Если такого самосвала еще нет, то добавляем новую запись
                    string insertQuery = @"
                INSERT INTO DumpTrucks (name, model_id, capacity_id, fuel_id, status_id)
                VALUES (@name, (SELECT model_id FROM Models WHERE description = @model), 
                        (SELECT capacity_id FROM Capacity WHERE description = @capacity), 
                        (SELECT fuel_id FROM FuelCapacity WHERE description = @fuel), 
                        (SELECT status_id FROM Statuses WHERE description = @status));
            ";

                    using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@name", truck.Name);
                        insertCommand.Parameters.AddWithValue("@model", truck.Model);
                        insertCommand.Parameters.AddWithValue("@capacity", truck.Capacity);
                        insertCommand.Parameters.AddWithValue("@fuel", truck.FuelCapacity);
                        insertCommand.Parameters.AddWithValue("@status", truck.Status);

                        int result = insertCommand.ExecuteNonQuery();
                        return result > 0;
                    }
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine($"SQL Error: {sqlEx.Message}");
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return false;
                }
            }
        }

    }
}