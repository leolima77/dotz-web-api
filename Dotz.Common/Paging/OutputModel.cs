using System.Collections.Generic;

namespace Dotz.Common.Paging
{
    public class OutputModel<T>
    {
        public PagingHeader Paging { get; set; }

        public List<LinkInfo> Links { get; set; }

        public List<T> Items { get; set; }
    }
}