/**
* 
* @author : Kai Tam    
*  
*/

using Company.Service.Extensions;
using Company.Service.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace Company.Service.Factories
{
    public class DatabaseFactory : IDatabaseFactory
    {
        private readonly string dbConnection;

        public DatabaseFactory(string dbConnection)
        {
            this.dbConnection = dbConnection.ThrowIfArgumentIsNullOrEmpty(nameof(dbConnection));
        }

        public IDbConnection GetSqlConnection()
        {
            return new SqlConnection(dbConnection);
        }
    }
}
