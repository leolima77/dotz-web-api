using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Dotz.Common.Paging
{
    public class PagingHeader
    {
        #region Constructor

        public PagingHeader(
           int totalItems, int pageNumber, int pageSize, int totalPages)
        {
            this.TotalItems = totalItems;
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.TotalPages = totalPages;
        }

        #endregion Constructor

        public int TotalItems { get; }

        public int PageNumber { get; }

        public int PageSize { get; }

        public int TotalPages { get; }

        public string ToJson() => JsonConvert.SerializeObject(this,
                                    new JsonSerializerSettings
                                    {
                                        ContractResolver = new
                    CamelCasePropertyNamesContractResolver()
                                    });
    }
}