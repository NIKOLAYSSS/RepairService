using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace RepairService
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper(string connectionString)
        {
            _connectionString = connectionString;
        }
        public bool DeleteRequest(Guid requestId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                string query = "DELETE FROM requests WHERE requestid = @requestid";
                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@requestid", requestId);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
        public void AddRequest(Request request)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"
            INSERT INTO Requests (requestid, RequestDate, Equipment, FaultType, Description, ClientName, Status, Responsible)
            VALUES (@Id, @RequestDate, @Equipment, @FaultType, @Description, @ClientName, @Status, @Responsible)";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("Id", request.RequestId);
                    command.Parameters.AddWithValue("RequestDate", request.RequestDate);
                    command.Parameters.AddWithValue("Equipment", request.Equipment);
                    command.Parameters.AddWithValue("FaultType", request.FaultType);
                    command.Parameters.AddWithValue("Description", request.Description ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("ClientName", request.ClientName);
                    command.Parameters.AddWithValue("Status", request.Status);
                    command.Parameters.AddWithValue("Responsible", request.Responsible);

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Request> GetRequests()
        {
            var requests = new List<Request>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Requests;";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            requests.Add(new Request
                            {
                                RequestId = (Guid)reader["RequestId"],
                                RequestDate = Convert.ToDateTime(reader["RequestDate"]),
                                Equipment = reader["Equipment"].ToString(),
                                FaultType = reader["FaultType"].ToString(),
                                Description = reader["Description"]?.ToString(),
                                ClientName = reader["ClientName"].ToString(),
                                Status = reader["Status"].ToString(),
                                Responsible = reader["Responsible"].ToString()
                            });
                        }
                    }
                }
            }

            return requests;
        }
        public void UpdateRequest(Request updatedRequest)
        {
            const string query = @"
        UPDATE Requests
        SET Description = @Description,
            Status = @Status,
            Responsible = @Responsible
        WHERE RequestId = @Id";

            using (var connection = new NpgsqlConnection(_connectionString))
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Description", updatedRequest.Description);
                command.Parameters.AddWithValue("@Status", updatedRequest.Status);
                command.Parameters.AddWithValue("@Responsible", updatedRequest.Responsible ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Id", updatedRequest.RequestId);
                //command.Parameters.AddWithValue("@RequestDate", updatedRequest.RequestDate);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public List<Request> SearchRequests(string searchTerm)
        {
            List<Request> requests = new List<Request>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                var command = new NpgsqlCommand("SELECT RequestId, equipment, FaultType, description, ClientName, status,responsible, RequestDate FROM requests WHERE equipment ILIKE @searchTerm OR FaultType ILIKE @searchTerm", connection);
                command.Parameters.AddWithValue("searchTerm", "%" + searchTerm + "%");

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        requests.Add(new Request
                        {
                            RequestId = Guid.Parse(reader["RequestId"].ToString()),
                            RequestDate = Convert.ToDateTime(reader["RequestDate"]),
                            Equipment = reader["equipment"].ToString(),
                            FaultType = reader["FaultType"].ToString(),
                            Description = reader["description"].ToString(),
                            ClientName = reader["ClientName"].ToString(),
                            Status = reader["status"].ToString(),
                            Responsible = reader["responsible"].ToString()
                        });
                    }
                }
            }
            return requests;
        }
        public Request GetRequestById(Guid requestId)
        {
            const string query = @"
        SELECT RequestId, Description, Status, Responsible 
        FROM Requests
        WHERE RequestId = @Id";

            using (var connection = new NpgsqlConnection(_connectionString))
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", requestId);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Request
                        {
                            RequestId = reader.GetGuid(0),
                            Description = reader.GetString(1),
                            Status = reader.GetString(2),
                            Responsible = reader.IsDBNull(3) ? null : reader.GetString(3)
                        };
                    }
                }
            }

            // Если заявка с указанным ID не найдена, возвращаем null
            return null;
        }
        public void RegisterUser(string username, string passwordHash)
        {
            string query = "INSERT INTO users (username, password_hash) VALUES (@username, @passwordHash)";
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("username", username);
                    command.Parameters.AddWithValue("passwordHash", passwordHash);
                    command.Parameters.AddWithValue("role", "User");
                    command.ExecuteNonQuery();
                }
            }
        }

        // Метод для получения хэша пароля по имени пользователя
        public string GetPasswordHash(string username)
        {
            string query = "SELECT password_hash FROM users WHERE username = @username";
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("username", username);
                    var result = command.ExecuteScalar();
                    return result?.ToString(); // Возвращаем строку или null, если пользователь не найден
                }
            }
        }

        public string GetUserRole(string username)
        {
            string query = "SELECT role FROM users WHERE username = @username";
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("username", username);
                    var result = command.ExecuteScalar();
                    return result?.ToString(); // Возвращаем роль или null, если пользователь не найден
                }
            }
        }
    }
}
