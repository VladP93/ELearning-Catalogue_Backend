using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.DapperConnection.Pagination
{
    public interface IPagination
    {
        Task<PaginationModel> returnPagination
        (
            string storedProcedure,
            int pageNumber,
            int quantity,
            IDictionary<string, object> fitlerParams,
            string columnSort
        );
    }
}