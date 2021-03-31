// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Linq;
using System.Threading.Tasks;
using BookApp.Books.Persistence;
using BookApp.Books.Persistence.QueryObjects;
using BookApp.Books.ServiceLayer.Cached.QueryObjects;
using BookApp.Books.ServiceLayer.Common;
using BookApp.Books.ServiceLayer.Common.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BookApp.Books.ServiceLayer.Cached.Services
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