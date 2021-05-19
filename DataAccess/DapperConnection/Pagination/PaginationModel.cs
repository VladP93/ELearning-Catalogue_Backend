using System.Collections.Generic;

namespace DataAccess.DapperConnection.Pagination
{
    public class PaginationModel
    {
        public List<IDictionary<string, object>> RecordsList { get; set; }
        public int RecordsTotal { get; set; }
        public int Quantity { get; set; }
    }
}