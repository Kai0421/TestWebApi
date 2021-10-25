/**
* 
* @author : Kai Tam    
*  
*/

using System.Data;

namespace Company.Service.Interfaces
{
    public interface IDatabaseFactory
    {
        IDbConnection GetSqlConnection();
    }
}
