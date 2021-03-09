// Copyright (c) 2020 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Linq;
using Microsoft.EntityFrameworkCore;
using ModMon.Books.Persistence;
using ModMon.Books.Persistence.QueryObjects;
using ModMon.Books.ServiceLayer.Cached.QueryObjects;
using ModMon.Books.ServiceLayer.Common;
using ModMon.Books.ServiceLayer.Common.Dtos;

namespace ModMon.Books.ServiceLayer.Cached.Services
{
    public class ListBooksCachedNoCountService : IListBooksCachedNoCountService
    {
        private readonly BookDbContext _context;

        public ListBooksCachedNoCountService(BookDbContext context)
        {
            _context = context;
        }

        public IQueryable<BookListDto> SortFilterPage
            (SortFilterPageOptionsNoCount options)
        {
            var booksQuery = _context.Books
                .AsNoTracking()
                .MapBookCachedToDto()
                .OrderBooksBy(options.OrderByOptions)
                .FilterBooksBy(options.FilterBy, options.FilterValue)
                .Page(options.PageNum - 1, options.PageSize);

            return booksQuery;
        }
    }


}