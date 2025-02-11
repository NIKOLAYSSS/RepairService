using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using RepairService.MODELS;

namespace RepairService.DAL
{
    public class DatabaseHelper : IRequestService
    {
        private readonly string _connectionString;

        public DatabaseHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddRequest(Request request)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                // Получаем или создаём запись в таблице Equipment
                Guid equipmentId = GetOrCreateEquipment(request.Equipment, connection);

                // Получаем или создаём запись в таблице FaultTypes
                int faultTypeId = GetOrCreateFaultType(request.FaultType, connection);

                // Получаем или создаём запись в таблице Clients
                Guid clientId = GetOrCreateClient(request.ClientName, connection);

                // Получаем или создаём запись в таблице RequestStatus
                int statusId = GetOrCreateRequestStatus(request.Status, connection);

                // Если указан ответственный, получаем или создаём запись в таблице Users
                int? responsibleId = null;
                if (!string.IsNullOrEmpty(request.Responsible))
                {
                    responsibleId = GetOrCreateUser(request.Responsible, connection);
                }

                // Вставка новой заявки в таблицу Requests с использованием полученных ID
                string query = @"
INSERT INTO Requests (RequestId, RequestDate, EquipmentId, FaultTypeId, Description, ClientId, StatusId, ResponsibleId)
VALUES (@Id, @RequestDate, @EquipmentId, @FaultTypeId, @Description, @ClientId, @StatusId, @ResponsibleId)";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("Id", request.RequestId);
                    command.Parameters.AddWithValue("RequestDate", request.RequestDate);
                    command.Parameters.AddWithValue("EquipmentId", equipmentId);
                    command.Parameters.AddWithValue("FaultTypeId", faultTypeId);
                    command.Parameters.AddWithValue("Description", request.Description ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("ClientId", clientId);
                    command.Parameters.AddWithValue("StatusId", statusId);
                    command.Parameters.AddWithValue("ResponsibleId", responsibleId.HasValue ? (object)responsibleId.Value : DBNull.Value);

                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Получает заявку с выполнением JOIN-ов для получения строковых представлений из связанных таблиц.
        /// Остальные методы (GetRequests, UpdateRequest, DeleteRequest, GetRequestById, SearchRequests)
        /// можно оставить без изменений (см. предыдущий вариант).
        /// </summary>
        public List<Request> GetRequests()
        {
            var requests = new List<Request>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
SELECT 
    r.RequestId, 
    r.RequestDate, 
    e.EquipmentName AS Equipment, 
    ft.FaultTypeName AS FaultType, 
    r.Description, 
    c.ClientName, 
    rs.StatusName AS Status, 
    u.Username AS Responsible
FROM Requests r
JOIN Equipment e ON r.EquipmentId = e.EquipmentId
JOIN FaultTypes ft ON r.FaultTypeId = ft.FaultTypeId
JOIN Clients c ON r.ClientId = c.ClientId
JOIN RequestStatus rs ON r.StatusId = rs.StatusId
LEFT JOIN Users u ON r.ResponsibleId =  u.id;
";

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
                                Responsible = reader["Responsible"] == DBNull.Value ? null : reader["Responsible"].ToString()
                            });
                        }
                    }
                }
            }

            return requests;
        }

        public void UpdateRequest(Request updatedRequest)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
UPDATE Requests
SET 
    Description = @Description,
    StatusId = (SELECT StatusId FROM RequestStatus WHERE StatusName = @Status),
    ResponsibleId = CASE WHEN @Responsible IS NULL THEN NULL ELSE (SELECT Id FROM Users WHERE Username = @Responsible) END
WHERE RequestId = @Id;
";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("Description", updatedRequest.Description);
                    command.Parameters.AddWithValue("Status", updatedRequest.Status);
                    command.Parameters.AddWithValue("Responsible", (object)updatedRequest.Responsible ?? DBNull.Value);
                    command.Parameters.AddWithValue("Id", updatedRequest.RequestId);

                    command.ExecuteNonQuery();
                }
            }
        }

        public bool DeleteRequest(Guid requestId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                string query = "DELETE FROM Requests WHERE RequestId = @requestid";
                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("requestid", requestId);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        public Request GetRequestById(Guid requestId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
SELECT 
    r.RequestId, 
    r.Description, 
    rs.StatusName AS Status, 
    u.Username AS Responsible
FROM Requests r
JOIN RequestStatus rs ON r.StatusId = rs.StatusId
LEFT JOIN Users u ON r.ResponsibleId =  u.id
WHERE r.RequestId = @Id;
";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("Id", requestId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Request
                            {
                                RequestId = (Guid)reader["RequestId"],
                                Description = reader["Description"].ToString(),
                                Status = reader["Status"].ToString(),
                                Responsible = reader["Responsible"] == DBNull.Value ? null : reader["Responsible"].ToString()
                            };
                        }
                    }
                }
            }

            return null;
        }

        public List<Request> SearchRequests(string searchTerm)
        {
            var requests = new List<Request>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
SELECT 
    r.RequestId, 
    r.RequestDate, 
    e.EquipmentName AS Equipment, 
    ft.FaultTypeName AS FaultType, 
    r.Description, 
    c.ClientName, 
    rs.StatusName AS Status, 
    u.Username AS Responsible
FROM Requests r
JOIN Equipment e ON r.EquipmentId = e.EquipmentId
JOIN FaultTypes ft ON r.FaultTypeId = ft.FaultTypeId
JOIN Clients c ON r.ClientId = c.ClientId
JOIN RequestStatus rs ON r.StatusId = rs.StatusId
LEFT JOIN Users u ON r.ResponsibleId =  u.id
WHERE e.EquipmentName ILIKE @searchTerm OR ft.FaultTypeName ILIKE @searchTerm;
";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("searchTerm", "%" + searchTerm + "%");
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
                                Responsible = reader["Responsible"] == DBNull.Value ? null : reader["Responsible"].ToString()
                            });
                        }
                    }
                }
            }
            return requests;
        }

        #region Helper-методы для работы со справочными таблицами

        /// <summary>
        /// Получает идентификатор оборудования по его имени. Если запись не найдена – создаёт новую.
        /// </summary>
        private Guid GetOrCreateEquipment(string equipmentName, NpgsqlConnection connection)
        {
            // Пытаемся получить существующую запись
            string query = "SELECT EquipmentId FROM Equipment WHERE EquipmentName = @equipmentName";
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("equipmentName", equipmentName);
                var result = command.ExecuteScalar();
                if (result != null)
                    return (Guid)result;
            }
            // Если запись не найдена – создаём новую
            string insertQuery = "INSERT INTO Equipment (EquipmentName) VALUES (@equipmentName) RETURNING EquipmentId";
            using (var command = new NpgsqlCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("equipmentName", equipmentName);
                return (Guid)command.ExecuteScalar();
            }
        }

        /// <summary>
        /// Получает идентификатор типа неисправности по его названию. Если запись отсутствует – создаёт новую.
        /// </summary>
        private int GetOrCreateFaultType(string faultTypeName, NpgsqlConnection connection)
        {
            string query = "SELECT FaultTypeId FROM FaultTypes WHERE FaultTypeName = @faultTypeName";
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("faultTypeName", faultTypeName);
                var result = command.ExecuteScalar();
                if (result != null)
                    return Convert.ToInt32(result);
            }
            string insertQuery = "INSERT INTO FaultTypes (FaultTypeName) VALUES (@faultTypeName) RETURNING FaultTypeId";
            using (var command = new NpgsqlCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("faultTypeName", faultTypeName);
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        /// <summary>
        /// Получает идентификатор клиента по его имени. Если записи нет – создаёт новую.
        /// </summary>
        private Guid GetOrCreateClient(string clientName, NpgsqlConnection connection)
        {
            string query = "SELECT ClientId FROM Clients WHERE ClientName = @clientName";
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("clientName", clientName);
                var result = command.ExecuteScalar();
                if (result != null)
                    return (Guid)result;
            }
            string insertQuery = "INSERT INTO Clients (ClientName) VALUES (@clientName) RETURNING ClientId";
            using (var command = new NpgsqlCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("clientName", clientName);
                return (Guid)command.ExecuteScalar();
            }
        }

        /// <summary>
        /// Получает идентификатор статуса заявки по его названию. Если статус отсутствует – создаёт его.
        /// </summary>
        private int GetOrCreateRequestStatus(string statusName, NpgsqlConnection connection)
        {
            string query = "SELECT StatusId FROM RequestStatus WHERE StatusName = @statusName";
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("statusName", statusName);
                var result = command.ExecuteScalar();
                if (result != null)
                    return Convert.ToInt32(result);
            }
            string insertQuery = "INSERT INTO RequestStatus (StatusName) VALUES (@statusName) RETURNING StatusId";
            using (var command = new NpgsqlCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("statusName", statusName);
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        /// <summary>
        /// Получает идентификатор пользователя по имени. Если пользователь отсутствует – создаёт нового
        /// с дефолтным passwordHash и ролью "User".
        /// </summary>
        private int GetOrCreateUser(string username, NpgsqlConnection connection)
        {
            string query = "SELECT Id FROM Users WHERE Username = @username";
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("username", username);
                var result = command.ExecuteScalar();
                if (result != null)
                    return Convert.ToInt32(result);
            }
            string defaultPasswordHash = "default_hash";  // В реальном приложении следует генерировать корректный hash
            string insertQuery = "INSERT INTO Users (Username, Password_Hash, Role) VALUES (@username, @passwordHash, @role) RETURNING Id";
            using (var command = new NpgsqlCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("username", username);
                command.Parameters.AddWithValue("passwordHash", defaultPasswordHash);
                command.Parameters.AddWithValue("role", "User");
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }
    }

        public class UserRepository : IUserService
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
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
                    return result?.ToString();
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
                    return result?.ToString();
                }
            }
        }
    }
}
#endregion