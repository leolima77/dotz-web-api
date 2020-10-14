using System.Collections.Generic;

namespace Dotz.Common.Paging.Interface
{
    public interface IPagingLinks<T>
    {
        List<LinkInfo> GetLinks(PagedList<T> list);

        LinkInfo CreateLink(string routeName, int pageNumber, int pageSize, string rel, string method);
    }
}