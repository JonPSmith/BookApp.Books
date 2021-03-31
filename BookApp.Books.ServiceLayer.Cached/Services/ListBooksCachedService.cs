// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ModMon.Books.Persistence;
using ModMon.Books.Persistence.QueryObjects;
using ModMon.Books.ServiceLayer.Cached.QueryObjects;
using ModMon.Books.ServiceLayer.Common;
using ModMon.Books.ServiceLayer.Common.Dtos;

namespace ModMon.Books.ServiceLayer.Cached.Services
{
    public class ListBooksCachedService : IListBooksCachedService
    {
        private readonly BookDbContext _context;

        public ListBooksCachedService(BookDbContext context)
        {
            _context = context;
        }

        public async Task<IQueryable<BookListDto>> SortFilterPageAsync
            (SortFilterPageOptions options)
        {
            var booksQuery = _context.Books 
                .AsNoTracking() 
                .MapBookCachedToDto() 
                .OrderBooksBy(options.OrderByOptions) 
                .FilterBooksBy(options.FilterBy, 
                    options.FilterValue); 

            await options.SetupRestOfDtoAsync(booksQuery); 

            return booksQuery.Page(options.PageNum - 1, 
                options.PageSize); 
        }
    }


}