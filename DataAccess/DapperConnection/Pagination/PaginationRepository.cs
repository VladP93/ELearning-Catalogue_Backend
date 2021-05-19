using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace DataAccess.DapperConnection.Pagination
{
    public class PaginationRepository : IPagination
    {
        private readonly IFactoryConnection _factoryConnection;
        public PaginationRepository(IFactoryConnection factoryConnection)
        {
            _factoryConnection = factoryConnection;
        }

        public async Task<PaginationModel> returnPagination(string storedProcedure, int pageNumber, int quantity, IDictionary<string, object> fitlerParams, string columnSort)
        {
            PaginationModel paginationModel = new PaginationModel();
            List<IDictionary<string, object>> reportList = null;
            int totalRecords = 0;
            int totalPages = 0;

            try
            {
                var connection = _factoryConnection.GetConnection();
                DynamicParameters parameters = new DynamicParameters();

                foreach(var param in fitlerParams)
                {
                    parameters.Add("@"+param.Key, param.Value);
                }

                parameters.Add("@pageNumber",pageNumber);
                parameters.Add("@quantity",quantity);
                parameters.Add("@columnSort", columnSort);

                parameters.Add("@totalRecords",totalRecords, DbType.Int32, ParameterDirection.Output);
                parameters.Add("@totalPages", totalPages, DbType.Int32, ParameterDirection.Output);

                var response = await connection.QueryAsync(storedProcedure, parameters, commandType : CommandType.StoredProcedure);
                reportList = response.Select(d => (IDictionary<string,object>)d ).ToList();
                paginationModel.RecordsList = reportList;
                paginationModel.Quantity = parameters.Get<int>("@totalPages");
                paginationModel.RecordsTotal = parameters.Get<int>("@totalRecords");
            }
            catch (Exception e)
            {            
                throw new Exception("Cannot execute stored procedure", e);
            }
            finally
            {
                _factoryConnection.CloseConnection();
            }

            return paginationModel;
        }
    }
}