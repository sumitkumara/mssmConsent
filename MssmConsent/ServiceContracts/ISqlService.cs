using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace MssmConsent.ServiceContracts
{
    public interface ISqlService
    {
        string ConnectionString { get; set; }
        Task<IEnumerable<T>> ExecuteQueryAsync<T>(string command, CommandType commandType, IDictionary<string, string> parameters = null);
        Task ExecuteScalarAsync(string command);
        Task<IEnumerable<T>[]> QueryMultipleAsync<T>(string command, CommandType commandType, IDictionary<string, string> parameters = null);
    }
}
