using Dapper;
using Npgsql;
using System.Data;

namespace Loan.Outbox.Publisher.Service
{
    public static class LoanOutboxSingletonDatabase
    {
        static NpgsqlConnection _connection;
        static bool _dataReaderState = true;
        static LoanOutboxSingletonDatabase()
            => _connection = new NpgsqlConnection("User ID=admin;Password=123;Host=localhost;Port=5432;Database=LoanDB;");
        public static IDbConnection Connection
        {
            get
            {
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
                return _connection;
            }
        }
        public static async Task<IEnumerable<T>> QueryAsync<T>(string sql)
             => await _connection.QueryAsync<T>(sql);
        public static async Task<int> ExecuteAsync(string sql)
            => await _connection.ExecuteAsync(sql);
        public static void DataReaderReady()
            => _dataReaderState = true;
        public static void DataReaderBusy()
            => _dataReaderState = false;
        public static bool DataReaderState => _dataReaderState;
    }
}
