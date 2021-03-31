// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Linq;
using System.Threading.Tasks;
using BookApp.Books.Persistence;
using BookApp.Books.Persistence.QueryObjects;
using BookApp.Books.ServiceLayer.Common;
using BookApp.Books.ServiceLayer.Common.Dtos;
using BookApp.Books.ServiceLayer.GoodLinq.QueryObjects;
using Microsoft.EntityFrameworkCore;

namespace BookApp.Books.ServiceLayer.GoodLinq.Services
{
    public class ListBooksService : IListBooksService
    {
        private readonly BookDbContext _context;

        public ListBooksService(BookDbContext context)
        {
            _context = context;
        }

        public async Task<IQueryable<BookListDto>> SortFilterPageAsync
            (SortFilterPageOptions options)
        {
            var booksQuery = _context.Books 
                .AsNoTracking() 
                .MapBookToDto() 
                .OrderBooksBy(options.OrderByOptions) 
                .FilterBooksBy(options.FilterBy, 
                    options.FilterValue); 

            await options.SetupRestOfDtoAsync(booksQuery); 

            return booksQuery.Page(options.PageNum - 1, 
                options.PageSize); 
        }
    }


}