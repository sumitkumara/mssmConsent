using Dapper;
using MssmConsent.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MssmConsent.Services
{
    public class SqlService : ISqlService
    {
        public string ConnectionString { get; set; }

        public SqlService()
        {
            //_clientSecrets = clientSecrets;
        }

        // todo: client secret should be used instead of connection string
        private SqlConnection GetSqlConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public async Task<IEnumerable<T>> ExecuteQueryAsync<T>(string command, CommandType commandType, IDictionary<string, string> parameters = null)
        {
            DynamicParameters param = null;
            if (parameters?.Count > 0)
            {
                param = new DynamicParameters();
                foreach (var item in parameters)
                {
                    param.Add($"@{item.Key}", item.Value);
                }
            }
            using (var connection = GetSqlConnection())
            {
                await connection.OpenAsync();

                var result = await connection.QueryAsync<T>(command, commandTimeout: 120, commandType: commandType, param: param);
                return result;
            }
        }

        public async Task<IEnumerable<T>[]> QueryMultipleAsync<T>(string command, CommandType commandType, IDictionary<string, string> parameters = null)
        {
            DynamicParameters param = null;
            if (parameters?.Count > 0)
            {
                param = new DynamicParameters();
                foreach (var item in parameters)
                {
                    param.Add($"@{item.Key}", item.Value);
                }
            }
            using (var connection = GetSqlConnection())
            {
                await connection.OpenAsync();
                using (var multi = await connection.QueryMultipleAsync(command, commandTimeout: 120, commandType: CommandType.StoredProcedure, param: param))
                {
                    var result1 = await multi.ReadAsync<T>();
                    var result2 = await multi.ReadAsync<T>();
                    var result3 = await multi.ReadAsync<T>();
                    return new[] { result1, result2, result3 };
                }
            }
        }

        public async Task ExecuteScalarAsync(string command)
        {
            using (var connection = GetSqlConnection())
            {
                await connection.OpenAsync();
                await connection.ExecuteScalarAsync(command);
            }
        }

        public async Task ExecuteAsync(string command, DynamicParameters parameters = null)
        {
            using (var connection = GetSqlConnection())
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(command, parameters, commandType: CommandType.StoredProcedure);
            }
        }

    }
}
