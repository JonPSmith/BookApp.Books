// Copyright (c) 2020 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ModMon.Books.Persistence;
using ModMon.Books.Persistence.QueryObjects;
using ModMon.Books.ServiceLayer.Common;
using ModMon.Books.ServiceLayer.Udfs.Dtos;
using ModMon.Books.ServiceLayer.Udfs.QueryObjects;

namespace ModMon.Books.ServiceLayer.Udfs.Services
{
    public class ListUdfsBooksService : IListUdfsBooksService
    {
        private readonly BookDbContext _context;

        public ListUdfsBooksService(BookDbContext context)
        {
            _context = context;
        }

        public async Task<IQueryable<UdfsBookListDto>> SortFilterPageAsync
            (SortFilterPageOptions options)
        {
            var preQuery = _context.Books
                .AsNoTracking();
            if (options.FilterBy == BooksFilterBy.ByTags)
            {
                preQuery = preQuery.Where(x =>
                    x.Tags.Select(y => y.TagId)
                        .Contains(options.FilterValue));
            }

            var booksQuery = preQuery
                .MapBookUdfsToDto()
                .OrderUdfsBooksBy(options.OrderByOptions)
                .FilterUdfsBooksBy(options.FilterBy,
                    options.FilterValue);

            await options.SetupRestOfDtoAsync(booksQuery);

            return booksQuery.Page(options.PageNum - 1,
                options.PageSize);
        }
    }


}