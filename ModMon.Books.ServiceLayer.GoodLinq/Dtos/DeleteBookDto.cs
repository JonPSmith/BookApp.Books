// Copyright (c) 2020 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using GenericServices;
using ModMon.Books.Domain;

namespace ModMon.Books.ServiceLayer.GoodLinq.Dtos
{
    public class DeleteBookDto : ILinkToEntity<Book>
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string AuthorsOrdered { get; set; }
    }
}