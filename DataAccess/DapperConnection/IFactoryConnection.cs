using System.Data;

namespace DataAccess.DapperConnection
{
    public interface IFactoryConnection
    {
         void CloseConnection();
         IDbConnection GetConnection();
    }
}