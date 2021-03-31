// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Linq;
using System.Threading.Tasks;
using BookApp.Books.ServiceLayer.Common;
using BookApp.Books.ServiceLayer.Common.Dtos;

namespace BookApp.Books.ServiceLayer.GoodLinq
{
    public interface IListBooksService
    {
        Task<IQueryable<BookListDto>> SortFilterPageAsync(SortFilterPageOptions options);
    }
}