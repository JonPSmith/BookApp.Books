// Copyright (c) 2020 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ModMon.Books.Persistence;
using ModMon.Books.ServiceLayer.Common;
using ModMon.Books.ServiceLayer.Common.Dtos;
using ModMon.Books.ServiceLayer.GoodLinq.QueryObjects;

namespace ModMon.Books.ServiceLayer.GoodLinq.Services
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