// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Linq;
using ModMon.Books.ServiceLayer.Common;
using ModMon.Books.ServiceLayer.Common.Dtos;

namespace ModMon.Books.ServiceLayer.Cached
{
    public interface IListBooksCachedNoCountService
    {
        IQueryable<BookListDto> SortFilterPage
            (SortFilterPageOptionsNoCount options);
    }
}