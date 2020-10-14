using Dotz.Common.Paging.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Dotz.Common.Paging
{
    public class PagingLinks<T> : IPagingLinks<T>
    {
        #region Variables

        private readonly IUrlHelper _urlHelper;

        #endregion Variables

        #region Constructor

        public PagingLinks(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        #endregion Constructor

        #region BusinessSection

        public List<LinkInfo> GetLinks(PagedList<T> list)
        {
            var links = new List<LinkInfo>();

            if (list.HasPreviousPage)
                links.Add(CreateLink("default", list.PreviousPageNumber,
                           list.PageSize, "previousPage", "GET"));

            links.Add(CreateLink("default", list.PageNumber,
                           list.PageSize, "self", "GET"));

            if (list.HasNextPage)
                links.Add(CreateLink("default", list.NextPageNumber,
                           list.PageSize, "nextPage", "GET"));

            return links;
        }

        public LinkInfo CreateLink(string routeName, int pageNumber, int pageSize, string rel, string method)
        {
            return new LinkInfo
            {
                Href = _urlHelper.Link(routeName,
                            new { PageNumber = pageNumber, PageSize = pageSize }),
                Rel = rel,
                Method = method
            };
        }

        #endregion BusinessSection
    }
}